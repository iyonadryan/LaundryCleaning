using Azure;
using HotChocolate.AspNetCore;
using HotChocolate.Types;
using LaundryCleaning;
using LaundryCleaning.Auth;
using LaundryCleaning.Common.Constants;
using LaundryCleaning.Common.Filters;
using LaundryCleaning.Common.Middleware;
using LaundryCleaning.Data;
using LaundryCleaning.Download;
using LaundryCleaning.Extensions;
using LaundryCleaning.Security;
using LaundryCleaning.Security.Permissions;
using LaundryCleaning.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Globalization;
using System.Reflection;
using System.Text;

var cultureInfo = new CultureInfo("id-ID");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var builder = WebApplication.CreateBuilder(args);

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();

builder.Services
    .AddGraphQLServer()
    .AddAuthorization() 
    .UseField<AuthMiddleware>() 
    .AddInMemorySubscriptions()
    .AddQueryType(d => d.Name(ExtendObjectTypeConstants.Query))
    .AddMutationType(d => d.Name(ExtendObjectTypeConstants.Mutation))
    .AddSubscriptionType(d => d.Name(ExtendObjectTypeConstants.Subscription))
    .AddType<UploadType>()
    .AddTypeExtensionsFromAssembly(Assembly.GetExecutingAssembly())
    .AddErrorFilter<BusinessErrorFilter>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddProjectServices();

var seqSettings = builder.Configuration.GetSection("SeqSettings");
var seqSecretKey = seqSettings["SecretKey"];

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.Seq("http://localhost:5341", apiKey: seqSecretKey)
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

app.UseStaticFiles();

app.UseAuthentication(); 
app.UseAuthorization();

app.MapLogin(); // endpoint login
app.MapDownloadEndpoint(); // endpoint download

app.UseWebSockets();

app.UseMiddleware<GraphQLErrorLoggerMiddleware>(); // For Error GraphQL Response

app.MapGraphQL();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

var dbContext = services.GetRequiredService<ApplicationDbContext>();
if (dbContext.Database.GetDbConnection().Database != null)
{
    Console.WriteLine("🔍 Check database...");
}

// 'dotnet ef database update'
dbContext.Database.Migrate();
Console.WriteLine("✅ Migration done / up-to-date");

var passwordService = services.GetRequiredService<IPasswordService>();
var seeder = new DatabaseSeeder(dbContext, passwordService);

seeder.SeedAll();

app.Run();

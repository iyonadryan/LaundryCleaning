using DinkToPdf;
using DinkToPdf.Contracts;
using HotChocolate.Subscriptions;
using LaundryCleaning.Auth.Services.Implementations;
using LaundryCleaning.Auth.Services.Interfaces;
using LaundryCleaning.Common.Handlers;
using LaundryCleaning.Common.Models.Messages;
using LaundryCleaning.Data;
using LaundryCleaning.Download;
using LaundryCleaning.GraphQL.Files.Services.Implementations;
using LaundryCleaning.GraphQL.Files.Services.Interfaces;
using LaundryCleaning.GraphQL.Roles.Services.Implementations;
using LaundryCleaning.GraphQL.Roles.Services.Interfaces;
using LaundryCleaning.GraphQL.Users.Services.Implementations;
using LaundryCleaning.GraphQL.Users.Services.Interfaces;
using LaundryCleaning.Services.Background;
using LaundryCleaning.Services.Dispatcher;
using LaundryCleaning.Services.Implementations;
using LaundryCleaning.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace LaundryCleaning.Extensions
{
    public static class ServiceRegistrationExtensions
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services)
        {
            #region Services
            services.AddScoped<DatabaseSeeder>();
            services.AddScoped<IInvoiceNumberService, InvoiceNumberService>();
            services.AddScoped<IPasswordHasher<object>, PasswordHasher<object>>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<IPublisherService, PublisherService>();
            services.AddHostedService<ReceivedBackgroundService>();
            services.AddSingleton<SecureDownloadHelper>();
            #endregion

            var context = new CustomAssemblyLoadContext();
            context.LoadUnmanagedLibrary(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Storages", "Lib", "libwkhtmltox", "libwkhtmltox.dll"));
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

            #region Dispatcher
            services.AddSingleton<SystemMessageDispatcher>();
            #endregion

            #region Handler
            services.AddScoped<ISystemMessageHandler<UserNotification>, UserNotificationHandler>();
            #endregion

            #region GraphQL Services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();
            #endregion

            return services;
        }
    }
}

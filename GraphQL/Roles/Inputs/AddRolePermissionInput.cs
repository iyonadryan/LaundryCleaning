namespace LaundryCleaning.GraphQL.Roles.Inputs
{
    public class AddRolePermissionInput
    {
        public Guid RoleId { get; set; }
        public string Permission {  get; set; }
    }
}

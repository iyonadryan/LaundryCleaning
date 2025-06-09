using LaundryCleaning.Common.Domain;

namespace LaundryCleaning.Common.Models.Entities
{
    public class RolePermission : EntityBase
    {
        public Guid RoleId { get; set; }
        public string Permission { get; set; } = string.Empty;
    }
}

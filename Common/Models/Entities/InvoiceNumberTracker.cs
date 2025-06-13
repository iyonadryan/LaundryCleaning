using LaundryCleaning.Common.Domain;

namespace LaundryCleaning.Common.Models.Entities
{
    public class InvoiceNumberTracker : EntityBase
    {
        public string Code { get; set; } = string.Empty;
        public int LastNumber { get; set; } 
    }
}

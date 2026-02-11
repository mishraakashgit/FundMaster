using System.ComponentModel.DataAnnotations;

namespace FundNavTracker.Models
{
    public class AlertCreateRequest
    {
        [Required]
        [EmailAddress]
        public string UserEmail { get; set; } = string.Empty;

        [Required]
        public string FundCode { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue)]
        public decimal TargetNav { get; set; }

        [Required]
        public AlertDirection Direction { get; set; }
    }
}

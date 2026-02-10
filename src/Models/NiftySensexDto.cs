namespace FundNavTracker.Models
{
    public class NiftySensexDto
    {
        public required string IndexName { get; set; }
        public decimal CurrentValue { get; set; }
        public decimal Change { get; set; }
        public decimal PercentChange { get; set; }
        public required string LastUpdated { get; set; }
    }
}

using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FundNavTracker.Models
{
    public enum AlertDirection
    {
        Buy,
        Sell
    }

    public class NavAlert
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public required string UserEmail { get; set; }
        public required string FundCode { get; set; }
        public string? FundName { get; set; }
        public decimal TargetNav { get; set; }
        public AlertDirection Direction { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime? TriggeredAtUtc { get; set; }
    }
}

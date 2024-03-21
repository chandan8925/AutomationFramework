namespace BenefitsDashboardAPITests.APIModal
{
    /// <summary>
    /// Properties Of Employee Response Payload
    /// </summary>
    public class Employee
    {
        public string? partitionKey { get; set; }
        public string? sortKey { get; set; }
        public string? username { get; set; }
        public string? id { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public int? dependants { get; set; }
        public DateTime? expiration { get; set; }
        public decimal? salary { get; set; }
        public decimal? gross { get; set; }
        public decimal benefitsCost { get; set; }
        public decimal net { get; set; }
    }


    /// <summary>
    /// Properties of creating or updating an employee payload
    /// </summary>
    public class CreateUpdateEmployeeRequest
    {
        public string? id { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public int dependants { get; set; }
    }

}


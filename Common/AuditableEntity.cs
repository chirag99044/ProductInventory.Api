namespace ProductInventory.Api.Common
{
    public class AuditableEntity
    {
        public string Browser { get; set; }

        public DateTime Created { get; set; }

        public string CreatedBy { get; set; }

        public string DeletedBy { get; set; }

        public DateTime? DeletedDate { get; set; }

        public string EventCode { get; set; }

        public string IPaddress { get; set; }

        public string IPCity { get; set; }

        public string IPCountry { get; set; }

        public string IPOrg { get; set; }

        public bool IsDeleted { get; set; } = false;

        public DateTime? LastModified { get; set; }

        public string LastModifiedBy { get; set; }
    }
}

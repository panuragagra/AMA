using System;
using System.Collections.Generic;

namespace POAM.Models
{
    public partial class CertifiedUserList
    {
        public long Id { get; set; }
        public long? ChangeLogId { get; set; }
        public long? EmployeeId { get; set; }
        public int? OfficeId { get; set; }
        public string OfficeName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string EMail { get; set; }
        public string AccessLevelName { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public int? ApplicantionNamesId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}

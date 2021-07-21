using System;
using System.Collections.Generic;

namespace POAM.Models
{
    public partial class Office
    {
        public int OfficeId { get; set; }
        public string RouteSym { get; set; }
        public string OfficeName { get; set; }
        public int? MainOfficeId { get; set; }
        public bool? IsSubofficeDivision { get; set; }
        public bool? IsActive { get; set; }
        public bool IsVisibleInPhoneDirectory { get; set; }
        public string OrgCode { get; set; }
        public int? ParentOffice { get; set; }
        public bool? IsParentOffice { get; set; }
        public string Title { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? Updated { get; set; }
    }
}

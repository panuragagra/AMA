using System;
using System.Collections.Generic;

namespace POAM.Models
{
    public partial class ChangeLog
    {
        public long Id { get; set; }
        public string ChangeDesc { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? ApplicantionNamesId { get; set; }

        public bool? IsRecertified { get; set; }
    }
}

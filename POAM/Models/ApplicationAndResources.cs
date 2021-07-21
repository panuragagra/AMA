using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace POAM.Models
{
    public class SuperSet
    {
        public IEnumerable<VUserAccountExternalAndInternalListOAsisWithUrlAndCertifiedDate>  vOasis { get; set; }
        public IEnumerable<ApplicationNames> applicationNames { get; set; }
        public IEnumerable<VChangeLog>  changeLogs { get; set; }

        public string strShowChangeLog { get; set; }

        [Required(ErrorMessage = "Required")]
        public int? SelectedApplicationID { get; set; }

        public string SelectedApplicationName { get; set; }

        public string SelectedApplicationURL { get; set; }

        public string strScrollPosition { get; set; }

    }

    public partial class VChangeLog
    {
        public long id { get; set; }

        [DataMember(Order = 2)]
        [DisplayName("Event Description")]
        public string ChangeDesc { get; set; }
        public Nullable<int> UpdatedBy { get; set; }

        [DataMember(Order = 4)]
        [DisplayName("Certified Date")]
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> ApplicantionNamesID { get; set; }
        public Nullable<int> EmployeeId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }

        [DataMember(Order = 3)]
        [DisplayName("Certified By")]
        public string FullNameWithOffice { get; set; }
        public string RouteSym { get; set; }
        public Nullable<int> OfficeID { get; set; }
        public Nullable<int> MainOfficeID { get; set; }
        public Nullable<int> ParentOffice { get; set; }
        public string MainOffice { get; set; }

        [DataMember(Order = 1)]
        [DisplayName("Application Name")]
        public string ApplicationName { get; set; }
    }


    public partial class VUserAccountExternalAndInternalListOAsisWithUrlAndCertifiedDate
    {
        [DataMember(Order = 9)]
        public int applicationid { get; set; }
        [DataMember(Order = 8)]
        [DisplayName("Application Name")]
        public string Applicationname { get; set; }
        [DataMember(Order = 7)]
        public int EmployeeID { get; set; }
        [DisplayName("E-Mail")]
        [DataMember(Order = 3)]
        public string E_Mail { get; set; }
        [DataMember(Order = 1)]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [DataMember(Order = 2)]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DataMember(Order = 4)]
        [DisplayName("Access Level")]
        public string AccessLevel { get; set; }
        [DataMember(Order = 5)]
        [DisplayName("Last Login Date")]
        public Nullable<System.DateTime> LastLogin { get; set; }

        [DataMember(Order = 10)]
        [DisplayName("Last Certified Date")]
        public Nullable<System.DateTime> LastCertifiedDate { get; set; }

        [DataMember(Order = 6)]
        public string Url { get; set; }

        [DataMember(Order = 0)]
        public string Office { get; set; }
}

    public partial class VUserAccountExternalAndInternalListOASI
    {
        public int applicationid { get; set; }
        public string Applicationname { get; set; }
        public int EmployeeID { get; set; }
        public string E_Mail { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string AccessLevel { get; set; }
        public Nullable<System.DateTime> LastLogin { get; set; }
    }

    public partial class VUserAccountExternalList
    {
        public int applicationid { get; set; }
        public string Applicationname { get; set; }
        public int EmployeeID { get; set; }
        public string E_Mail { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string AccessLevel { get; set; }
        public Nullable<System.DateTime> LastLogin { get; set; }
    }



    public class VOASIS
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public Boolean IsActive { get; set; }

        [Description("Last name")]
        [DisplayName("Last name")]
        [Display(Name = "Last name")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string RouteSym { get; set; }
        public int OfficeID { get; set; }
        public int MainOfficeID { get; set; }
        public int ParentOffice { get; set; }
        public string MainOffice { get; set; }
    }

    public partial class VUserAccountListAdminsOASIS
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string RouteSym { get; set; }
        public int OfficeID { get; set; }
        public Nullable<int> MainOfficeID { get; set; }
        public Nullable<int> ParentOffice { get; set; }
        public string MainOffice { get; set; }
        public int applicationid { get; set; }
        public string Applicationname { get; set; }
        public Nullable<int> EmployeeID { get; set; }
        public string AccessLevel { get; set; }
        public Nullable<System.DateTime> LastLogin { get; set; }
        public string URL { get; set; }
    }

    public class DropdownValues
    {
        public int? ID { get; set; }
        public string Name { get; set; }


    }
    public class Applications
    {
        public int ID { get; set; }
        public string Name { get; set; }
        
        public virtual Employees Employees { get; set; }
    }

    public class Employees
    {
        public Employees()
        {
            this.Application = new HashSet<Applications>();
        }
        public int ID { get; set; }
        public string Name { get; set; }
        
        public virtual ICollection<Applications> Application { get; set; }
    }

}

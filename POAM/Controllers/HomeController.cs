using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ExportExcel.Code;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using POAM.Code;
using POAM.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace POAM.Controllers
{
    [Authorize(Roles = Role.Admin)]
    public class HomeController : Controller
    {

        [HttpPost]
        public ActionResult Index(SuperSet model, string ApplicationNames, string strShowChangeLog, string submit)
        {
            ViewBag.IDMainContextGridView = "none";
            ViewBag.IDShowHideChangeLog = "none";
            POAMContext pOAMContext = new POAMContext();

            switch (submit)
            {
                case "Load Application User Data":
                    // Do something
                    break;
                case "Click to Certify":
                    // Do something
                    break;
            }


            if (!ModelState.IsValid)
            {
                // For throwing validation errors              

                model = GetModalDetails(pOAMContext);
                return View(model);
            }
            ViewBag.IDMainContextGridView = "inline";
            ViewBag.IDShowHideChangeLog = "inline";
            HttpContext.Session.Set<int>(ConstantValues.SelectedApplicationID, (int)model.SelectedApplicationID);
            SessionValues.SelectedApplicationID = (int)model.SelectedApplicationID;
            // At this stage the model is OK => do something with the selected category
            model = GetModalDetails(pOAMContext);

            return View(model);
        }

        //[AllowAnonymous]
        [ActionName("Index")]
        [Authorize(Roles = Role.Admin)]
        [HttpGet]
        public IActionResult Index_Get()
        {
            try
            {
                int SelectedApplicationID = Convert.ToInt32(Request.Form["applicationNames"].ToString());
            }
            catch (Exception)
            {


            }


            POAMContext pOAMContext = new POAMContext();
            LoadApplicationnames("", pOAMContext);




            SuperSet model = GetModalDetails(pOAMContext);

            try
            {
                if (Request.Form["strShowChangeLog"].ToString() == "")
                {
                    model.strShowChangeLog = Request.Form["strShowChangeLog"].ToString();
                }
            }
            catch (Exception)
            {


            }


            ViewBag.IDMainContextGridView = "none";
            ViewBag.IDShowHideChangeLog = "none";
            if (TempData["shortMessage"] != null)
            {
                if (TempData["shortMessage"].ToString() == "Details saved successfully.")
                {
                    ViewBag.IDMainContextGridView = "inline";
                    ViewBag.IDShowHideChangeLog = "inline";
                }
            }


            if (HttpContext.Session.Get<int>(ConstantValues.SelectedApplicationID) != -1)
            {
                ViewBag.IDMainContextGridView = "inline";
                ViewBag.IDShowHideChangeLog = "inline";
            }
            if (TempData["ScrollPositions"] != null)
            {
                model.strScrollPosition = TempData["ScrollPositions"].ToString();
            }

            return View(model);
        }

        [HttpGet]        
        public PartialViewResult LoadChangeLog_CertifiedUserList(string strChangeLogID)
        {
            if (strChangeLogID != null)
            {
                if (strChangeLogID.Trim() != "")
                {
                    POAMContext pOAMContext = new POAMContext();
                    var varCertifiedUserList = pOAMContext.CertifiedUserList.Where(e => e.ChangeLogId == Convert.ToInt64(strChangeLogID)).ToList();
                    return PartialView("LoadChangeLog_CertifiedUserList", varCertifiedUserList);
                }
                
            }  
            
                return PartialView();                       
        }

        [HttpPost]
        public ActionResult UndoCertificatonDetails(SuperSet model, string ApplicationNames, string strShowChangeLog , string hdnRecertificationChangeLogID)
        {


            TempData["shortMessage"] = " Recertification un-done successfully.";
            TempData["ScrollPositions"] = model.strScrollPosition;

            //return RedirectToAction("Index");
            POAMContext pOAMContext = new POAMContext();

            // undo the previous recertification details.
            if (hdnRecertificationChangeLogID != null && hdnRecertificationChangeLogID.Trim()!="")
            {

                //undo recertification
                ChangeLog changeLog_Update = new ChangeLog();

                changeLog_Update = pOAMContext.ChangeLog.FirstOrDefault(e => e.Id == Convert.ToInt64(hdnRecertificationChangeLogID));
                changeLog_Update.IsRecertified = false;
                pOAMContext.Attach(changeLog_Update);
                pOAMContext.Entry(changeLog_Update).Property("IsRecertified").IsModified = true;
                pOAMContext.SaveChanges();
            }
            

            
            ChangeLog changeLog = new ChangeLog();
            changeLog.ApplicantionNamesId = SessionValues.SelectedApplicationID;
            changeLog.ChangeDesc = SessionValues.strSelectedApplicationName + " Recertification un-done";
            changeLog.UpdatedBy = SessionValues.LoggedUserDetails.FirstOrDefault().Id;  //NameIdentifier
            changeLog.UpdatedDate = System.DateTime.Now;
            pOAMContext.Add(changeLog);
            pOAMContext.SaveChanges();

            var @OfficeID = DBNull.Value;
            var @AccessLevelName = DBNull.Value;
            var @OfficeName = DBNull.Value;
            //Insert date into Certified list
            //pOAMContext.Database.ExecuteSqlCommand("EXEC CertifiedUserList_Insert @ChangeLogID, @OfficeID,@AccessLevelName, @OfficeName ,@ApplicantionNamesID, @CreatedBy ",
            //parameters: new[] { changeLog.Id.ToString(), @OfficeID, @AccessLevelName, @OfficeName, SessionValues.SelectedApplicationID.ToString(), SessionValues.LoggedUserDetails.FirstOrDefault().Id.ToString() });

            var sql = @"EXEC [CertifiedUserList_Insert] @ChangeLogID = @ChangeLogID, @OfficeID = @OfficeID ,@AccessLevelName = @AccessLevelName, @OfficeName = @OfficeName ,@ApplicantionNamesID =@ApplicantionNamesID, @CreatedBy = @CreatedBy, @CreatedDate =@CreatedDate ";
            pOAMContext.Database.ExecuteSqlRaw(
                    sql,
                new SqlParameter("@ChangeLogID", changeLog.Id.ToString())
                , new SqlParameter("@OfficeID", @OfficeID)
                , new SqlParameter("@AccessLevelName", DBNull.Value)
                , new SqlParameter("@OfficeName", DBNull.Value)
                , new SqlParameter("@ApplicantionNamesID", SessionValues.SelectedApplicationID.ToString())
                , new SqlParameter("@CreatedBy", SessionValues.LoggedUserDetails.FirstOrDefault().Id.ToString())
                , new SqlParameter("@CreatedDate", changeLog.UpdatedDate)

                );

            model = GetModalDetails(pOAMContext);
            var varRecipients = pOAMContext.vUserAccountListAdminsOASIS.Where(e => e.applicationid == SessionValues.SelectedApplicationID).ToList();
            if (varRecipients != null)
            {
                string strRecipients = "";
                for (int i = 0; i < varRecipients.Count; i++)
                {
                    if (varRecipients[i].Email != null && varRecipients[i].Email.ToString() != "")
                    {
                        strRecipients = strRecipients + varRecipients[i].Email.ToString() + ";";
                    }

                }
                string CopyRecipients = "";
                string Subject = SessionValues.strSelectedApplicationName + " Recertification un-done Successfully";
                string Message = SessionValues.strSelectedApplicationName + " Recertification un-done successfully by " + SessionValues.LoggedUserDetails.FirstOrDefault().LastName + ", " + SessionValues.LoggedUserDetails.FirstOrDefault().FirstName + " on " + System.DateTime.Now.ToString("MM/dd/yyyy") + ". <br />";
                if (strRecipients != "")
                {
                    SendDBEmail(pOAMContext, strRecipients, CopyRecipients, Subject, Message);
                }

            }


            return RedirectToAction("Index");
        }



        [HttpPost]
        public ActionResult AddCertificatonDetails(SuperSet model, string ApplicationNames, string strShowChangeLog)
        {


            TempData["shortMessage"] = "User Accounts Recertified Successfully.";
            TempData["ScrollPositions"] = model.strScrollPosition;
            //return RedirectToAction("Index");
            POAMContext pOAMContext = new POAMContext();
            ChangeLog changeLog = new ChangeLog();
            changeLog.ApplicantionNamesId = SessionValues.SelectedApplicationID;
            changeLog.ChangeDesc = SessionValues.strSelectedApplicationName + " User Accounts Re-certified";
            changeLog.UpdatedBy = SessionValues.LoggedUserDetails.FirstOrDefault().Id;  //NameIdentifier
            changeLog.UpdatedDate = System.DateTime.Now;
            changeLog.IsRecertified = true;
            pOAMContext.Add(changeLog);
            pOAMContext.SaveChanges();

            var @OfficeID = DBNull.Value;
            var @AccessLevelName = DBNull.Value;
            var @OfficeName = DBNull.Value;
            //Insert date into Certified list
            //pOAMContext.Database.ExecuteSqlCommand("EXEC CertifiedUserList_Insert @ChangeLogID, @OfficeID,@AccessLevelName, @OfficeName ,@ApplicantionNamesID, @CreatedBy ",
            //parameters: new[] { changeLog.Id.ToString(), @OfficeID, @AccessLevelName, @OfficeName, SessionValues.SelectedApplicationID.ToString(), SessionValues.LoggedUserDetails.FirstOrDefault().Id.ToString() });
            TempData["RecertificationChangeLogID"] = changeLog.Id.ToString();

            var sql = @"EXEC [CertifiedUserList_Insert] @ChangeLogID = @ChangeLogID, @OfficeID = @OfficeID ,@AccessLevelName = @AccessLevelName, @OfficeName = @OfficeName ,@ApplicantionNamesID =@ApplicantionNamesID, @CreatedBy = @CreatedBy, @CreatedDate =@CreatedDate ";
            pOAMContext.Database.ExecuteSqlRaw(
                    sql,
                new SqlParameter("@ChangeLogID", changeLog.Id.ToString())
                ,new SqlParameter("@OfficeID", @OfficeID)
                , new SqlParameter("@AccessLevelName", DBNull.Value)
                , new SqlParameter("@OfficeName", DBNull.Value)
                , new SqlParameter("@ApplicantionNamesID", SessionValues.SelectedApplicationID.ToString())
                , new SqlParameter("@CreatedBy", SessionValues.LoggedUserDetails.FirstOrDefault().Id.ToString())
                , new SqlParameter("@CreatedDate", changeLog.UpdatedDate)

                );

            model = GetModalDetails(pOAMContext);
            var varRecipients = pOAMContext.vUserAccountListAdminsOASIS.Where(e => e.applicationid == SessionValues.SelectedApplicationID).ToList();
            if (varRecipients != null)
            {
                string strRecipients = "";
                for (int i = 0; i < varRecipients.Count; i++)
                {
                    if (varRecipients[i].Email != null && varRecipients[i].Email.ToString() != "")
                    {
                        strRecipients = strRecipients + varRecipients[i].Email.ToString() + ";";
                    }

                }
                string CopyRecipients = "";
                string Subject = SessionValues.strSelectedApplicationName + " User Accounts Recertified Successfully";
                string Message = SessionValues.strSelectedApplicationName + " user accounts recertified successfully by " + SessionValues.LoggedUserDetails.FirstOrDefault().LastName + ", " + SessionValues.LoggedUserDetails.FirstOrDefault().FirstName + " on " + System.DateTime.Now.ToString("MM/dd/yyyy") + ". <br />";
                if (strRecipients != "")
                {
                    SendDBEmail(pOAMContext, strRecipients, CopyRecipients, Subject, Message);
                }

            }


            return RedirectToAction("Index");
        }

        private static void SendDBEmail(POAMContext pOAMContext, string Recipients, string CopyRecipients, string Subject, string Message)
        {
            //sending email
            //pOAMContext.Database.ExecuteSqlCommand("usp_SendEmail @Recipients, @CopyRecipients , @Subject ,@Message ", parameters: new[] { Recipients, CopyRecipients, Subject, Message });

            pOAMContext.Database.ExecuteSqlRaw("EXEC usp_SendEmail @p0, @p1,@p2, @p3 ",
                parameters: new[] { Recipients, CopyRecipients, Subject, Message });

        }


        private SuperSet LoadIndexModel()
        {
            //Employees employees = NewMethod();
            POAMContext pOAMContext = new POAMContext();
            string strSessionShowChangeLog = "none";
            if (HttpContext.Session.Get<string>(ConstantValues.strShowChangeLog) != null)
            {
                strSessionShowChangeLog = HttpContext.Session.Get<string>(ConstantValues.strShowChangeLog);
            }
            SuperSet model = GetModalDetails(pOAMContext);
            return model;
        }

        private SuperSet GetModalDetails(POAMContext pOAMContext)
        {
            List<User> users = new List<User>();

            if (HttpContext.Session.Get<List<User>>(ConstantValues.UserDetails) != null)
            {
                users = (List<User>)HttpContext.Session.Get<List<User>>(ConstantValues.UserDetails);
            }

            int? SelectedApplicationID = null;

            string strSelectedApplicationName = "";
            string strSelectedApplicationURl = "";

            if (HttpContext.Session.Get<int>(ConstantValues.SelectedApplicationID) != -1)
            {
                SelectedApplicationID = HttpContext.Session.Get<int>(ConstantValues.SelectedApplicationID);
                var list = pOAMContext.ApplicationNames.Where(e => e.Id == SelectedApplicationID).FirstOrDefault();
                if (list != null)
                {
                    strSelectedApplicationName = list.Name;
                    SessionValues.strSelectedApplicationName = list.Name;
                    strSelectedApplicationURl = list.Url;
                    SessionValues.strSelectedApplicationURl = list.Url;
                }

            }


            string strSessionShowChangeLog = "none";
            if (HttpContext.Session.Get<string>(ConstantValues.strShowChangeLog) != null)
            {
                strSessionShowChangeLog = HttpContext.Session.Get<string>(ConstantValues.strShowChangeLog);
            }

            List<ApplicationNames> ApplicationNames = (from c in users
                                                       select new ApplicationNames { Id = ParseInt32(c.intApplicationID), Name = c.ApplicationName, Url = c.URL }
                                                   ).OrderBy(c => c.Name).ToList();

            return new SuperSet
            {
                vOasis = pOAMContext.VoasisUserNames.Where(e => e.applicationid == SelectedApplicationID).ToList(),
                applicationNames = ApplicationNames
                ,
                changeLogs = pOAMContext.VChangelog.Where(e => e.ApplicantionNamesID == SelectedApplicationID).OrderByDescending(e => e.UpdatedDate)
            ,
                strShowChangeLog = strSessionShowChangeLog,
                SelectedApplicationID = SelectedApplicationID
            ,
                SelectedApplicationName = strSelectedApplicationName
            ,
                SelectedApplicationURL = strSelectedApplicationURl
            };
        }
        public static int ParseInt32(string str, int defaultValue = 0)
        {
            int result;
            return Int32.TryParse(str, out result) ? result : defaultValue;
        }
        private void LoadApplicationnames(string strDDLValue, POAMContext pOAMContext)
        {
            List<User> users = new List<User>();

            if (HttpContext.Session.Get<List<User>>(ConstantValues.UserDetails) != null)
            {
                users = (List<User>)HttpContext.Session.Get<List<User>>(ConstantValues.UserDetails);
            }

            List<DropdownValues> ApplicationNames = (from c in users
                                                     select new DropdownValues { ID = ParseInt32(c.intApplicationID), Name = c.ApplicationName }
                                                    ).OrderBy(c => c.Name).ToList();
            //ViewBag.MainOfficeList = MainOfficeList;
            ViewBag.ApplicationNames = new SelectList(ApplicationNames, "ID", "Name", strDDLValue);
        }

        private static Employees NewMethod()
        {
            Applications ap = new Applications();
            ap.ID = 1;
            ap.Name = "COOP";

            ICollection<Applications> names = new List<Applications>();

            names.Add(new Applications { ID = 1, Name = "COOP" });

            Employees employees = new Employees { ID = 1, Name = "Mohan Krishna", Application = names };
            return employees;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult AcceptTermsAndConditions(string AcceptAndTerms)
        {
            // Request.Path.ToString().Contains("AcceptTermsAndConditions");
            HttpContext.Session.Set<string>(ConstantValues.SessionAcceptAndTermsClicked, "True");

            SuperSet model = LoadIndexModel();
            return View("Index", model);
        }

        [AllowAnonymous]
        [ActionName("AcceptTermsAndConditions")]
        public IActionResult AcceptTermsAndConditions_Get()
        {
            return View("AcceptTermsAndConditions");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [HttpPost]
        public JsonResult ShowHideChangeLog(string strShowChangeLog)
        {

            HttpContext.Session.Set<string>(ConstantValues.strShowChangeLog, strShowChangeLog);
            return Json(new { someString = strShowChangeLog });
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
            //AccessDenied
        }

        [HttpGet]
        public FileContentResult ExportToExcel()
        {

            POAMContext pOAMContext = new POAMContext();

            SuperSet model = GetModalDetails(pOAMContext);



            string[] columns = { "Office", "Last Name", "First Name", "E-Mail", "Access Level", "Last Login Date", "Last Certified Date" };
            string[] columnsOrderBy = { "Office", "LastName", "FirstName", "E_Mail", "AccessLevel", "LastLogin", "LastCertifiedDate" };
            byte[] filecontent = ExcelExportHelper.ExportExcel<VUserAccountExternalAndInternalListOAsisWithUrlAndCertifiedDate>(model.vOasis.ToList(), "User Details", false, columnsOrderBy, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "UserDetails.xlsx");
        }

        [HttpGet]
        public FileContentResult ExportToExcel_ChangeLog()
        {

            POAMContext pOAMContext = new POAMContext();

            SuperSet model = GetModalDetails(pOAMContext);



            string[] columns = { "Application Name", "Event Description", "Certified By", "Certified Date" };
            string[] columnsOrderBy = { "ApplicationName", "ChangeDesc", "FullNameWithOffice", "UpdatedDate" };
            byte[] filecontent = ExcelExportHelper.ExportExcel<VChangeLog>(model.changeLogs.ToList(), "Change Log Details", false, columnsOrderBy, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "ChangeLogDetails.xlsx");
        }
    }
}

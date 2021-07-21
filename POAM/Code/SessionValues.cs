using Microsoft.AspNetCore.Http;
using POAM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POAM.Code
{
    public static class SessionValues
    {
        private static IHttpContextAccessor _httpContextAccessor;

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static HttpContext Current => _httpContextAccessor.HttpContext;

        public static string strShowChangeLog
        {
            get
            {
                return _httpContextAccessor.HttpContext.Session.Get<string>(ConstantValues.strShowChangeLog);
            }

            set
            {
                _httpContextAccessor.HttpContext.Session.Set<string>(ConstantValues.strShowChangeLog, value);
            }

        }


        public static string strSelectedApplicationName
        {
            get
            {
                return _httpContextAccessor.HttpContext.Session.Get<string>(ConstantValues.strSelectedApplicationName);
            }

            set
            {
                _httpContextAccessor.HttpContext.Session.Set<string>(ConstantValues.strSelectedApplicationName, value);
            }

        }

        public static string strSelectedApplicationURl
        {
            get
            {
                return _httpContextAccessor.HttpContext.Session.Get<string>(ConstantValues.strSelectedApplicationURl);
            }

            set
            {
                _httpContextAccessor.HttpContext.Session.Set<string>(ConstantValues.strSelectedApplicationURl, value);
            }

        }

        public static int SelectedApplicationID
        {
            get
            {
                return _httpContextAccessor.HttpContext.Session.Get<int>(ConstantValues.SelectedApplicationID);
            }

            set
            {
                _httpContextAccessor.HttpContext.Session.Set<int>(ConstantValues.SelectedApplicationID, value);
            }

        }

        public static List<POAM.Models.User> LoggedUserDetails
        {
            get
            {
                return _httpContextAccessor.HttpContext.Session.Get<List<POAM.Models.User>>(ConstantValues.UserDetails);
            }

            set
            {
                _httpContextAccessor.HttpContext.Session.Set<List<POAM.Models.User>>(ConstantValues.UserDetails, value);
            }

        }

    }
}

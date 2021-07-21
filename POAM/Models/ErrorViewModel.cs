using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;

namespace POAM.Models
{
   

    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }

    public static class ConstantValues
    {
        public const string SessionKeyName = "_Name";
        public const string SessionAcceptAndTermsClicked = "AcceptAndTermsClicked";
        public const string UserDetails = "UserDetails";
        public const string strShowChangeLog = "strShowChangeLog";
        public const string SelectedApplicationID = "SelectedApplicationID";
        public const string strSelectedApplicationName = "strSelectedApplicationName";
        public const string strSelectedApplicationURl = "strSelectedApplicationURl";      
    }

    public static class SessionExtensions
    {
        

        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) :
                JsonConvert.DeserializeObject<T>(value);
        }
    }

}
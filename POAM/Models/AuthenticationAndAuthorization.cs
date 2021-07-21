using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
//using Microsoft.IdentityModel.Tokens;
//using System.DirectoryServices;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using System.Text.Encodings.Web;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http.Features.Authentication;
using POAM.Code;

namespace POAM.Models
{
    public class AuthenticationAndAuthorization
    {
    }


    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUserService _userService;
        IHttpContextAccessor _httpContextAccessor = null;


        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IHttpContextAccessor httpContextAccessor,
            IUserService userService

             )
            : base(options, logger, encoder, clock)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;


        }


        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            //if (!Request.Headers.ContainsKey("Authorization"))
            //  return AuthenticateResult.Fail("Missing Authorization Header");

            if (_httpContextAccessor.HttpContext.Session.Get<string>(ConstantValues.SessionAcceptAndTermsClicked) == null
             &&
             _httpContextAccessor.HttpContext.Session.Get<string>(ConstantValues.SessionAcceptAndTermsClicked) != "True"
             )
            {
                var claims_D = new[] {
                new Claim(ClaimTypes.NameIdentifier, "0"),
                new Claim(ClaimTypes.Name, "0"),
                new Claim(ClaimTypes.Role, "0")
            };
                var identity_d = new ClaimsIdentity(claims_D, Scheme.Name);
                var principal_d = new ClaimsPrincipal(identity_d);
                var ticket_d = new AuthenticationTicket(principal_d, Scheme.Name);

                return AuthenticateResult.Success(ticket_d);
                //return AuthenticateResult.Fail("Not accepted Terms and Conditions");
            }


            User user = null;
            HttpContext httpContext = _httpContextAccessor.HttpContext;


            try
            {
                var username = ((System.Security.Principal.WindowsIdentity)((WindowsPrincipal)httpContext.User).Identity).Name.Split('\\')[1];
                var password = "";
                user = await _userService.Authenticate(username, password);
            }
            catch
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }

            if (user == null)
            {
                // AuthenticateResult.Fail()
                return AuthenticateResult.Fail("Invalid Username or Password");
            }


            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }

        //protected override async Task<bool> HandleForbiddenAsync(ChallengeContext context)
        //{
        //    var properties = new AuthenticationProperties(context.Properties);
        //    var returnUrl = properties.RedirectUri;
        //    if (string.IsNullOrEmpty(returnUrl))
        //    {
        //        returnUrl = OriginalPathBase + Request.Path + Request.QueryString;
        //    }
        //    var accessDeniedUri = Options.AccessDeniedPath + QueryString.Create(Options.ReturnUrlParameter, returnUrl);
        //    var redirectContext = new CookieRedirectContext(Context, Options, BuildRedirectUri(accessDeniedUri), properties);
        //    await Options.Events.RedirectToAccessDenied(redirectContext);
        //    return true;
        //}

        protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            try
            {
                if (_httpContextAccessor.HttpContext.Session.Get<string>(ConstantValues.SessionAcceptAndTermsClicked) == null
              &&
              _httpContextAccessor.HttpContext.Session.Get<string>(ConstantValues.SessionAcceptAndTermsClicked) != "True"
              )
                {
                    Context.Response.Redirect("/Home/AcceptTermsAndConditions");
                    return Task.CompletedTask;
                }
                else
                {
                    Context.Response.Redirect("/Home/AccessDenied");
                }

                return Task.CompletedTask;
            }
            catch (Exception)
            {

                throw;
            }

           

            // Response.Headers["WWW-Authenticate"] = $"Basic realm=\"{Options.Realm}\", charset=\"UTF-8\"";
            // properties.RedirectUri = "/Home/Privacy";

            // return base.HandleForbiddenAsync(properties);
        }
        //protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        //{
        //    Context.Response.Redirect("/Home/Error");
        //    // Response.Headers["WWW-Authenticate"] = $"Basic realm=\"{Options.Realm}\", charset=\"UTF-8\"";
        //    await base.HandleChallengeAsync(properties);
        //}

    }


    public static class Role
    {
        public const string Admin = "Administrator";
        public const string User = "User";
    }

    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        public string ApplicationName { get; set; }
        public string intApplicationID { get; set; }
        public string URL { get; set; }
    }



    public class AppSettings
    {
        public string Secret { get; set; }
        //  public string LoggedInuserId = ((System.Security.Principal.WindowsIdentity)((WindowsPrincipal)System.Web.HttpContext.HttpContext.User).Identity).Name.Split('\\')[1];


    }

    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);
        Task<IEnumerable<User>> GetAll();
    }

    public class UserService : IUserService
    {
        private List<User> _users;

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            
            if (httpContextAccessor.HttpContext.Session.Get<List<User>>(ConstantValues.UserDetails) != null)
            {
                _users = (List<User>) httpContextAccessor.HttpContext.Session.Get<List<User>>(ConstantValues.UserDetails);
            }
            else
            {
                string strCurrentlyLoggedinUserName = ((System.Security.Principal.WindowsIdentity)((WindowsPrincipal)httpContextAccessor.HttpContext.User).Identity).Name.Split('\\')[1];
                POAMContext pOAMContext = new POAMContext();


                _users = (from vAdminOASIS in pOAMContext.vUserAccountListAdminsOASIS
                          where vAdminOASIS.UserName.ToLower() == strCurrentlyLoggedinUserName.ToLower()
                          select new User
                          {
                              ApplicationName = vAdminOASIS.Applicationname,
                              FirstName = vAdminOASIS.FirstName,
                              Id = vAdminOASIS.ID,
                              LastName = vAdminOASIS.LastName,
                              Role = vAdminOASIS.AccessLevel,
                              Username = vAdminOASIS.UserName.ToLower(),
                              intApplicationID = vAdminOASIS.applicationid.ToString(),
                              URL=vAdminOASIS.URL

                          }).ToList();

                //// users hardcoded for simplicity, store in a db with hashed passwords in production applications
                //        _users = new List<User>
                //{
                //    new User { Id = 1, FirstName = "Test", LastName = "User", Username = "m.mohankrishna.ctr1", Password = "", Role= Role.Admin },
                //    new User { Id = 1, FirstName = "Test", LastName = "User", Username = "l.c.sukumar.ctr", Password = "", Role= Role.Admin },
                //    new User { Id = 1, FirstName = "Test", LastName = "User", Username = "a.kalakonda.ctr", Password = "", Role= Role.Admin }
                //};

                httpContextAccessor.HttpContext.Session.Set<List<User>>(ConstantValues.UserDetails, _users);
                SessionValues.LoggedUserDetails = _users;
            }
        

    }

       

        public async Task<User> Authenticate(string username, string password)
        {
            var user = await Task.Run(() => _users.FirstOrDefault(x => x.Username.ToLower() == username.ToLower()));

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so return user details without password
            user.Password = null;
            return user;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            // return users without passwords
            return await Task.Run(() => _users.Select(x => {
                x.Password = null;
                return x;
            }));
        }
    }
}

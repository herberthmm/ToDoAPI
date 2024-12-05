using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using ToDoList.Database;
using ToDoList.Models;

namespace ToDoList.Services
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly DbHelper _db;

        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, 
                                          UrlEncoder encoder, ISystemClock clock, Context database) : base(options, logger, encoder, clock)
        {
            _db = new DbHelper(database);
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var endpoint = Context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
                return AuthenticateResult.NoResult();

            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Missing Authorization Header");

            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);

                //Recovering user and password from header
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(":");
                var user = credentials[0];
                var password = credentials[1];

                var userDb = _db.GetUserByUsername(user);

                if (userDb != null)
                {
                    //if (user == "Admin" && password == "Admin1234")
                    if (user == userDb.username && password == userDb.password)
                    {
                        var claims = new[] { new Claim(ClaimTypes.Name, user) };
                        var identity = new ClaimsIdentity(claims, Scheme.Name);
                        var principal = new ClaimsPrincipal(identity);
                        var ticket = new AuthenticationTicket(principal, Scheme.Name);

                        return AuthenticateResult.Success(ticket);
                    }
                    else
                    {
                        return AuthenticateResult.Fail("Invalid username or password. Please try again.");
                    }
                }
                else
                {
                    return AuthenticateResult.Fail("Invalid username or password. Please try again.");
                }
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail($"Authentication failed : {ex.Message}");
            }
        }
    }
}

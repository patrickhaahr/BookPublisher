using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Publisher.Presentation.Authorization;

public class JwtAdminAttribute : AuthorizeAttribute
{
    public JwtAdminAttribute()
    {
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
        Roles = "Admin";
    }
}

public class JwtAuthorizeAttribute : AuthorizeAttribute
{
    public JwtAuthorizeAttribute()
    {
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
    }
} 
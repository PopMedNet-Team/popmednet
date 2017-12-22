using Lpp.Auth;

namespace Lpp.Auth.Basic
{
    public interface IRoleBasedUser<TRolesEnum> : IUser
    {
        TRolesEnum Roles { get; set; }
        new string Login { get; set; }
        string Name { get; set; }
        string Email { get; set; }
    }
}
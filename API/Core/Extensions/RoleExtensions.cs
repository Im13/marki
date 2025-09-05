using API.Core.Constants;
using API.Core.Enums;

namespace Core.Extensions
{
    public static class RoleExtensions
    {
        public static string ToShortString(this UserRole role)
        {
            return role switch
            {
                UserRole.Admin => RoleConstants.ADMIN,
                UserRole.SuperAdmin => RoleConstants.SUPER_ADMIN,
                UserRole.Employee => RoleConstants.EMPLOYEE,
                UserRole.Customer => RoleConstants.CUSTOMER,
                _ => throw new ArgumentOutOfRangeException(nameof(role))
            };
        }

        public static UserRole? ToUserRole(this string roleString)
        {
            return roleString switch
            {
                RoleConstants.SUPER_ADMIN => UserRole.SuperAdmin,
                RoleConstants.ADMIN => UserRole.Admin,
                RoleConstants.EMPLOYEE => UserRole.Employee,
                RoleConstants.CUSTOMER => UserRole.Customer,
                _ => null
            };
        }

        public static bool CanReceiveOrderNotifications(this UserRole role)
        {
            return role == UserRole.SuperAdmin || role == UserRole.Admin;
        }

        public static bool CanManageOrders(this UserRole role)
        {
            return role == UserRole.SuperAdmin || role == UserRole.Admin;
        }
    }
}
namespace API.Core.Constants
{
    public static class RoleConstants
    {
        public const string SUPER_ADMIN = "SuperAdmin";
        public const string ADMIN = "Admin";
        public const string EMPLOYEE = "Employee";
        public const string CUSTOMER = "Customer";

        // Roles can receive order notifications
        public static readonly string[] ORDER_NOTIFICATION_ROLES = { SUPER_ADMIN, ADMIN };

        // Roles can manage orders
        public static readonly string[] ORDER_MANAGEMENT_ROLES = { SUPER_ADMIN, ADMIN };

        // All roles
        public static readonly string[] ALL_ROLES = { SUPER_ADMIN, ADMIN, EMPLOYEE, CUSTOMER };
    }
}
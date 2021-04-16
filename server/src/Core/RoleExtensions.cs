using Chad.Models.Common;

namespace Chad
{
    public static class RoleExtensions
    {
        /// <summary>
        ///     转换角色表达式为角色枚举类型
        /// </summary>
        /// <param name="roleExpression">角色表达式</param>
        /// <returns>对应的枚举类型</returns>
        public static UserRole AsUserRole(this string? roleExpression)
        {
            return roleExpression switch
            {
                nameof(UserRole.Administrator) => UserRole.Administrator,
                nameof(UserRole.Teacher) => UserRole.Teacher,
                _ => UserRole.Student
            };
        }

        /// <summary>
        ///     转换枚举类型到对应的角色文本
        /// </summary>
        /// <param name="role">枚举类型</param>
        /// <returns>对应橘色文本</returns>
        public static string AsUserRoleString(this UserRole role)
        {
            return role switch
            {
                UserRole.Administrator => nameof(UserRole.Administrator),
                UserRole.Teacher => nameof(UserRole.Teacher),
                _ => nameof(UserRole.Student)
            };
        }
    }
}
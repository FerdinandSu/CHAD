using Chad.Models.Common;

namespace ChadApi.Models
{
    public record Role
    {
        private Role(UserRole role)
        {
            RoleBase = role;
        }

        public static Role Administrator { get; } = new(UserRole.Administrator);
        public static Role Judgement { get; } = new(UserRole.Teacher);
        public static Role Base { get; } = new(UserRole.Student);

        /// <summary>
        ///     对应的角色值
        /// </summary>
        public UserRole RoleBase { get; }
    }
}
using System;
using Chad.Models.Common;
using Microsoft.AspNetCore.Identity;

namespace Chad.Data
{
    /// <summary>
    ///     用户
    /// </summary>
    public class DbUser : IdentityUser
    {
        /// <summary>
        ///     用户姓名
        /// </summary>
        public string FriendlyName { get; set; } = "";

        /// <summary>
        ///     用户角色
        /// </summary>
        public UserRole Role { get; set; }

        protected bool Equals(DbUser other)
        {
            return other.Id == Id;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((DbUser) obj);
        }

        public override int GetHashCode()
            // ReSharper disable once NonReadonlyMemberInGetHashCode
        {
            return HashCode.Combine(Id ?? "");
        }

        public static bool operator ==(DbUser? l, DbUser? r)
        {
            return Equals(l, r);
        }

        public static bool operator !=(DbUser? l, DbUser? r)
        {
            return !(l == r);
        }
    }
}
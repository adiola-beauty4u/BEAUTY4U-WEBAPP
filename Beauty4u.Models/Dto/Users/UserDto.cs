using Beauty4u.Interfaces.Dto.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty4u.Models.Dto.Users
{
    public class UserDto : IUserDto
    {
        public string StoreCode { get; set; } = string.Empty;
        public string UserCode { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string UserPassword { get; set; } = string.Empty;
        public string EmployeeId { get; set; } = string.Empty;
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string UserRole { get; set; } = string.Empty;
        public string LockCount { get; set; } = string.Empty;
        public bool Status { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiry { get; set; }
        public string Position { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
    }
}

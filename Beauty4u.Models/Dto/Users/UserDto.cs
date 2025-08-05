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
        public string StoreCode { get; set; } = null!;
        public string UserCode { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string UserPassword { get; set; } = null!;
        public string EmployeeId { get; set; } = null!;
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public string UserRole { get; set; } = null!;
        public string LockCount { get; set; } = null!;
        public bool Status { get; set; }
        public string RefreshToken { get; set; } = null!;
        public DateTime RefreshTokenExpiry { get; set; }
        public string Position { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
    }
}

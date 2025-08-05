namespace Beauty4u.Interfaces.Dto.Users
{
    public interface IUserDto
    {
        string EmployeeId { get; set; }
        string Firstname { get; set; }
        string Lastname { get; set; }
        string LockCount { get; set; }
        string Password { get; set; }
        string PasswordHash { get; set; }
        string RefreshToken { get; set; }
        DateTime RefreshTokenExpiry { get; set; }
        bool Status { get; set; }
        string StoreCode { get; set; }
        string UserCode { get; set; }
        string UserId { get; set; }
        string UserRole { get; set; }
        string Position { get; set; }
    }
}
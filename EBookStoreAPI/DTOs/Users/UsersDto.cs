namespace EBookStoreAPI.DTOs.Users
{
    public class UsersDto
    {
        public int? Id { get; set; }
        public string? Account { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public bool? Gender { get; set; }
        public string? Photo { get; set; }
        public DateTime? CreatedTime { get; set; }
        public bool? IsConfirmed { get; set; }
        public string? ConfirmCode { get; set; }
    }
}

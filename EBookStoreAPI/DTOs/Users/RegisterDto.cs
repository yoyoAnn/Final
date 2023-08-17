namespace EBookStoreAPI.DTOs.Users
{
    public class RegisterDto
    {
        public int Id { get; set; }

        public string? Account { get; set; } 

        public string? Password { get; set; }

        public string? ConfirmedPassword { get; set; }

        public string? Email { get; set; }

        public DateTime? CreatedTime { get; set; }
    }
}

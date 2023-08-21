namespace EBookStoreAPI.DTOs.Users
{
    public class ChangePasswordDto
    {
        public int? Id { get; set; }
        public string? OriginalPassword { get; set; } 
        public string? NewPassword { get; set; }
        public string? ConfirmedPassword { get; set; }


        public string? ConfirmCode { get; set; }
    }
}

namespace EBookStoreAPI.DTOs
{
    public class CSMailsDto
    {
        public int Id { get; set; }
        public string UserAccount { get; set; }
        public string Email { get; set; }
        public int ProblemTypeId { get; set; }
        public string ProblemStatement { get; set; }
        public long? OrderId { get; set; }
        public bool IsRead { get; set; }
        public bool IsReplied { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}

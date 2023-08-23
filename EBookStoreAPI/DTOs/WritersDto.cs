namespace EBookStoreAPI.DTOs
{
    public class WritersDto
    {
        public int Id { get; set; }
        public string Photo { get; set; }
        public string Name { get; set; }
        public string Profile { get; set; }
        public string Email { get; set; }
        public int ArticleId { get; set; }
        public string ArticleTitle { get; set; }
		public DateTime CreatedTime { get; set; }
	}
}

namespace EBookStoreAPI.DTOs
{
    public class ArticlesDto
    {

        public int Id { get; set; }
        public int WriterId { get; set; }
        public string  WriterName { get; set; }
        public string WriterPhoto { get; set; }
        public string WriterProfile { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int PageViews { get; set; }
        public bool Status { get; set; }
        public string Image { get; set; }
        public DateTime CreatedTime { get; set; }
        public int BookId { get; set; }
        public string BookName { get; set; }


    }
}

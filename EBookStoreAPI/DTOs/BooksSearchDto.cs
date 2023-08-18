namespace EBookStoreAPI.DTOs
{
    public class BooksSearchDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string CategoryName { get; set; }

        public string PublisherName { get; set; }

        public string ISBN { get; set; }
    }
}

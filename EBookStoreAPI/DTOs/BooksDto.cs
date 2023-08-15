namespace EBookStoreAPI.DTOs
{
    public class BooksDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Author { get; set; }
        public string CategoryName { get; set; }

        public string PublisherName { get; set; }

        public string BookImage { get; set; } = "default.jpg";

        public DateTime PublishDate { get; set; }

        public string PublisherDateTxt => PublishDate.ToString("yyyy/MM/dd");

        public string Summary { get; set; }

        public string ISBN { get; set; }

        public string EISBN { get; set; }

        public int Stock { get; set; }

        public bool? Status { get; set; } = true;
        public decimal Price { get; set; }

        public int? Discount { get; set; } = 1;
    }
}

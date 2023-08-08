using Dapper;
using EBookStoreAPI.DTOs;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EBookStoreAPI.Models.DapperRepository
{
    public class BookDapperRepository
    {
        private readonly IDbConnection _connection;
        private readonly EBookStoreContext _db;
        private readonly string connStr;
        private readonly IConfiguration _configuration;

        public BookDapperRepository(EBookStoreContext db,IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
            connStr = _configuration.GetConnectionString("EBookStoreContext");
            _connection = new SqlConnection(connStr);
        }


        /// <summary>
        /// 取得書籍
        /// </summary>
        /// <returns></returns>
        public async Task<List<BooksDto>> GetBookItems()
        {

            string sql = $@"SELECT BI.Image as BookImage, C.Name as CategoryName,B.ID as Id,B.Name as Name,P.Name as PublisherName ,
                         A.Name as Author,B.PublishDate as PublishDate,B.ISBN,B.EISBN,B.Price,B.Summary,
                         B.Stock,B.Status FROM Books as B
                         LEFT JOIN BookAuthors as BA ON BA.BookId = B.Id
                         LEFT JOIN Authors as A ON A.Id = BA.AuthorId
                         LEFT JOIN Publishers as P ON P.Id = B.PublisherId
                         LEFT JOIN Categories as C ON C.Id = B.CategoryId
						 LEFT JOIN BookImages as BI ON BI.BookId = B.Id";

            IEnumerable<BooksDto> bookitems = await _connection.QueryAsync<BooksDto>(sql);

            return bookitems.ToList();
        }
    }
}

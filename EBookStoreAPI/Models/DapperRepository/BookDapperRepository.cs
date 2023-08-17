using Dapper;
using EBookStoreAPI.DTOs;
using EBookStoreAPI.Models.EFModels;
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
            connStr = _configuration.GetConnectionString("EBookStore");
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

        /// <summary>
        /// 取得單一書本資訊
        /// </summary>
        /// <param name="bookId">書本的 ID</param>
        /// <returns>書本資訊</returns>
        public async Task<BooksDto> GetBookItemById(int bookId)
        {
            string sql = $@"SELECT BI.Image as BookImage, C.Name as CategoryName, B.ID as Id, B.Name as Name, P.Name as PublisherName,
                     A.Name as Author, B.PublishDate as PublishDate, B.ISBN, B.EISBN, B.Price, B.Summary,
                     B.Stock, B.Status FROM Books as B
                     LEFT JOIN BookAuthors as BA ON BA.BookId = B.Id
                     LEFT JOIN Authors as A ON A.Id = BA.AuthorId
                     LEFT JOIN Publishers as P ON P.Id = B.PublisherId
                     LEFT JOIN Categories as C ON C.Id = B.CategoryId
                     LEFT JOIN BookImages as BI ON BI.BookId = B.Id
                     WHERE B.ID = @bookId"; 

            BooksDto bookItem = await _connection.QueryFirstOrDefaultAsync<BooksDto>(sql, new { bookId });

            return bookItem;
        }

        /// <summary>
        /// 從分類名稱取得分類ID
        /// </summary>
        /// <param name="bookId">書本的 ID</param>
        /// <returns>書本資訊</returns>
        public int GetCategoryIdByCategoryName(string CategoryName)
        {
            string sql = $@"SELECT B.Id
                            FROM Books as B
                            LEFT JOIN Categories as C ON B.CategoryId = C.Id
                            WHERE C.Name = @CategoryName";

            int categoryId = _connection.QueryFirstOrDefault<int>(sql, new { CategoryName });

            return categoryId;
        }


        /// <summary>
        /// 從出版商名稱取得從出版商名稱取得分類IDID
        /// </summary>
        /// <param name="bookId">書本的 ID</param>
        /// <returns>書本資訊</returns>
        public int GetPublisherIdByPublisherName(string PublisherName)
        {
            string sql = $@"SELECT B.Id
                            FROM Books as B
                            LEFT JOIN Publishers as P ON B.PublisherId = P.Id
                            WHERE P.Name = @PublisherName";

            int PublisherId = _connection.QueryFirstOrDefault<int>(sql, new { PublisherName });

            return PublisherId;
        }
    }
}

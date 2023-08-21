using Dapper;
using EBookStoreAPI.Context;
using EBookStoreAPI.DTOs;
using EBookStoreAPI.Models.EFModels;
using Microsoft.Data.SqlClient;
using System.Text;

namespace EBookStoreAPI.Models.Infra.CartDapper
{
    public class CartGetDapperRepository
    {
        private readonly EbookStoreDepperContext _connStr;
        public CartGetDapperRepository(EbookStoreDepperContext context)
        {
            _connStr = context;
        }


        public IEnumerable<CartItemDapperVM> CartItemLoad()
        {
            DynamicParameters param = new DynamicParameters(); // Dapper 動態參數
            StringBuilder sql = new StringBuilder();


            sql.AppendLine(@"
                            select Carts.Id,userId,Books.Id as bookId, [name] ,[image],price,qty
                            from Carts
                            left join Books on [Carts].BookId=Books.Id
                            left join BookImages on [Carts].BookId=BookImages.BookId
                            where payment='0'
                              ");
            //if(Id != null)
            //{
            //    sql.AppendLine(@"where [Carts].Id=@Id");
            //    param.Add("Id", Id);
            //}
            using (var connection = _connStr.CreateConnection())
            {
                connection.Open();

                IEnumerable<CartItemDapperVM> DetailCarts = connection.Query<CartItemDapperVM>(sql.ToString());

                return DetailCarts;
            }
        }


        public class CartItemDapperVM
        {
            public int Id { get; set; }
            public int bookId { get; set; }
            public string image { get; set; }
            public int userId { get; set; }
            public string name { get; set; }
            public decimal price { get; set; }
            public int qty { get; set; }
        }



    }
}

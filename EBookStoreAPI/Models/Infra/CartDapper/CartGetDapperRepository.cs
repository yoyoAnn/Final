using Dapper;
using EBookStoreAPI.Context;
using EBookStoreAPI.Models.EFModels;
using Microsoft.Data.SqlClient;
using System.Text;

namespace EBookStoreAPI.Models.Infra.CartDapper
{
    public class CartGetDapperRepository
    {
        private readonly EbookStoreDepperContext _conStr;
        public CartGetDapperRepository(EbookStoreDepperContext context)
        {
            _conStr = context;
        }

        public IEnumerable<CartItemDapperVM> CartItemLoad(int Id)
        {
            DynamicParameters param = new DynamicParameters(); // Dapper 動態參數
            StringBuilder sql = new StringBuilder();


            sql.AppendLine(@"
                              select UserId, [name],Price,Qty
                              from Carts
                              left join Books on [Carts].BookId=Books.Id
                              where [Carts].Id=@Id");

            param.Add("Id", Id);

            using (var connection = _conStr.CreateConnection())
            {
                connection.Open();
                return connection.Query<CartItemDapperVM>(sql.ToString(), param);  // 使用 Query 方法執行查詢並取得結果
            }
            //using (var connection = _conStr.CreateConnection())
            //{
            //    // 執行更新後，您可以再次查詢相關資料並回傳
            //    string selectSql = "SELECT * FROM Orders WHERE Id = @Id";
            //    IEnumerable<CartItemDapperVM> cartItems = connection.Query<CartItemDapperVM>(selectSql, new { Id= Id });

            //    return cartItems.ToList();
            //}
        }

        public class CartItemDapperVM
        {
            public int UserId { get; set; }
            public string name { get; set; }
            public decimal Price { get; set; }
            public int Qty { get; set; }
        }



    }
}

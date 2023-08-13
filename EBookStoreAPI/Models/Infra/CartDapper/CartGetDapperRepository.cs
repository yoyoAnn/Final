using Dapper;
using EBookStoreAPI.Context;
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


        public  IEnumerable<CartItemDapperVM> CartItemLoad()
        {
            DynamicParameters param = new DynamicParameters(); // Dapper 動態參數
            StringBuilder sql = new StringBuilder();


            sql.AppendLine(@"
                            select UserId, [name],[Image],Price,Qty
                            from Carts
                            left join Books on [Carts].BookId=Books.Id
                            left join BookImages on [Carts].BookId=BookImages.BookId
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
            public string Image { get; set; }
            public int UserId { get; set; }
            public string name { get; set; }
            public decimal Price { get; set; }
            public int Qty { get; set; }
        }



    }
}

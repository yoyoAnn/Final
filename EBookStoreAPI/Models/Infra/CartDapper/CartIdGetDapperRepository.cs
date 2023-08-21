using Dapper;
using EBookStoreAPI.Context;
using EBookStoreAPI.DTOs;
using EBookStoreAPI.Models.EFModels;
using Microsoft.Data.SqlClient;
using System.Text;

namespace EBookStoreAPI.Models.Infra.CartDapper
{
    public class CartIdGetDapperRepository
    {
        private readonly EbookStoreDepperContext _connStr;
        public CartIdGetDapperRepository(EbookStoreDepperContext context)
        {
            _connStr = context;
        }


        public IEnumerable<CartItemDapperVM> CartItemLoad(CartsDto dto)
        {
            DynamicParameters param = new DynamicParameters(); // Dapper 動態參數
            StringBuilder sql = new StringBuilder();


            sql.AppendLine(@"
                            select Carts.Id,userId,Books.Id as bookId, [name] ,[image],price,qty
                            from Carts
                            left join Books on [Carts].BookId=Books.Id
                            left join BookImages on [Carts].BookId=BookImages.BookId
                            where 1=1
                            and payment='0'
                              ");

            if (!string.IsNullOrWhiteSpace(dto.UserId.ToString()))
            {
                sql.AppendLine(@"and userId=@userId");
                param.Add("userId", dto.UserId);
            }

            //if(Id != null)
            //{
            //    sql.AppendLine(@"where [Carts].Id=@Id");
            //    param.Add("Id", Id);
            //}
            using (var connection = _connStr.CreateConnection())
            {
                connection.Open();

                IEnumerable<CartItemDapperVM> DetailCarts = connection.Query<CartItemDapperVM>(sql.ToString(), param);

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

using Dapper;
using EBookStoreAPI.Context;
using EBookStoreAPI.DTOs;
using EBookStoreAPI.DTOs.Orders;
using EBookStoreAPI.Models.EFModels;
using EBookStoreAPI.VM.OrderVM;
using Microsoft.Data.SqlClient;
using System.Text;

namespace EBookStoreAPI.Models.Infra.CartDapper
{
    public class CartPostDapperRepository
    {
        private readonly EbookStoreDepperContext _connStr;
        public CartPostDapperRepository(EbookStoreDepperContext context)
        {
            _connStr = context;
        }


        public async Task CartItemPost(CartsDto dto)
        {
            DynamicParameters param = new DynamicParameters(); // Dapper 動態參數
            StringBuilder sql = new StringBuilder();


            sql.AppendLine(@"
  							insert into [dbo].[Carts](UserId,BookId,Qty,payment)
                            values
                            (@UserId,@BookId,@Qty,@payment)				
                              ");

            param.Add("UserId", dto.UserId);
            param.Add("BookId", dto.BookId);
            param.Add("Qty", dto.Qty);
            param.Add("payment", dto.payment);


            //if (!string.IsNullOrWhiteSpace(dto.Id.ToString()))
            //{
            //    sql.AppendLine(@"and Id=@Id");
            //    param.Add("Id", dto.Id);
            //}
            //if (!string.IsNullOrWhiteSpace(dto.BookId.ToString()))
            //{
            //    sql.AppendLine(@"and BookId=@BookId");
            //    param.Add("BookId", dto.BookId);
            //}
            //if (!string.IsNullOrWhiteSpace(dto.UserId.ToString()))
            //{
            //    sql.AppendLine(@"and UserId=@UserId");
            //    param.Add("UserId", dto.UserId);
            //}
            using (var connection = _connStr.CreateConnection())
            {
                connection.Open();
            
                connection.Execute(sql.ToString(), param);
             
            }
        }

        public async Task CartItemAdd1(CartsDto dto)
        {
            DynamicParameters param = new DynamicParameters(); // Dapper 動態參數
            StringBuilder sql = new StringBuilder();


            sql.AppendLine(@"
                              update Carts
                              set
                              Qty=Qty+1
                              where(1=1)
                              and UserId=@UserId
                              and BookId=@BookId	
                              and payment='0'
                              ");

            param.Add("UserId", dto.UserId);
            param.Add("BookId", dto.BookId);

            //if (!string.IsNullOrWhiteSpace(dto.Id.ToString()))
            //{
            //    sql.AppendLine(@"and Id=@Id");
            //    param.Add("Id", dto.Id);
            //}
            //if (!string.IsNullOrWhiteSpace(dto.BookId.ToString()))
            //{
            //    sql.AppendLine(@"and BookId=@BookId");
            //    param.Add("BookId", dto.BookId);
            //}
            //if (!string.IsNullOrWhiteSpace(dto.UserId.ToString()))
            //{
            //    sql.AppendLine(@"and UserId=@UserId");
            //    param.Add("UserId", dto.UserId);
            //}
            using (var connection = _connStr.CreateConnection())
            {
                connection.Open();

                connection.Execute(sql.ToString(), param);

            }
        }

        public async Task<bool> IfHasValue(CartsDto dto)
        {
            DynamicParameters param = new DynamicParameters();
            StringBuilder sql = new StringBuilder();

            sql.AppendLine(@"
                            SELECT  [Id]
                                  ,[UserId]
                                  ,[BookId]
                                  ,[Qty]
                                  ,[payment]
                              FROM [EBookStore].[dbo].[Carts]
                              where(1=1)
                              and BookId=@bookid
                              and UserId=@UserId
                              and payment='0'
                        ");

            param.Add("bookid", dto.BookId);
            param.Add("UserId", dto.UserId);

            using (var connection = _connStr.CreateConnection())
            {
                connection.Open();

                IEnumerable<CartsDto> DetailOrders = await connection.QueryAsync<CartsDto>(sql.ToString(), param);

                return DetailOrders.Any();
            }
        }

        public async Task<int> bookStock(CartsDto dto)
        {
            DynamicParameters param = new DynamicParameters(); // Dapper 動態參數
            StringBuilder sql = new StringBuilder();


            sql.AppendLine(@"
                          SELECT 
                              [Stock]
                          FROM [EBookStore].[dbo].[Books]
                          where(1=1)
                          and Id=@Id
                              ");


            param.Add("Id", dto.BookId);
            //if(Id != null)
            //{
            //    sql.AppendLine(@"where [Carts].Id=@Id");
            //    param.Add("Id", Id);
            //}
            using (var connection = _connStr.CreateConnection())
            {
                connection.Open();

                int DetailCarts = await connection.QueryFirstOrDefaultAsync<int>(sql.ToString(), param);

                return DetailCarts;
            }
        }

     
    }
}

using Dapper;
using EBookStoreAPI.Context;
using EBookStoreAPI.DTOs;
using EBookStoreAPI.Models.EFModels;
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
    }
}

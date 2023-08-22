using Dapper;
using EBookStoreAPI.Context;
using EBookStoreAPI.DTOs;
using EBookStoreAPI.Models.EFModels;
using Microsoft.Data.SqlClient;
using System.Text;

namespace EBookStoreAPI.Models.Infra.CartDapper
{
    public class OrderStatusEditDapperRepository
    {
        private readonly EbookStoreDepperContext _connStr;

        public OrderStatusEditDapperRepository(EbookStoreDepperContext context)
        {
            _connStr = context;
        }


        public async Task PayInfoEdit(EcpayReturnDto dto)
        {
            DynamicParameters param = new DynamicParameters(); // Dapper 動態參數
            StringBuilder sql = new StringBuilder();


            sql.AppendLine(@"                  
  						          update Orders
                                  set OrderStatusId='6'
                                  where(1=1)
                                  and Id=@Id  ");

            param.Add("Id", dto.MerchantTradeNo);

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

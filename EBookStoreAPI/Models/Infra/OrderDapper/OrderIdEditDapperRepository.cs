using Dapper;
using EBookStoreAPI.Context;
using EBookStoreAPI.DTOs;
using EBookStoreAPI.DTOs.Orders;
using EBookStoreAPI.Models.EFModels;
using Microsoft.Data.SqlClient;
using System.Text;

namespace EBookStoreAPI.Models.Infra.CartDapper
{
    public class OrderIdEditDapperRepository
    {
        private readonly EbookStoreDepperContext _connStr;
        public OrderIdEditDapperRepository(EbookStoreDepperContext context)
        {
            _connStr = context;
        }


        public async Task OrderIdEditPost(oldNewOrderId dto)
        {

            DynamicParameters param = new DynamicParameters(); // Dapper 動態參數
            StringBuilder sql = new StringBuilder();


            sql.AppendLine(@"
								  update Orders
								  set
								  Id=@newOrderId
                                  ,OrderTime=GETDATE()
								  where
								  Id=@oldOrderId		
                              ");

            param.Add("newOrderId", dto.newOrderid);
            param.Add("oldOrderId", dto.oldOrderid);


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
            try
            {
                using (var connection = _connStr.CreateConnection())
                {
                    connection.Open();

                    connection.Execute(sql.ToString(), param);

                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }
    }
}

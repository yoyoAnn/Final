using Dapper;
using EBookStoreAPI.Context;
using EBookStoreAPI.DTOs;
using EBookStoreAPI.Models.EFModels;
using Microsoft.Data.SqlClient;
using System.Text;

namespace EBookStoreAPI.Models.Infra.CartDapper
{
    public class OrderPostDapperRepository
    {
        private readonly EbookStoreDepperContext _connStr;

        public OrderPostDapperRepository(EbookStoreDepperContext context)
        {
            _connStr = context;
        }


        public async Task PayInfoPost(OrdersDto dto)
        {
            DynamicParameters param = new DynamicParameters(); // Dapper 動態參數
            StringBuilder sql = new StringBuilder();


            sql.AppendLine(@"                  
  							insert into [dbo].[Orders](Id,UserId,ReceiverName,ReceiverAddress,ReceiverPhone,VehicleNum,Remark,OrderTime,OrderStatusId,TotalAmount,TotalPayment,OrderStatusId)
                            values
                                                    (@Id,@UserId,@ReceiverName,@ReceiverAddress,@ReceiverPhone,@VehicleNum,@Remark,@OrderTime,@OrderStatusId,@TotalAmount,@TotalPayment,'1')				
                              ");

            param.Add("Id", dto.Id);
            param.Add("UserId", dto.UserId);
            param.Add("ReceiverName", dto.ReceiverName);
            param.Add("ReceiverAddress", dto.ReceiverAddress);
            param.Add("ReceiverPhone", dto.ReceiverPhone);
            param.Add("VehicleNum", dto.VehicleNum);
            param.Add("Remark", dto.Remark);
            param.Add("OrderTime", dto.OrderTime);
            param.Add("OrderStatusId", dto.OrderStatusId);
            param.Add("TotalAmount", dto.TotalAmount);
            param.Add("TotalPayment", dto.TotalPayment);


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

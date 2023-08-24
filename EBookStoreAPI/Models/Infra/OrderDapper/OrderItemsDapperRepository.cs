using Dapper;
using EBookStoreAPI.Context;
using EBookStoreAPI.DTOs;
using EBookStoreAPI.DTOs.Orders;
using EBookStoreAPI.Models.EFModels;
using EBookStoreAPI.VM.OrderVM;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.Data.SqlClient;
using System.Text;

namespace EBookStoreAPI.Models.Infra.CartDapper
{
    public class OrderItemsDapperRepository
    {
        private readonly EbookStoreDepperContext _connStr;
        public OrderItemsDapperRepository(EbookStoreDepperContext context)
        {
            _connStr = context;
        }


        public IEnumerable<OrderItemsVM> GetOrderItemLoad(GetOrderItem dto)
        {
            DynamicParameters param = new DynamicParameters(); // Dapper 動態參數
            StringBuilder sql = new StringBuilder();


            sql.AppendLine(@"
                                SELECT    [OrderItems].[Id]
										  ,[Image]
                                          ,[OrderId]
                                          ,Books.[Name]
                                          ,[OrderItems].[Price]
                                          ,[Qty]
                                      FROM [EBookStore].[dbo].[OrderItems]
                                    LEFT JOIN Books on Books.Id=[OrderItems].BookId
									LEFT JOIN BookImages on BookImages.BookId=Books.Id
                                  where(1=1)                                   

                              ");

            if (!string.IsNullOrWhiteSpace(dto.orderId))
            {
                sql.AppendLine(@"and OrderId=@userId");
                param.Add("userId", dto.orderId);
            }

            sql.AppendLine(@" order by [OrderItems].[Price] desc");

            //if(Id != null)
            //{
            //    sql.AppendLine(@"where [Carts].Id=@Id");
            //    param.Add("Id", Id);
            //}
            try
            {
                using (var connection = _connStr.CreateConnection())
                {
                    connection.Open();

                    IEnumerable<OrderItemsVM> DetailOrders = connection.Query<OrderItemsVM>(sql.ToString(), param);

                    return DetailOrders;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


    }
}

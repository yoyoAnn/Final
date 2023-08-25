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
    public class OrderFinalDapperRepository
    {
        private readonly EbookStoreDepperContext _connStr;
        public OrderFinalDapperRepository(EbookStoreDepperContext context)
        {
            _connStr = context;
        }


        public IEnumerable<OrdersVM> OrderFinalLoad(GetOrdersListId dto)
        {
            DynamicParameters param = new DynamicParameters(); // Dapper 動態參數
            StringBuilder sql = new StringBuilder();


            sql.AppendLine(@"
                                  SELECT [Orders].[Id]
	                                  ,UserId
                                      ,Users.[Name]
                                      ,[ReceiverName]
                                      ,[ReceiverAddress]
                                      ,[ReceiverPhone]
                                      ,[TaxIdNum]
                                      ,[VehicleNum]
                                      ,[Remark]
                                      ,[OrderTime]
                                      ,OrderStatuses.[Name] as OrderStatusName
                                      ,[TotalAmount]
                                      ,[ShippingNumber]
                                      ,[ShippingTime]
                                      ,[ShippingFee]
                                      ,[ShippingStatuses].[Name] as ShippingStatusName
                                      ,[TotalPayment]
                                  FROM [EBookStore].[dbo].[Orders]
                                  LEFT JOIN Users on Orders.UserId = Users.Id 
                                  LEFT JOIN OrderStatuses on Orders.OrderStatusId=OrderStatuses.Id
                                  LEFT JOIN [ShippingStatuses] on Orders.ShippingStatusId=[ShippingStatuses].Id
                                  where(1=1) 
                                  and (OrderStatuses.[Name]='已完成' or OrderStatuses.[Name]='已退款')
								  and ([ShippingStatuses].[Name]='已取貨' or [ShippingStatuses].[Name]='已退貨')
                              ");

            if (!string.IsNullOrWhiteSpace(dto.id.ToString()))
            {
                sql.AppendLine(@"and userId=@userId");
                param.Add("userId", dto.id);
            }

            sql.AppendLine(@" order by ShippingTime desc");           

            //if(Id != null)
            //{
            //    sql.AppendLine(@"where [Carts].Id=@Id");
            //    param.Add("Id", Id);
            //}
            using (var connection = _connStr.CreateConnection())
            {
                connection.Open();

                IEnumerable<OrdersVM> DetailOrders = connection.Query<OrdersVM>(sql.ToString(), param);

                return DetailOrders;
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

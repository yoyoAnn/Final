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
    public class OrderReturnDapperRepository
    {
        private readonly EbookStoreDepperContext _connStr;
        public OrderReturnDapperRepository(EbookStoreDepperContext context)
        {
            _connStr = context;
        }


        public void OrderReturnEdit(OrderReturnDto dto)
        {
            DynamicParameters param = new DynamicParameters(); // Dapper 動態參數
            StringBuilder sql = new StringBuilder();


            sql.AppendLine(@"
                                update [Orders]
                                set
                                ShippingStatusId='7'
                                ,OrderStatusId='4'
                                where(1=1)
                              ");

                sql.AppendLine(@"and Id=@id");
                param.Add("id", dto.id);

            //if(Id != null)
            //{
            //    sql.AppendLine(@"where [Carts].Id=@Id");
            //    param.Add("Id", Id);
            ////}
            //using (var connection = _connStr.CreateConnection())
            //{
            //    connection.Open();

            //    IEnumerable<OrdersVM> DetailOrders = connection.Query<OrdersVM>(sql.ToString(), param);

            //    return DetailOrders;
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

        public void CheckOrderToFinal(OrderReturnDto dto)
        {
            DynamicParameters param = new DynamicParameters(); // Dapper 動態參數
            StringBuilder sql = new StringBuilder();


            sql.AppendLine(@"
                                update [Orders]
                                set
                                OrderStatusId='2'
                                where(1=1)
                              ");

            sql.AppendLine(@"and Id=@id");
            param.Add("id", dto.id);

            //if(Id != null)
            //{
            //    sql.AppendLine(@"where [Carts].Id=@Id");
            //    param.Add("Id", Id);
            ////}
            //using (var connection = _connStr.CreateConnection())
            //{
            //    connection.Open();

            //    IEnumerable<OrdersVM> DetailOrders = connection.Query<OrdersVM>(sql.ToString(), param);

            //    return DetailOrders;
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

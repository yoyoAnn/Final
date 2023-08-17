using Dapper;
using EBookStoreAPI.Context;
using EBookStoreAPI.DTOs;
using EBookStoreAPI.Models.EFModels;
using Microsoft.Data.SqlClient;
using System.Data;
using static EBookStoreAPI.Models.Infra.CartDapper.CartGetDapperRepository;
using System.Text;

namespace EBookStoreAPI.Models.DapperRepository
{
    public class CSMailsDapperRepository
    {
        private readonly EbookStoreDepperContext _context;
        public CSMailsDapperRepository(EbookStoreDepperContext context)
        {
			_context = context;
        }
		public void AddCSMail(CSMailsDto dto)
		{
			DynamicParameters param = new DynamicParameters(); // Dapper 動態參數
			StringBuilder sql = new StringBuilder();


			sql.AppendLine(@"
INSERT INTO CustomerServiceMails
(UserAccount,Email,ProblemTypeId,ProblemStatement,OrderId,IsRead,IsReplied,CreatedTime)
VALUES
(@account,@email,@problemTypeId,@problemStatement,@orderId,1,1,GETDATE())
                              ");
			
			param.Add("account", dto.UserAccount);
			param.Add("email", dto.Email);
			param.Add("problemTypeId", dto.ProblemTypeId);
			param.Add("problemStatement", dto.ProblemStatement);
			param.Add("orderId", dto.OrderId);


			using (var connection = _context.CreateConnection())
			{
				connection.Open();
				connection.Execute(sql.ToString(), param);
			}
		}
    }
}

using Dapper;
using EBookStoreAPI.DTOs;
using EBookStoreAPI.Models.EFModels;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EBookStoreAPI.Models.DapperRepository
{
    public class CSMailsDapperRepository
    {
        private readonly IDbConnection _connection;
        private readonly EBookStoreContext _context;
        private readonly string _connStr;
        private readonly IConfiguration _configuration;
        public CSMailsDapperRepository(EBookStoreContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            //_connStr = System.Configuration.ConfigurationManager.ConnectionStrings["EBookStoreContext"].ConnectionString;
            _connStr = _configuration.GetConnectionString("EBookStore");
            //_connection = new SqlConnection(_connStr);



        }
        public async Task<string> AddCSMail(CSMailsDto csmailDto)
        {
            string sql = $@"
INSERT INTO CustomerServiceMails
(UserAccount,Email,ProblemTypeId,ProblemStatement,OrderId,IsRead,IsReplied,CreatedTime)
VALUES
(@account,@email,@problemTypeId,@problemStatement,@orderId,1,1,GETDATE())
";


            string sql2 = $@"
UPDATE RepliedMails
SET Email = @email,
Title = @title,
Content = @content,
CreatedTime = GETDATE()
WHERE Id = @id";

            var csMail = new SqlConnection(_connStr)
                .Query<CSMailsDto>(sql, new
                {
                    account = csmailDto.UserAccount,
                    email = csmailDto.Email,
                    problemTypeId = csmailDto.ProblemTypeId,
                    problemStatement = csmailDto.ProblemStatement,
                    orderId = csmailDto.OrderId
                });

            if (csMail != null)
            {
                return "Success";
            }
            return "Fail";
        }
    }
}

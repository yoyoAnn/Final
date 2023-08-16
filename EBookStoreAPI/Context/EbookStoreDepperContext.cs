using Microsoft.Data.SqlClient;
using System.Data;

namespace EBookStoreAPI.Context
{
    public class EbookStoreDepperContext
    {

        private readonly IConfiguration _configuration;

        public EbookStoreDepperContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection CreateConnection() => new SqlConnection(_configuration.GetConnectionString("EBookStore"));
    }
}

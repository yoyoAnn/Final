using Dapper;
using EBookStoreAPI.Context;
using EBookStoreAPI.DTOs;
using EBookStoreAPI.Models.EFModels;
using System.Text;

namespace EBookStoreAPI.Models.DapperRepository
{
    public class ArticleDapperRepository
    {
        private readonly EbookStoreDepperContext _context;
        public ArticleDapperRepository(EbookStoreDepperContext context)
        {
            _context = context;
        }
        public IEnumerable<ArticlesDto> GetArticle(int id)
        {
            DynamicParameters param = new DynamicParameters(); // Dapper 動態參數
            StringBuilder sql = new StringBuilder();


            sql.AppendLine(@"
SELECT [Articles].[Id], [Articles].[WriterId], [Writers].[Name] AS WriterName, [Writers].[Photo] AS WriterPhoto,
[Writers].[Profile] AS WriterProfile, [Articles].[Title], [Articles].[Content], [Articles].[PageViews],
[Articles].[Status], [Articles].[Image], [Articles].[CreatedTime],
[Articles].[BookId], [Books].[Name] AS BookName
FROM [Articles]
LEFT JOIN [Writers] ON [Articles].[WriterId] = [Writers].[Id]
LEFT JOIN [Books] ON [Articles].[BookId] = [Books].[Id]
LEFT JOIN [BookImages] ON [Articles].[BookId] = [BookImages].[BookId]
WHERE [Articles].[Id] = @Id
");

            param.Add("Id", id);

            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                IEnumerable<ArticlesDto> article = connection.Query<ArticlesDto>(sql.ToString(), param);
            return article;
            }

        }
    }    
}

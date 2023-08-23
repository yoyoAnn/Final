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
        public IEnumerable<ArticlesDto> GetArticles(int? writerId, string? searchText)
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
");
            if (writerId != null)
            {
                sql.AppendLine(@"WHERE [Articles].[WriterId]=@WriterId");
                param.Add("WriterId", writerId);
            }
            
            if(writerId == null && searchText != null)
            {
                string text = "%" + searchText + "%";
                sql.AppendLine(@"WHERE ([Articles].[Title] LIKE @SearchText or [Books].[Name] LIKE @SearchText or [Writers].[Name] LIKE @SearchText)");
                param.Add("SearchText", text);
            }

            if (writerId != null && searchText != null)
            {
                string text = "%" + searchText + "%";
                sql.AppendLine(@" AND ([Articles].[Title] LIKE @SearchText or [Books].[Name] LIKE @SearchText or [Writers].[Name] LIKE @SearchText)");
                param.Add("SearchText", text);
            }

            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                IEnumerable<ArticlesDto> article = connection.Query<ArticlesDto>(sql.ToString(), param);
                return article; 
            }

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
        public IEnumerable<WritersDto> GetWriter(int id)
        {
            DynamicParameters param = new DynamicParameters(); // Dapper 動態參數
            StringBuilder sql = new StringBuilder();


            sql.AppendLine(@"
SELECT TOP (3) [Writers].[Id], [Writers].[Name], [Writers].[Photo], [Writers].[Profile],
[Articles].[Id] AS ArticleId, [Articles].[Title] AS ArticleTitle, [Articles].[CreatedTime]
FROM [Writers]
RIGHT JOIN [Articles] ON [Articles].[WriterId] = [Writers].[Id]
WHERE [Writers].[Id] = @Id
ORDER BY [CreatedTime] DESC
");

            param.Add("Id", id);

            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                IEnumerable<WritersDto> writer = connection.Query<WritersDto>(sql.ToString(), param);
                return writer;
            }

        }
    }    
}

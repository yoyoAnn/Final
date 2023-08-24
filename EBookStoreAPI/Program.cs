using EBookStoreAPI.Models;
using EBookStoreAPI.Context;
using EBookStoreAPI.Models.EFModels;
using EBookStoreAPI.Models.Infra.CartDapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Authorization;
using EBookStoreAPI.Models.DapperRepository;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the DI container.
string EBookStoreConnectionString = builder.Configuration.GetConnectionString("EBookStore");
builder.Services.AddDbContext<EBookStoreContext>(options =>
{
    options.UseSqlServer(EBookStoreConnectionString);
});


builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
{
    //���n�J�ɷ|�۰ʾɨ�o�Ӻ�}
    //option.LoginPath = new PathString("/api/Login/NoLogin");
});

//����M��
//builder.Services.AddMvc(options =>
//{
//    options.Filters.Add(new AuthorizeFilter());
//});

//builder.Services.AddCors(options => {
//	options.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
//});

//CORS全部開放用上面那段，部分開放用下面這段，用註解切換

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.WithOrigins("https://127.0.0.1:8081", "https://127.0.0.1:8080", "https://payment-stage.ecpay.com.tw", "https://127.0.0.1:8080/orders", "https://127.0.0.1:8081/orders")
               .AllowAnyMethod()
               .AllowAnyHeader()
                .AllowCredentials();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<CSMailsDapperRepository>();
builder.Services.AddScoped<CartIdGetDapperRepository>();
builder.Services.AddScoped<CartPostDapperRepository>();
builder.Services.AddScoped<CartGetDapperRepository>();
builder.Services.AddScoped<CartPutDapperRepository>();
builder.Services.AddScoped<ArticleDapperRepository>();
builder.Services.AddScoped<OrderPostDapperRepository>();
builder.Services.AddScoped<PaymentCartDapperRepository>();
builder.Services.AddScoped<OrderStatusEditDapperRepository>();
builder.Services.AddScoped<OrderNotPayDapperRepository>();
builder.Services.AddScoped<OrderItemPostDapperRepository>();
builder.Services.AddScoped<OrderItemsDapperRepository>();
builder.Services.AddScoped<OrderIdEditDapperRepository>();
builder.Services.AddScoped<OrderPayDapperRepository>();
builder.Services.AddScoped<CancelOrderIdDapperRepository>();




builder.Services.AddSingleton<EbookStoreDepperContext>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();

app.UseCors("AllowAll");


app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

//app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.Run();

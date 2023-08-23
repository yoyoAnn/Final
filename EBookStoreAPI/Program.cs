using EBookStoreAPI.Models;
using EBookStoreAPI.Context;
using EBookStoreAPI.Models.EFModels;
using EBookStoreAPI.Models.Infra.CartDapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Authorization;
using EBookStoreAPI.Models.DapperRepository;

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

builder.Services.AddCors(options => {
	options.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
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


builder.Services.AddSingleton<EbookStoreDepperContext>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();

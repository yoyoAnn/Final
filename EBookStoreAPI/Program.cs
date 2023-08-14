using EBookStoreAPI.Models;
using EBookStoreAPI.Context;
using EBookStoreAPI.Models.EFModels;
using EBookStoreAPI.Models.Infra.CartDapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Authorization;

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
	//未登入時會自動導到這個網址
	option.LoginPath = new PathString("/api/Login/NoLogin");
});

//全域套用
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

builder.Services.AddScoped<CartGetDapperRepository>();
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

global using SeldonStockScannerAPI.Data;
using Microsoft.EntityFrameworkCore;
//using SeldonStockScannerAPI.Config;
using StockScannerCommonCode;
using StockScannerCommonCode.finviz_classes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddCaching(builder.Configuration);

builder.Services.AddControllers();

// DATA
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IFinvizFilter, FinvizFilter>();
builder.Services.AddScoped<IWebScraper, WebScraper>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed((host) => true)
            .AllowAnyHeader());
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("CorsPolicy");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

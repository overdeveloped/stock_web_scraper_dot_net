global using SeldonStockScannerAPI.Data;
using Microsoft.EntityFrameworkCore;
using SeldonStockScannerAPI.FinvizScan;
using SeldonStockScannerAPI.FinvizUrlTranslator;
using SeldonStockScannerAPI.WatchList;
using SeldonStockScannerAPI.WebScraper;
using System;
//using SeldonStockScannerAPI.Config;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add services to the container.

//builder.Services.AddCaching(builder.Configuration);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Services, singleton injection
builder.Services.AddScoped<IFinvizService, FinvizService>();
builder.Services.AddScoped<IWebScraper, WebScraper>();
builder.Services.AddScoped<IFinvizUrlTranslator, FinvizUrlTranslator>();

builder.Services.AddScoped<IWatchListService, WatchListService>();


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

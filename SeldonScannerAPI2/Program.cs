global using SeldonStockScannerAPI.Data;
using Microsoft.EntityFrameworkCore;
using SeldonStockScannerAPI.FinvizScan;
using SeldonStockScannerAPI.FinvizUrlTranslator;
using SeldonStockScannerAPI.WatchList;
using SeldonStockScannerAPI.WebScraper;
using System;
//using SeldonStockScannerAPI.Config;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddCaching(builder.Configuration);

builder.Services.AddDbContext<ApplicationDbContext>();

// Services, singleton injection
builder.Services.AddSingleton<IFinvizService, FinvizService>();
builder.Services.AddSingleton<IWebScraper, WebScraper>();
builder.Services.AddSingleton<IFinvizUrlTranslator, FinvizUrlTranslator>();

builder.Services.AddScoped<IWatchListService, WatchListService>();

builder.Services.AddControllers();

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

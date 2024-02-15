global using SeldonStockScannerAPI.Data;
using Microsoft.EntityFrameworkCore;
using SeldonStockScannerAPI.FinvizScan;
using SeldonStockScannerAPI.FinvizUrlTranslator;
using SeldonStockScannerAPI.WebScraper;
//using SeldonStockScannerAPI.Config;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddCaching(builder.Configuration);

builder.Services.AddControllers();

// DATA
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Services, singleton injection
builder.Services.AddSingleton<IFinvizService, FinvizService>();
builder.Services.AddSingleton<IWebScraper, WebScraper>();
builder.Services.AddSingleton<IFinvizUrlTranslator, FinvizUrlTranslator>();

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

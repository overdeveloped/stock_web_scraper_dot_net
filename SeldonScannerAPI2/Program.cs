using Microsoft.EntityFrameworkCore;
using SeldonStockScannerAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// DATA
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS explanation:
https://learn.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-7.0

var MyAllowSpecificOrigins = "corsPolicy";

builder.Services.AddCors(options =>
{
    //options.AddPolicy("CorsPolicy",
    //    builder => builder
    //        .AllowAnyMethod()
    //        .AllowAnyHeader()
    //        //.SetIsOriginAllowed((host) => true)
    //        .WithOrigins("http://localhost:4200") // Allow only this origin can also have multiple origins separated with comma
    //        .AllowCredentials());

    options.AddPolicy(name: MyAllowSpecificOrigins,
                        policy =>
                        {
                            //policy.WithOrigins("http://localhost:4200")
                            policy.AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader();
                        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();



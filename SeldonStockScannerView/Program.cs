using SeldonStockScannerView.Clients;
using SeldonStockScannerView.Services;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHttpClient<IWatchListClient, WatchListClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7059/api/");
});

builder.Services.AddHttpClient<IFinvizScanClient, FinvizScanClient> (client =>
{
    client.BaseAddress = new Uri("https://localhost:7059/api/Finviz/");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();

using VideoQRCode.Web.Services;

var builder = WebApplication.CreateBuilder(args);

var apiUrl = builder.Configuration["ApiConfig:UrlApi"];

builder.Services.AddHttpClient<IVideoService, VideoService>(client =>
{
    client.BaseAddress = new Uri(apiUrl!);
});


builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

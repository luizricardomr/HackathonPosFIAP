using FFMpegCore.Pipes;
using Microsoft.OpenApi.Models;
using VideoQRCode.DAO.Configuration;
using VideoQRCode.DAO.Infra.Repository;
using VideoQRCode.DAO.Services;
using VideoQRCode.DAO.Utils;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddMongo(builder.Configuration);
RabbitConfiguration.Configure(builder);

builder.Services.AddScoped<IVideoRepository, VideoRepository>();
builder.Services.AddScoped<IConteudoVideoRepository, ConteudoVideoRepository>();
builder.Services.AddScoped<IVideoService, VideoService>();
builder.Services.AddScoped<IConteudoVideoService, ConteudoVideoService>();
builder.Services.AddScoped<IFrameExtractor, FrameExtractor>();
builder.Services.AddScoped<IFrameProcessor, FrameProcessor>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Video QRCODE API",
        Version = "v1",
        Description = "HACKATHON realizado pelo grupo 13",
        License = new OpenApiLicense
        {
            Name = "MIT",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

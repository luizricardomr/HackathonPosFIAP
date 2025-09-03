using VideoQRCode.DAO.Configuration;
using VideoQRCode.DAO.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Configura��o dos servi�os e CORS
builder.AddApiConfig();

// SignalR
builder.Services.AddSignalR();

// Outros servi�os (Mongo, Rabbit, etc)
builder.Services.AddMongo(builder.Configuration);
RabbitConfiguration.Configure(builder);

var app = builder.Build();

// Ordem correta do middleware
app.UseRouting();

app.UseCors("AllowFrontend");

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<VideoHub>("/videoHub");
});

app.MapControllers();


app.UseSwagger();
app.UseSwaggerUI();



app.Run();

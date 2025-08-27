using VideoQRCode.API.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiConfig();
builder.Services.AddMongo(builder.Configuration);

RabbitConfiguration.Configure(builder);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minha API V1");
    c.RoutePrefix = "swagger"; // deixa o caminho em /swagger
});

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
// Torna o Program público para testes
public partial class Program { }
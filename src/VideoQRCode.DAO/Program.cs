using VideoQRCode.DAO.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiConfig();

builder.Services.AddMongo(builder.Configuration);
RabbitConfiguration.Configure(builder);

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

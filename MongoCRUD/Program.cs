using System.Text.Json.Serialization;
using MongoCRUD;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(c => c.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region MongoDB·þÎñ×¢Èë

var dbStr = builder.Configuration.GetConnectionString("Mongo");
builder.Services.AddMongoDbContext<DbContext>(dbStr!);

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    _ = app.UseSwagger().UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

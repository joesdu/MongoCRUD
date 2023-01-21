using MongoCRUD;
using MongoCRUD.Extensions;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(c => c.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region MongoDB服务注入

var dbStr = builder.Configuration.GetConnectionString("Mongo");
builder.Services.AddMongoDbContext<DbContext>(dbStr!);

#endregion

#region GridFS服务注入

// 由于我们BaseDbContext中已经注入了IMongoClient所以这里不需要显示传入db参数.否则需要显示传入.
builder.Services.AddHoyoGridFS();

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

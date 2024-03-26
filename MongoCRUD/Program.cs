using EasilyNET.Mongo.ConsoleDebug;
using EasilyNET.MongoSerializer.AspNetCore;
using MongoCRUD;
using MongoDB.Driver.Linq;
using Serilog;
using Serilog.Events;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// 添加Serilog配置
builder.Host.UseSerilog((hbc, lc) =>
{
    const LogEventLevel logLevel = LogEventLevel.Information;
    lc.ReadFrom.Configuration(hbc.Configuration)
      .MinimumLevel.Override("Microsoft", logLevel)
      .MinimumLevel.Override("System", logLevel)
      .Enrich.FromLogContext()
      .WriteTo.Async(wt => wt.SpectreConsole());
});
// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(c => c.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// 注册MongoDB的一些东西
builder.Services.AddMongoContext<DbContext>(builder.Configuration, op =>
{
    op.ClientSettings = cs =>
    {
        cs.ClusterConfigurator = s => s.Subscribe(new ActivityEventSubscriber());
        cs.LinqProvider = LinqProvider.V3;
    };
    op.DefaultConventionRegistry = true;
});
// 注册自定义的MongoDB类型序列化
builder.Services.RegisterSerializer(new DateOnlySerializerAsString());
builder.Services.RegisterSerializer(new TimeOnlySerializerAsString());
// 注册GridFS
builder.Services.AddMongoGridFS();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger().UseSwaggerUI();
}
app.UseAuthorization();
app.MapControllers();
app.Run();
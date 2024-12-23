using GameStore.Api.Configs;
using GameStore.Api.Data;
using GameStore.Api.Dtos;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;
// Add services to the container.

services.AddControllers();
services.AddOptions<PostgresConfig>()
    .Bind(config.GetSection(PostgresConfig.SectionName));

var postgresConfig = config.GetRequiredSection(PostgresConfig.SectionName).Get<PostgresConfig>();

services.AddDbContextPool<GameStoreContext>(options =>
    options.UseNpgsql(postgresConfig!.ConnectionString));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MigrateDb();

app.Run();


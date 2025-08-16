using Api.Dto;
using Api.Interfaces;
using Api.Services;
using Microsoft.Extensions.Caching.Hybrid;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<ITestService, TestService>();


builder.Services.AddHybridCache();
builder.Services.AddStackExchangeRedisCache(opt =>
{
    opt.Configuration = builder.Configuration.GetConnectionString("Redis");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/api/data/{id}", async (
    Guid id,
    ITestService testService,
    HybridCache cache
    ) =>
{
    return await cache.GetOrCreateAsync($"data:{id}", async _ => await testService.GetDataAsync(id), null, null);
});

app.MapGet("/api/data", async (
    ITestService testService, HybridCache cache) =>
{
    return await cache.GetOrCreateAsync("data:all", async _ => await testService.GetAllDataAsync(), null, null);
});

app.MapPost("/api/data", async (
    DataDto data,
    ITestService testService, HybridCache cache) =>
{
    await cache.RemoveAsync($"data:all");
    return Results.Ok(await testService.CreateDataAsync(data));
});

app.MapPut("/api/data/{id}", async (
    Guid id,
    DataDto data,
    ITestService testService, HybridCache cache) =>
{
    await cache.RemoveAsync($"data:{id}");
    await cache.RemoveAsync($"data:all");
    return Results.Ok(await testService.UpdateDataAsync(id, data));
});

app.MapDelete("/api/data/{id}", async (
    Guid id,
    ITestService testService, HybridCache cache) =>
{
    var response = await testService.DeleteDataAsync(id);

    if (response != null)
    {
        await cache.RemoveAsync($"data:all");
        await cache.RemoveAsync($"data:{id}");
    }

    return Results.Ok(response);
});


app.Run();
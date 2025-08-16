using Api.Dto;
using Api.Interfaces;
using Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<ITestService, TestService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/api/data/{id}", async (Guid id, ITestService testService) =>
{
    return Results.Ok(await testService.GetDataAsync(id));
});

app.MapGet("/api/data", async (ITestService testService) =>
{
    return Results.Ok(await testService.GetAllDataAsync());
});

app.MapPost("/api/data", async (DataDto data, ITestService testService) =>
{
    return Results.Ok(await testService.CreateDataAsync(data));
});

app.MapPut("/api/data/{id}", async (Guid id, DataDto data, ITestService testService) =>
{
    return Results.Ok(await testService.UpdateDataAsync(id, data));
});

app.MapDelete("/api/data/{id}", async (Guid id, ITestService testService) =>
{
    return Results.Ok(await testService.DeleteDataAsync(id));
});


app.Run();
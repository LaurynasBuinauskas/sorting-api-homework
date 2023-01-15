using Sorting.Api.Homework.WebApi.InputOutput.Readers;
using Sorting.Api.Homework.WebApi.InputOutput.Writers;
using Sorting.Api.Homework.WebApi.Services;
using System.IO.Abstractions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddTransient<IFileSystem, FileSystem>();
builder.Services.AddTransient<IFileWriter, FileWriter>();
builder.Services.AddTransient<IFileReader, FileReader>();
builder.Services.AddTransient<ISortService, SortService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.Run();

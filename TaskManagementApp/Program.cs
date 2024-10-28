using TaskManagementApp.Data;
using Microsoft.EntityFrameworkCore;
using TaskManagementApp.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Adding services to the container.
builder.Services.AddDbContext<TaskContext>(options =>
    options.UseInMemoryDatabase("TaskBoard"));

builder.Services.AddScoped<ITaskRepository, TaskRepository>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

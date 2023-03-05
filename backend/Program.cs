using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

using (var con = new TicTacContext())
{
    con.Database.EnsureDeleted();
    con.Database.EnsureCreated();
}
builder.Services.AddScoped<TicTacContext, TicTacContext>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHsts();
app.UseHttpsRedirection();

app.MapControllers();

app.Run();

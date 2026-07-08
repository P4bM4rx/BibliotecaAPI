using BibliotecaAPI.Data;
using Microsoft.EntityFrameworkCore;
using BibliotecaAPI.Interfaces;
using BibliotecaAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BibliotecaContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString(
            "BibliotecaConnection"));
});

builder.Services.AddScoped<IAutorService, AutorService>();

builder.Services.AddScoped<ILibroService, LibroService>();

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
var app = builder.Build();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

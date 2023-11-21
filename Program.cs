using Amazon.Auth.AccessControlPolicy;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<JsonOptions>(options =>
     options.SerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddCors();

var app = builder.Build();

app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.MapGet("/", () => "Hi World");

app.MapGet("/control-escolar/alumnos", AlumnosRequestHandlers.ListarAlumnos);

app.MapPost("/Usuarios/Registrar", RegistroRequestHandler.Registrar);

app.MapPost("Usuarios/IniciarSesion", InicioSesionRequestHandler.IniciarSesion);

app.MapPost("Usuarios/RecuperarContrasenia", RecuperacionRequestHandler.RecuperarContra);

app.MapPost("/Categoria/Crear", CategoriaRequestHandler.Crear);

app.MapGet("/Categoria/Listar", CategoriaRequestHandler.Listar);

app.MapGet("/Lenguaje/{idCategoria}", LenguajeRequestHandler.ListarRegistros);

app.MapPost("/Lenguaje/CrearRegistro", LenguajeRequestHandler.CrearRegistro);

app.MapDelete("/Lenguaje/{id}", LenguajeRequestHandler.Eliminar);

app.MapGet("/Lenguaje/Buscar", LenguajeRequestHandler.Buscar);

app.Run();

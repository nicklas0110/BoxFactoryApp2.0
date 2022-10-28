using Application.DTOs;
using AutoMapper;
using BoxFactoryApp;
using BoxFactoryInfrastructure;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine("initializing");

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());


var mapper = new MapperConfiguration(configuration =>
{
    configuration.CreateMap<BoxDTOs, Box>();
}).CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddDbContext<BoxDbContext>(options => options.UseSqlite(
    "Data source=db.db"
));


Application.DependencyResolver
    .DependencyResolverService
    .RegisterApplicationLayer(builder.Services);

BoxFactoryInfrastructure.DependencyResolver
    .DependencyResolverService
    .RegisterInfrastructure(builder.Services);

builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(options => {
        options.AllowAnyOrigin();
        options.AllowAnyHeader();
        options.AllowAnyMethod();
    });
    
}




app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
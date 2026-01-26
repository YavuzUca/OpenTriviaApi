using Microsoft.EntityFrameworkCore;
using Trivia.Models;
using Trivia.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<OpenTriviaContext>(opt =>
    opt.UseInMemoryDatabase("OpenTriviaResults"))
    ;
builder.Services.AddHttpClient<IOpenTriviaApiService, OpenTriviaApiService> (client =>
{
    client.BaseAddress = new Uri("https://opentdb.com/");
})
    .SetHandlerLifetime(TimeSpan.FromMinutes(1));

builder.Services.AddHttpClient<IOpenTriviaTokenService, OpenTriviaTokenService>(client =>
{
    client.BaseAddress = new Uri("https://opentdb.com/");
})
    .SetHandlerLifetime(TimeSpan.FromMinutes(1));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

if (builder.Environment.IsDevelopment())
{
    app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseCors("AllowAngular");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

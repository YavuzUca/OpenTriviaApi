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
    opt.UseInMemoryDatabase("OpenTriviaResults"));
builder.Services.AddScoped<IOpenTriviaTokenService, OpenTriviaTokenService>();
builder.Services.AddScoped<IOpenTriviaApiService, OpenTriviaApiService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddHttpClient<IOpenTriviaApiService, OpenTriviaApiService>(client =>
{
    client.BaseAddress = new Uri("https://opentdb.com/");
    client.Timeout = TimeSpan.FromSeconds(30);
});
builder.Services.AddHttpClient<IOpenTriviaTokenService, OpenTriviaTokenService>(client =>
{
    client.BaseAddress = new Uri("https://opentdb.com/");
    client.Timeout = TimeSpan.FromSeconds(30);
});
builder.Services.AddHttpClient<IUserService, UserService>(client =>
{
    client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
    client.Timeout = TimeSpan.FromSeconds(30);
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod();
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

using Microsoft.EntityFrameworkCore;
using Movies.Api.Handlers;
using Movies.Api.Modules;
using Movies.Application;
using Movies.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddOpenApi(options =>
{
    options.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_0;
});

builder.Services.AddDbContext<MoviesDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DbConnectionString"));
});

builder.Services.AddApplication();
builder.Services.AddExceptionHandler<ExceptionHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
    
    app.MapScalarApiReference(options =>
        {
            options
                .WithTitle("Movies.Api")
                .WithTheme(ScalarTheme.Mars)
                .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.Http);
        });
}

app.UseExceptionHandler(_ => { });
app.AddMoviesEndpoints();
app.Run();

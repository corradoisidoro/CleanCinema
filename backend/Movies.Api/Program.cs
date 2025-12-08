using Microsoft.EntityFrameworkCore;
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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
        {
            options
                .WithTitle("Movies.Api")
                .WithTheme(ScalarTheme.Mars)
                .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.Http);
        });
}

app.Run();

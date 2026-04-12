using System.Reflection;
using AboriginalArtGallery.Api.Data;
using AboriginalArtGallery.Api.Interfaces;
using AboriginalArtGallery.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod();
        }
    );
});

// used DefaultConnection as it contains sensitive information.
// and that is stored in appsettings.json which is not committed to version control.
// this way we can keep our connection string secure and easily configurable for
// different environments (development, staging, production) without changing the code.

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsqlOptions => npgsqlOptions.UseNetTopologySuite()
    // UseNetTopologySuite is required to enable spatial data support for PostGIS geometry types like Point.
    )
);

builder.Services.AddScoped<IExhibitionService, ExhibitionService>();
builder.Services.AddScoped<ITribeService, TribeService>();
builder.Services.AddScoped<IArtistService, ArtistService>();
builder.Services.AddScoped<IArtifactService, ArtifactService>();
builder.Services.AddScoped<ICommentService, CommentService>();

builder.Services.AddEndpointsApiExplorer(); // for Swagger/OpenAPI documentation generation
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.MapControllers();

app.Run();

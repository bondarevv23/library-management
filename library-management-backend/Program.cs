using FluentValidation;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Exceptions.Handlers;
using LibraryManagementSystem.Repositories;
using LibraryManagementSystem.Services;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using static LibraryManagementSystem.Constants.ConfigurationConstants;

var builder = WebApplication.CreateBuilder(args);

{
    builder.Services.AddControllers();
    builder.Services.AddSwaggerGen();

    var settings = builder
        .Configuration.GetSection(APPLICATION_SETTINGS_PREFIX_NAME)
        .Get<ApplicationSettings>();
    builder.Services.AddSingleton(settings);

    builder.Logging.AddConsole();

    builder.Services.AddDbContext<LibraryManagementSystemContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString(DEFAULT_CONNECTION))
    );

    var redisConfig = builder.Configuration.GetSection(REDIS_CONNECTION_STRING).Value;
    var redis = await ConnectionMultiplexer.ConnectAsync(redisConfig!);

    builder.Services.AddSingleton<IConnectionMultiplexer>(redis);
    builder.Services.AddSingleton<ICacheService, CacheService>();

    builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
    builder.Services.AddScoped<IBookRepository, BookRepository>();
    builder.Services.AddScoped<IAuthorService, AuthorService>();
    builder.Services.AddScoped<IBookService, BookService>();

    builder.Services.AddValidatorsFromAssemblyContaining<Program>();
    builder.Services.AddScoped<IValidationService, ValidationService>();

    builder.Services.AddAutoMapper(typeof(Program));
}

var app = builder.Build();

{
    DbInitializer.Initialize(app.Configuration.GetConnectionString(DEFAULT_CONNECTION)!);

    app.UseMiddleware<ApplicationExceptionHandler>();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.MapControllers();

    app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
}

await app.RunAsync();

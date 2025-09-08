using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://127.0.0.1:5500")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(e => e.Value.Errors.Count > 0)
                .Select(e => new
                {
                    Field = e.Key,
                    Errors = e.Value.Errors.Select(x => x.ErrorMessage)
                });

            return new BadRequestObjectResult(new
            {
                Message = "Errores de validación",
                Errors = errors
            });
        };
    });

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("moviesLimiter", c =>
    {
        c.Window = TimeSpan.FromSeconds(10);
        c.PermitLimit = 5;
        c.QueueLimit = 0;
    });
});

var app = builder.Build();

// Pipeline
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.UseRateLimiter();

app.MapControllers()
   .RequireRateLimiting("moviesLimiter");

app.Run();
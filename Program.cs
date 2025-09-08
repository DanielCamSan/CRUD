using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("MiPoliticaCors", policy =>
    {
        policy.WithOrigins("https://localhost:7162",
                  "http://127.0.0.1:5500")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// ==== Rate Limiting ====
builder.Services.AddRateLimiter(options =>
{
    // Opcional: fija un código por defecto (igual lo podemos setear en OnRejected)
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.OnRejected = async (ctx, ct) =>
    await ctx.HttpContext.Response.WriteAsync("Too many attempts, try it in one minute", ct);


    options.AddFixedWindowLimiter("default", config =>
    {
        config.PermitLimit = 3;                    //numero de requests
        config.Window = TimeSpan.FromMinutes(1);     // en 1 minuto
        config.QueueLimit = 0;                       // sin cola
    });
});

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseCors("MiPoliticaCors");


app.UseAuthorization();
app.UseRateLimiter();
app.MapControllers().RequireRateLimiting("default");
// Aplica la política a todos los controladores


app.Run();
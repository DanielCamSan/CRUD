using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy("MiPoliticaCorsTeam03", policy =>
    {
        policy.WithOrigins("https://localhost:7162",
            "http://127.0.0.1:5510")
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.OnRejected = async (ctx, ct) =>
    await ctx.HttpContext.Response.WriteAsync("Too many attempts, try it in one minute");
    options.AddFixedWindowLimiter("default", config =>
    {
        config.PermitLimit = 4;
        config.Window = TimeSpan.FromMinutes(1);
        config.QueueLimit = 0;
    });
});

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseCors("MiPoliticaCorsTeam03");

app.UseAuthorization();

app.MapControllers();

app.Run();

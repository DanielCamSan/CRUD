using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsPolicy", policy =>
    {
        policy.WithOrigins(
            "http://127.0.0.1:5510")
        .AllowAnyMethod()
        .AllowAnyHeader();
    }
    );
}
);

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.OnRejected = async (ctx, ct) =>
    await ctx.HttpContext.Response.WriteAsync("Too many attempts, try again in 1 minute", ct);

    options.AddFixedWindowLimiter("default", config =>
    {
        config.PermitLimit = 3;                   
        config.Window = TimeSpan.FromMinutes(1);    
        config.QueueLimit = 0;                       
    });
}
);

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseCors("MyCorsPolicy");

app.UseAuthorization();

app.UseRateLimiter();

app.MapControllers().RequireRateLimiting("default");

app.Run();

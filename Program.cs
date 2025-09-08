using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicyCors", policy =>
    {
        policy.WithOrigins(
            "http://127.0.0.1:5500")
                .AllowAnyMethod()
                .AllowAnyHeader();
    });
});

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.OnRejected = async (ctx, ct) =>
    await ctx.HttpContext.Response.WriteAsync("Too many requests sent, try again in 5 minutes", ct);
    options.AddFixedWindowLimiter("default", config =>
    {
        config.PermitLimit = 6;
        config.Window = TimeSpan.FromMinutes(5);
        config.QueueLimit = 0;

    });

});
// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseRateLimiter();
app.UseCors("MyPolicyCors");

app.MapControllers().RequireRateLimiting("default");

app.Run();

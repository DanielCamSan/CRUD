using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("Mi_Cors", policy =>
    {
        policy.WithOrigins("http://127.0.0.1:5500").AllowAnyMethod().AllowAnyHeader();
    });
}
);

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("default", limiterOptions =>
    {
        limiterOptions.PermitLimit = 5; 
        limiterOptions.Window = TimeSpan.FromSeconds(60);
        limiterOptions.QueueLimit = 0; 
    });
});


var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors("Mi_Cors");
app.UseRateLimiter();
app.MapControllers().RequireRateLimiting("default");

app.Run();

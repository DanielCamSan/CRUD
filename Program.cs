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

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors();
app.UseRateLimiter();
app.MapControllers().RequireRateLimiting("default");

app.Run();

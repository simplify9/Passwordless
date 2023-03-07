using Microsoft.EntityFrameworkCore;
using SW.Auth.Web;
using SW.Auth.Web;
using SW.HttpExtensions;
using SW.Logger;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSwLogger();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureApp(builder.Configuration, builder.Environment.EnvironmentName);
builder.Services.AddCors(options =>
{
    options.AddPolicy("SiteCorsPolicy",
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var requiredService = scope.ServiceProvider.GetRequiredService<SwDbContext>();
    try
    {
        requiredService.Database.Migrate();
    }
    finally
    {
        ((IDisposable)requiredService)?.Dispose();
    }
}

app.UseCors(b => b.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpAsRequestContext();
app.MapControllers();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/api/swagger.json", "Auth API V1");
    c.RoutePrefix = "docs";
});

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
app.Run();
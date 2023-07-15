using MessagingAppApi.Model;
using MessagingAppApi.Profiles;
using MessagingAppApi.Shared.Exceptions;
using MessagingAppApi.Shared.Security;
using MessagingAppApi.Shared.Swagger;
using MessagingAppApi.Hubs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddErrorHandling();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .WithOrigins("http://localhost:3000");
    });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<MessagingAppDbContext>(x => x.UseSqlServer(connectionString));


builder.Services.AddSignalR(cfg=> cfg.EnableDetailedErrors = true);

builder.Services.AddJwt(builder.Configuration);
builder.Services.AddAuthentication();


builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerOptions();

builder.Services.AddAutoMapperProfiles();

var app = builder.Build();

app.UseErrorHandling();
app.UseCors("CorsPolicy");


app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("MessagingAppApi");
    });
    endpoints.MapHub<MessageHub>("/messagehub");
});

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MessagingAppDbContext>();
    await db.Database.MigrateAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerOptions();
}

app.UseHttpsRedirection();



app.MapControllers();

app.Run();

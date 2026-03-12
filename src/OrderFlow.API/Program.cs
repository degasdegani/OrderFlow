using Microsoft.EntityFrameworkCore;
using OrderFlow.API.Configurations;
using OrderFlow.API.Middlewares;
using OrderFlow.Infrastructure.Data;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .WriteTo.Console());

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerConfiguration();
    builder.Services.AddInfrastructure(builder.Configuration);

    var app = builder.Build();

    // Seed database
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await context.Database.MigrateAsync();
        await DatabaseSeeder.SeedAsync(context);
    }

    app.UseMiddleware<GlobalExceptionMiddleware>();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "OrderFlow API v1");
            c.RoutePrefix = string.Empty;
        });
    }

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    Log.Information("OrderFlow API iniciada com sucesso!");

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Aplicação encerrada inesperadamente.");
}
finally
{
    Log.CloseAndFlush();
}

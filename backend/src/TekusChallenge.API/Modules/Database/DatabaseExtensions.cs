using Microsoft.EntityFrameworkCore;
using TekusChallenge.Infrastructure.Data;

namespace TekusChallenge.API.Modules.Database;

public static class DatabaseExtensions
{
    public static WebApplication ApplyMigrations(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "A error has occurred while applying migrations.");
                throw;
            }
        }

        return app;
    }
}


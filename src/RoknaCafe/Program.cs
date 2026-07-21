using Microsoft.EntityFrameworkCore;
using Rokna.Infrastructure.Data;
using Rokna.Domain.Interfaces;
using Rokna.Infrastructure.Repositories;
using Rokna.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace RoknaCafe;

static class Program
{
    [STAThread]
    static async Task Main()
    {
        try
        {
            var services = new ServiceCollection();

            services.AddDbContext<RoknaDbContext>(options =>
                options.UseSqlite(new RoknaDbContextFactory().CreateDbContext().Database.GetDbConnection().ConnectionString)
                       .ConfigureWarnings(warnings => warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning)));

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IMenuItemRepository, MenuItemRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IMenuItemService, MenuItemService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<Form1>();

            var serviceProvider = services.BuildServiceProvider();

            ApplicationConfiguration.Initialize();

            using (var scope = serviceProvider.CreateScope())
            {
                var form = scope.ServiceProvider.GetRequiredService<Form1>();
                form.Shown += async (s, e) =>
                {
                    form.Enabled = false;
                    try
                    {
                        var initScope = serviceProvider.CreateScope();
                        using var ctx = initScope.ServiceProvider.GetRequiredService<RoknaDbContext>();
                        await ctx.Database.EnsureCreatedAsync();
                        await DbInitializer.SeedAsync(ctx);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"DB init failed: {ex.Message}", "Startup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    form.Enabled = true;
                    await form.InitializeAsync();
                };

                Application.Run(form);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString(), "Startup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
using SignalRWithSqlTableDependency.Hubs;
using SignalRWithSqlTableDependency.SubscribeTableDependencies;

namespace SignalRWithSqlTableDependency
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSignalR();

            // DI
            builder.Services.AddSingleton<DashboardHub>();
            builder.Services.AddSingleton<SubscribeProductTableDependency>();
            builder.Services.AddSingleton<SubscribeSaleTableDependency>();
            builder.Services.AddSingleton<SubscribeCustomerTableDependency>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();
            app.MapHub<DashboardHub>("/dashboardHub");
            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Dashboard}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}

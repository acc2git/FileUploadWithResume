using FileUploadWithResume.Web.Options;
using FileUploadWithResume.Web.Repositories;
using FileUploadWithResume.Web.Services;

namespace FileUploadWithResume.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.Configure<FileUploadOption>(configuration.GetSection("FileUploadOption"));

            builder.Services.AddScoped<IFileUploadService, FileUploadService>();
            builder.Services.AddSingleton<IFileUploadRepository, FileUploadRepository>();

            var app = builder.Build();

            app.UseHsts();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

using HubWalks.Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.HttpOverrides;

namespace HubWalks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ===== Services =====
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<HubWalksDbContext>(options =>
                options.UseSqlServer(connectionString, b => b.MigrationsAssembly("HubWalks.Data")));

            // Em DEV, mostra páginas detalhadas de erro de banco
            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            }

            builder.Services
                .AddDefaultIdentity<IdentityUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = true;
                    // Ajustes de senha/lockout se quiser...
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<HubWalksDbContext>();

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // ===== Pipeline =====
            if (app.Environment.IsDevelopment())
            {
                // Páginas de erro amigáveis para migrations/dev
                app.UseMigrationsEndPoint();
                // Opcional: app.UseDeveloperExceptionPage();
            }
            else
            {
                // Nunca exponha detalhes em produção
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();

                // (Opcional, mas recomendado se estiver atrás de proxy/load balancer)
                // Isso ajuda a respeitar X-Forwarded-Proto/For e manter HTTPS correto.
                app.UseForwardedHeaders(new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
                });
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // >>> Você tinha Identity mas faltava habilitar:
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            // Importante: você disse que já aplicou as migrations no servidor,
            // então NÃO vamos chamar db.Database.Migrate() aqui.
            // (Se um dia quiser automatizar, coloque dentro de if (app.Environment.IsDevelopment()).

            app.Run();
        }
    }
}

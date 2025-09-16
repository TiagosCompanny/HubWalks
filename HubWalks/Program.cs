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

            // Em DEV, mostra p�ginas detalhadas de erro de banco
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
                // P�ginas de erro amig�veis para migrations/dev
                app.UseMigrationsEndPoint();
                // Opcional: app.UseDeveloperExceptionPage();
            }
            else
            {
                // Nunca exponha detalhes em produ��o
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();

                // (Opcional, mas recomendado se estiver atr�s de proxy/load balancer)
                // Isso ajuda a respeitar X-Forwarded-Proto/For e manter HTTPS correto.
                app.UseForwardedHeaders(new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
                });
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // >>> Voc� tinha Identity mas faltava habilitar:
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            // Importante: voc� disse que j� aplicou as migrations no servidor,
            // ent�o N�O vamos chamar db.Database.Migrate() aqui.
            // (Se um dia quiser automatizar, coloque dentro de if (app.Environment.IsDevelopment()).

            app.Run();
        }
    }
}

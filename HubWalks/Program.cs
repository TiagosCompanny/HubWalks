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
            var connectionString = "Server=192.168.1.158,1433;Database=HubWalks;User Id=TiagoCarvalho;Password=Crespo@123;TrustServerCertificate=True;MultipleActiveResultSets=True";

            builder.Services.AddDbContext<HubWalksDbContext>(options =>
                options.UseSqlServer(connectionString, b => b.MigrationsAssembly("HubWalks.Data")));

            // (Opcional) Em DEV, páginas detalhadas para erros de banco:
            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            }

            builder.Services
                .AddDefaultIdentity<IdentityUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = true;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<HubWalksDbContext>();

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // ===== Pipeline =====

            // FORÇA DETALHES DE EXCEÇÃO MESMO EM PRODUÇÃO (TEMPORÁRIO)
            app.UseDeveloperExceptionPage();

            // Se estiver atrás de proxy/load balancer (IIS/NGINX/Azure), preserve IP/PROTO
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            // Em modo "debug temporário" não usamos UseExceptionHandler/HSTS,
            // para não esconder a página detalhada de erro.
            // -> Quando terminar o diagnóstico, REMOVA a linha acima
            //    e reative o bloco seguro abaixo:
            //
            // if (!app.Environment.IsDevelopment())
            // {
            //     app.UseExceptionHandler("/Home/Error");
            //     app.UseHsts();
            // }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            // Não aplicar migrations automaticamente em produção
            // (você disse que já aplicou manualmente).
            // Se um dia precisar, faça dentro de if (app.Environment.IsDevelopment()).

            app.Run();
        }
    }
}

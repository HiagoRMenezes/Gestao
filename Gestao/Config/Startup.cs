using Microsoft.IdentityModel.Tokens;

namespace Gestao.Config
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Gerar chave secreta
            TokenConfig.GenerateKey();

            // Adicionar serviços à aplicação
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Bearer";
                options.DefaultChallengeScheme = "Bearer";
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = TokenConfig.Key
                };
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configurar aplicação
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}


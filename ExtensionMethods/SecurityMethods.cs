using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebApiMagazin.ExtensionMethods
{
    public static class SecurityMethods
    {
        public const string DEFAULT_POLICY = "DEFAULT_POLICY";

        public static void AddCustomSecurity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCustomCors(configuration);
            services.AddCustomAuthentification(configuration);
        }
        public static void AddCustomAuthentification(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });
        }
        public static void AddCustomCors(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                //Pour ajouter un policy, c'est comme un dictionnaire (clé et valeur)
                //On peut en mettre en tant qu'on veut
                options.AddPolicy(DEFAULT_POLICY, builder =>
                {
                    //Autorize tous
                    //builder.AllowAnyOrigin();
                    builder.AllowAnyHeader()
                           .AllowAnyMethod();
                    //Pour specifier les origines
                    builder.WithOrigins(configuration["Cors:Origin"]!);
                });
            });
        }
    }
}

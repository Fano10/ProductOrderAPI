using System.Runtime.CompilerServices;
using WebApiMagazin.Data;
using WebApiMagazin.ExtensionMethods;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ContextDb>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//mon methode d'extension pour Services
builder.Services.AddCustomSecurity(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//Pour l'aunthentification
app.UseAuthentication();
app.UseAuthorization();

//Pour configurer la politique d'autorisation de CORS
app.UseCors(SecurityMethods.DEFAULT_POLICY);

app.MapControllers();

app.Run();

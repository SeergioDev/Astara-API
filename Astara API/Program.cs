using Astara_API.DataAccess.mytasks.Context;
using Astara_API.DataModel;
using Astara_API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//Añadimos swagger + autenticación en Swagger
builder.Services.AddSwaggerGen(x => {

    x.SwaggerDoc("v1", new OpenApiInfo { 
        Title = "Astara Move API", 
        Version = "v1"
    });

    x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Bearer Authorization",
        Description = "Please, enter a valid token",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
    });

    x.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Id = "Bearer", Type = ReferenceType.SecurityScheme }
            },
            new string []{ }
        }
    });

    x.EnableAnnotations(); 
});

//Añadimos los ficheros de configuración
var jwtToken = builder.Configuration.GetSection(nameof(JWT));
builder.Services.Configure<JWT>(jwtToken);

//Añadimos autenticacion por JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true,
        ValidAudience = configuration["JWT:Audience"],
        ValidIssuer = configuration["JWT:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"])),
        ClockSkew = TimeSpan.Zero
    };

});

//Añadimos conexiones a SQL
builder.Services.AddDbContextFactory<myTasksContext>(options => options.UseSqlServer(configuration.GetConnectionString("myTasksDB")));

//Añadimos servicios
builder.Services.AddTransient<IUserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//Usamos autenticación de JWT
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();



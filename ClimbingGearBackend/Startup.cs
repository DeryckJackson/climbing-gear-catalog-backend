using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using ClimbingGearBackend.Infrastucture;
using ClimbingGearBackend.Models;

namespace ClimbingGearBackend
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddDbContext<ClimbingGearContext>(opt =>
        opt.UseNpgsql(Configuration.GetConnectionString("ClimbingGearContext")));

      services.AddControllers();
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "ClimbingGearBackend", Version = "v1" });
      });

      services.AddDatabaseDeveloperPageExceptionFilter();

      services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
        .AddEntityFrameworkStores<ClimbingGearContext>();

      services.AddIdentityServer()
        .AddApiAuthorization<User, ClimbingGearContext>();

      services.AddAuthentication().AddIdentityServerJwt();
    }

    protected virtual void ConfigureDatabaseServices(IServiceCollection services)
    {
      services.AddDbContext<ClimbingGearContext>(opt =>
        opt.UseNpgsql(Configuration.GetConnectionString("ClimbingGearContext")));
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ClimbingGearBackend v1"));
        app.UseMigrationsEndPoint();
      }
      else
      {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();
      app.UseIdentityServer();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}

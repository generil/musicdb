using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Music.Infrastructure.UnitsOfWork;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Serilog;
using PostgresSink;
using AspNet.Security.OpenIdConnect.Primitives;
using OpenIddict.Core;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using Music.Presentation;
using Music.Application;
using Music.Application.IService;
using Music.Infrastructure.Repositories;
using Music.Domain.Entities;
using Music.Domain.IRepositories;
using Music.Infrastructure.Logging;

namespace MusicDB
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.PostgreSqlServer(Configuration.GetConnectionString("gen"), "logs")
                .WriteTo.LiterateConsole()
                .CreateLogger();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            var connection = new NpgsqlConnection(Configuration.GetConnectionString("gen"));

            services.AddDbContext<FirstGenUnitOfWork>(option => option.UseNpgsql(connection));
            services.AddDbContext<SecondGenUnitOfWork>(option => option.UseNpgsql(connection));

            services.AddScoped<MainUnitOfWork, MainUnitOfWork>();

            services.AddScoped<IArtistService, ArtistService>();
            services.AddScoped<IArtistRepository, ArtistRepository>();

            services.AddScoped<IAlbumService, AlbumService>();
            services.AddScoped<IAlbumRepository, AlbumRepository>();

            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<IGenreRepository, GenreRepository>();

            services.AddScoped<ITrackService, TrackService>();
            services.AddScoped<ITrackRepository, TrackRepository>();

            services.AddMvc();
            services.AddAutoMapper();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
            Logger.LoggerFactory = loggerFactory.AddSerilog();

            app.UseCors(builder =>
            {
                builder.WithOrigins("http://192.168.143.180:4200", "http://localhost:4200");
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
            });

            // Disable the automatic JWT -> WS-Federation claims mapping used by the JWT middleware:
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

            // Authenticate users on a separate server
            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                Audience = "FedAuth",
                Authority = "http://localhost:5110/",
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                RequireHttpsMetadata = false,
                TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = OpenIdConnectConstants.Claims.Name,
                    RoleClaimType = OpenIdConnectConstants.Claims.Role
                }
            });

            app.UseMvc();
            appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
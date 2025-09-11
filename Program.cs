using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using TMG_Site_API.Data;
using TMG_Site_API.NodeAPI.Services;
using TMG_Site_API.ChainCrawlerService;


namespace TMG_Site_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            // Add services to the container.
            
            //Razor Pages
            builder.Services.AddRazorPages();

            //EF Core - SQLite DB
            builder.Services.AddDbContextFactory<TmgPoolApiContext>(options =>
              options.UseSqlite(builder.Configuration.GetConnectionString("TmgPoolApiContext") ?? throw new InvalidOperationException("Connection string 'TmgPoolApiContext' not found.")));

            //HTTP client factory
            builder.Services.AddHttpClient<ISignumAPIService, SignumAPIService>().ConfigureHttpClient(httpClient =>
            {
                httpClient.BaseAddress = new(builder.Configuration.GetSection("NodeSettings")["NodeAddress"] ?? throw new InvalidOperationException("Connection string 'NodeAddress' not found."));

            });

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();


            //Background worker service
            builder.Services.AddSingleton<TmgPriceService>();
            builder.Services.AddHostedService(serviceCollection =>
                serviceCollection.GetRequiredService<TmgPriceService>());







            var app = builder.Build();

            
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseHttpsRedirection();

                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }



            //Get DB created and setup
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<TmgPoolApiContext>();
                context.Database.EnsureCreated();

                //DbInitializer.Initialize(context);


            }


            // app.UseHttpsRedirection();

            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapRazorPages()
               .WithStaticAssets();


            //API Enpoint Mapping
            app.MapTmgPriceEndpoints();


            app.Run();
        }
    }
}

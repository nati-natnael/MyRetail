using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.OpenApi.Models;
using System.Net.Http;
using MongoDB.Driver;
using MongoDB.Bson;

namespace MyRetail
{
    public class Startup
    {
        private const string _dbUsername = "MyRetail";
        private const string _dbPassword = "myretail";
        private const string _dbName = "MyRetailDB";
        private const string _collectioName = "Products";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyRetail", Version = "v1" });
            });

            services.AddScoped<IProductManager, ProductManager>();

            services.AddScoped<IProductRepository>(c =>
            {
                string dbConnectionString = $"mongodb+srv://{_dbUsername}:{_dbPassword}@cluster0.88sdm.mongodb.net";

                IMongoClient client = new MongoClient(dbConnectionString);
                IMongoDatabase database = client.GetDatabase(_dbName);
                IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>(_collectioName);

                return new ProductRepository(new HttpClient(), collection);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => 
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyRetail v1");
                c.RoutePrefix = "swagger";
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

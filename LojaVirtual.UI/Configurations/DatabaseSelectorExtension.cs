using LojaVirtual.Data;
using LojaVirtual.UI.Data;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.UI.Configurations
{
    public static class DatabaseSelectorExtension
    {
        public static void AddDatabaseSelector(this WebApplicationBuilder builder)
        {
            if (builder.Environment.IsDevelopment())
            {
                //builder.Services.AddDbContext<LojaVirtualDbContext>(options =>
                //options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

                //builder.Services.AddDbContext<ApplicationDbContext>(options =>
                //    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

                builder.Services.AddDbContext<LojaVirtualDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnectionLite")));

                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnectionLite")));
            }
            else
            {
                builder.Services.AddDbContext<LojaVirtualDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            }

        }
    }
}

using LojaVirtual.Data;
using LojaVirtual.Data.Model;
using LojaVirtual.UI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.UI.Configurations
{
    public static class DbMigrationHelperExtension
    {
        public static void UseDbMigrationHelper(this WebApplication app)
        {
            DbMigrationHelpers.EnsureSeedData(app).Wait();
        }
    }

    public static class DbMigrationHelpers
    {
        public static async Task EnsureSeedData(WebApplication serviceScope)
        {
            var services = serviceScope.Services.CreateScope().ServiceProvider;
            await EnsureSeedData(services);
        }

        public static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

            var context = scope.ServiceProvider.GetRequiredService<LojaVirtualDbContext>();
            var contextId = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if (env.IsDevelopment() || env.IsEnvironment("Docker") || env.IsStaging())
            {
                await context.Database.MigrateAsync();
                await contextId.Database.MigrateAsync();

                await EnsureSeedProducts(context, contextId);
            }
        }

        private static async Task EnsureSeedProducts(LojaVirtualDbContext context, ApplicationDbContext contextId)
        {
            if (context.Vendedores.Any())
                return;

            var userId = Guid.NewGuid().ToString();

            await contextId.Users.AddAsync(new IdentityUser
            {
                Id = userId,
                UserName = "teste@teste.com",
                NormalizedUserName = "TESTE@TESTE.COM",
                Email = "teste@teste.com",
                NormalizedEmail = "TESTE@TESTE.COM",
                AccessFailedCount = 0,
                LockoutEnabled = false,
                PasswordHash = "AQAAAAIAAYagAAAAEEdWhqiCwW/jZz0hEM7aNjok7IxniahnxKxxO5zsx2TvWs4ht1FUDnYofR8JKsA5UA==",
                TwoFactorEnabled = false,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            });

            await contextId.SaveChangesAsync();

            await context.Vendedores.AddAsync(new Vendedor
            {
                Id = userId,
                Nome = "Test"
            });

            await context.SaveChangesAsync();

            if (context.Categorias.Any())
                return;

            await context.Categorias.AddAsync(new Categoria
            {
                Titulo = "Celular",
                Descricao = "Celulares e Smartphones"
            });

            await context.Categorias.AddAsync(new Categoria
            {
                Titulo = "Tablet",
                Descricao = "Tablets"
            });

            await context.SaveChangesAsync();

            if (context.Produtos.Any())
                return;

            int categoriaId = context.Categorias.Where(x => x.Titulo == "Celular").Select(x=> x.Id).FirstOrDefault();
            if (categoriaId> 0)
            {
                await context.Produtos.AddAsync(new Produto()
                {
                    Titulo = "iPhone 15 128GB",
                    Valor = 4599,
                    Descricao = "O iPhone 15 traz a Dynamic Island, câmera grande-angular de 48 MP e USB-C. Tudo em um vidro resistente colorido por infusão e design em alumínio.",
                    Estoque = 100,
                    Imagem = "/images/4e9debbc-077e-4f62-b3f4-2f98d2403f5c_iphone15.jpg",
                    CategoriaId = categoriaId,
                    VendedorId = userId
                });

                await context.Produtos.AddAsync(new Produto()
                {
                    Titulo = "iPhone 16 256GB",
                    Valor = 5998,
                    Descricao = "iPhone 16. Novo Controle da Câmera, câmera Fusion de 48 MP, cinco cores lindas e o chip A18.",
                    Estoque = 100,
                    Imagem = "/images/a73ef993-ce30-43e5-8424-9842fef4d601_iphone16.jpg",
                    CategoriaId = categoriaId,
                    VendedorId = userId
                });

            }

            categoriaId = context.Categorias.Where(x => x.Titulo == "Tablet").Select(x => x.Id).FirstOrDefault();
            if (categoriaId > 0)
            {
                await context.Produtos.AddAsync(new Produto()
                {
                    Titulo = "iPad Apple (9° Geração) A13 Bionic",
                    Valor = 3647,
                    Descricao = "Tela de 10,2\", 5G + Wi-Fi, 64GB de Capacidade, Processador A13 Bionic\r\n",
                    Estoque = 50,
                    Imagem = "/images/ef0a69b2-8457-4ee5-b2f2-bcf7b5f8a0d2_ipadGeracao9.jpg",
                    CategoriaId = categoriaId,
                    VendedorId = userId
                });

                await context.Produtos.AddAsync(new Produto()
                {
                    Titulo = "iPad Pro Apple (5ª Geração) Processador M4",
                    Valor = 20999,
                    Descricao = "iPad Pro Apple (5ª Geração) Processador M4 (11\", Wi-Fi + Celular, 1TB com Vidro com Nano-Texture) - Preto-Espacial",
                    Estoque = 30,
                    Imagem = "/images/2f082d32-28a1-477d-8c08-312e5bdaafdf_ipadPro.jpeg",
                    CategoriaId = categoriaId,
                    VendedorId = userId
                });

            }

            await context.SaveChangesAsync();

            if (contextId.Users.Any())
                return;

        }
    }
}

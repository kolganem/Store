using Microsoft.AspNetCore.Identity;
using StoreProject.Infrastructure.Extensions;
using StoreProject.Models;

namespace StoreProject;

public static class DataInitializer
{
    private const string Password = "testStore";
    public static WebApplication Seed(this WebApplication app)
    {
        using (IServiceScope scope = app.Services.CreateScope())
        {
            using StoreDbContext context = scope.ServiceProvider.GetRequiredService<StoreDbContext>();
            PasswordHasher<User> passwordHasher = new PasswordHasher<User>();

            try
            {
                context.Database.EnsureCreated();

                if (!context.Categories.Any())
                {
                    context.Categories.AddRange(
                        new Category
                        {
                            Id = (int)Categories.Food,
                            Name = Categories.Food.GetDisplayName()
                        },
                        new Category
                        {
                            Id = (int)Categories.Goodies,
                            Name = Categories.Goodies.GetDisplayName()
                        },
                        new Category
                        {
                            Id = (int)Categories.Water,
                            Name = Categories.Water.GetDisplayName()
                        }
                    );
                }

                if (!context.Products.Any())
                {
                    context.Products.AddRange(
                        new Product
                        {
                            Name = "Селёдка",
                            CategoryId = (int)Categories.Food,
                            Info = "Селёдка солёная",
                            Price = 10_000,
                            GeneralNote = "Акция",
                            SpecialNote = "Пересоленая"
                        },
                        new Product
                        {
                            Name = "Тушёнка",
                            CategoryId = (int)Categories.Food,
                            Info = "Тушенка говяжья",
                            Price = 20_000,
                            GeneralNote = "Вкусная",
                            SpecialNote = "Жилы"
                        },
                        new Product
                        {
                            Name = "Сгущёнка",
                            CategoryId = (int)Categories.Goodies,
                            Info = "В банках",
                            Price = 30_000,
                            GeneralNote = "С ключом",
                            SpecialNote = "Вкусная"
                        },
                        new Product
                        {
                            Name = "Квас",
                            CategoryId = (int)Categories.Water,
                            Info = "В бутылках",
                            Price = 15_000,
                            GeneralNote = "Вятский",
                            SpecialNote = "Тёплый"
                        }
                    );
                }

                if (!context.Roles.Any())
            {
                context.Roles.AddRange(
                new IdentityRole
                {
                        Id = "1",
                        Name = Roles.User,
                        NormalizedName = Roles.User.ToUpper()
                    },
                    new IdentityRole
                    {
                        Id = "2",
                        Name = Roles.ProfessionalUser,
                        NormalizedName = Roles.ProfessionalUser.ToUpper()
                    },
                    new IdentityRole
                    {
                        Id = "3",
                        Name = Roles.Administrator,
                        NormalizedName = Roles.Administrator.ToUpper()
                    }
                );
            }

                context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new OperationCanceledException("Can't seed initial data to DB");
            }
        }

        return app;
    }
}
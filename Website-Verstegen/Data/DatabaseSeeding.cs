using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website_Verstegen.Models;

namespace Website_Verstegen.Data
{
    public class DatabaseSeeding
    {
        public static void SeedDatabase(DatabaseContext context)
        {
            if (!context.Certificates.Any() && !context.Packagings.Any() && !context.Products.Any() && !context.ProductDetails.Any() && !context.Counties.Any() && !context.Recipes.Any())
            {
                List<Province> SeedProvinces = new List<Province>
                    {
                        new Province
                        {
                            Naam = "ProvinceOne",
                            Country = new Country{ Naam = "Zwitserland", Code = "1001" },
                            People = new List<ContactPerson> {
                                new ContactPerson
                                {
                                    Naam = "Yta",
                                    Functie = "Poker",
                                    Telefoonnummer = 00011,
                                    EmailAdres = "ruto123@hotmail.nl",
                                    ImageUrl = "\\images\\spotlightPartial\\piment.jpg",
                                },
                                new ContactPerson
                                {
                                    Naam = "Ita",
                                    Functie = "Polo",
                                    Telefoonnummer = 87968,
                                    EmailAdres = "pato345@hotmail.com",
                                    ImageUrl = "\\images\\spotlightPartial\\beakers.jpg",
                                }
                            }
                        },
                        new Province
                        {
                            Naam = "Zuid-Holland",
                            Country = new Country { Naam = "Nederland", Code = "070"},
                            People = new List<ContactPerson>
                            {
                                new ContactPerson
                                {
                                    Naam = "Paul",
                                    Functie = "Paulio",
                                    Telefoonnummer = 98769,
                                    EmailAdres = "pauldevries@gov.nl",
                                    ImageUrl = "\\images\\spotlightPartial\\bbq.jpg",
                                },
                                new ContactPerson
                                {
                                    Naam = "Bob",
                                    Functie = "Bouwer",
                                    Telefoonnummer = 54321,
                                    EmailAdres = "bobdebouwer@bouwer.com",
                                    ImageUrl = "\\images\\spotlightPartial\\turkey.jpg",
                                },
                                new ContactPerson
                                {
                                    Naam = "Ernst",
                                    Functie = "Bobbie",
                                    Telefoonnummer = 12345,
                                    EmailAdres = "bobbieernst@ernstbobbie.nl",
                                    ImageUrl = "\\images\\spotlightPartial\\farmersausages.jpg"
                                }
                            }
                        },
                        new Province
                        {
                            Naam = "Texas",
                            Country = new Country { Naam = "United States of America", Code = "999"},
                            People = new List<ContactPerson>
                            {
                                new ContactPerson
                                {
                                    Naam = "Barack Obama",
                                    Functie = "Professionele tweeter",
                                    Telefoonnummer = 19283,
                                    EmailAdres = "Yobama@gov.com",
                                    ImageUrl = "\\images\\Products\\TW_ext.jpg"
                                }
                            }
                        },
                        new Province
                        {
                            Naam = "Geen idee",
                            Country = new Country { Naam = "Portugal", Code = "787" },
                            People = new List<ContactPerson>
                            {
                                new ContactPerson
                                {
                                    Naam = "Cristiano Ronaldo",
                                    Functie = "Professioneel hoofd",
                                    Telefoonnummer = 77777,
                                    EmailAdres = "cronaldo@juventus.nl",
                                    ImageUrl = "\\images\\Products\\T.jpg"
                                }
                            }
                        },
                        new Province
                        {
                            Naam = "Another one",
                            Country = new Country { Naam = "Japan", Code = "564" },
                            People = new List<ContactPerson>
                            {
                                new ContactPerson
                                {
                                    Naam = "Anime",
                                    Functie = "Wow",
                                    Telefoonnummer = 56756,
                                    EmailAdres = "anime@jpn.jpn",
                                    ImageUrl = "\\images\\Products\\base 1 th11.jpg"
                                }
                            }
                        }
                    };

                List<RecipeIngredient> SeedRecipeIngredients = new List<RecipeIngredient>
                    {
                        new RecipeIngredient
                        {
                            Recipe = new Recipe
                            {
                                Title = "RecipeOne",
                                Category = "Soup",
                                ImageUrl = "\\images\\Products\\RfO.jpg",
                                PreparationSteps = new List<PreparationStep>
                                {
                                    new PreparationStep
                                    {
                                        Text = "1. Do this"
                                    },
                                    new PreparationStep
                                    {
                                        Text = "2. Do that"
                                    }
                                }
                            },
                            Ingredient = new Ingredient
                            {
                                Name = "Peper",
                                Unit = "500g",
                                Amount = 1
                            }
                        },
                        new RecipeIngredient
                        {
                            Recipe = new Recipe
                            {
                                Title = "Goed recept",
                                Category = "Sushi",
                                ImageUrl = "\\images\\Products\\TW_ext.jpg",
                                PreparationSteps = new List<PreparationStep>
                                {
                                    new PreparationStep
                                    {
                                        Text = "1. Make sushi"
                                    },
                                    new PreparationStep
                                    {
                                        Text = "2. Eat it"
                                    },
                                    new PreparationStep
                                    {
                                        Text = "3. Done"
                                    }
                                }
                            },
                            Ingredient = new Ingredient
                            {
                                Name = "Vis",
                                Unit = "100g",
                                Amount = 50
                            }
                        },
                        new RecipeIngredient
                        {
                            Recipe = new Recipe
                            {
                                Title = "Recept titel",
                                Category = "Sushi",
                                ImageUrl = "\\images\\Products\\Header-Verstegen.png",
                                PreparationSteps = new List<PreparationStep>
                                {
                                    new PreparationStep
                                    {
                                        Text = "1. Klaar"
                                    }
                                }
                            },
                            Ingredient = new Ingredient
                            {
                                Name = "Peper",
                                Unit = "1000g",
                                Amount = 20
                            }
                        },
                        new RecipeIngredient
                        {
                            Recipe = new Recipe
                            {
                                Title = "Amazing",
                                Category = "Soep",
                                ImageUrl = "\\images\\recipes\\groentes.png",
                                PreparationSteps = new List<PreparationStep>
                                {
                                    new PreparationStep
                                    {
                                        Text = "1. Eat"
                                    },
                                    new PreparationStep
                                    {
                                        Text = "2. Eat again"
                                    }
                                }
                            },
                            Ingredient = new Ingredient
                            {
                                Name = "Mouth",
                                Unit = "1m",
                                Amount = 1
                            }
                        },
                        new RecipeIngredient
                        {
                            Recipe = new Recipe
                            {
                                Title = "Shoarma",
                                Category = "Meat",
                                ImageUrl = "\\images\\recipes\\burgers.jpg",
                                PreparationSteps = new List<PreparationStep>
                                {
                                    new PreparationStep
                                    {
                                        Text = "1. Get meat"
                                    },
                                    new PreparationStep
                                    {
                                        Text = "2. Eat meat"
                                    }
                                }
                            },
                            Ingredient = new Ingredient
                            {
                                Name = "Ingredientje",
                                Unit = "10g",
                                Amount = 2
                            }
                        },
                    };

                List<Certificate> SeedCertificates = new List<Certificate>
                    {
                        new Certificate {
                            Alt_Text = "Alternate text",
                            Img_Src = "\\images\\certificates\\certificate-1.PNG",
                            Name = "Goede naam"
                        },
                        new Certificate {
                            Alt_Text = "Prague",
                            Img_Src = "\\images\\certificates\\certificate-2.PNG",
                            Name = "China"
                        },
                        new Certificate {
                            Alt_Text = "Alt",
                            Img_Src = "\\images\\certificates\\certificate-3.PNG",
                            Name = "Ghana"
                        },
                        new Certificate {
                            Alt_Text = "Paars",
                            Img_Src = "\\images\\certificates\\certificate-4.PNG",
                            Name = "Nieuw Zeeland"
                        },
                        new Certificate {
                            Alt_Text = "Rocket",
                            Img_Src = "\\images\\certificates\\certificate-5.PNG",
                            Name = "Duitsland"
                        },
                    };

                List<Packaging> SeedPackagings = new List<Packaging>
                    {
                        new Packaging {
                            Category = "Huisverhuur",
                            Title = "Pakket een",
                            AltText = "Alternate text",
                            Contents = "Veel",
                            Description = "Lange description",
                            ImageUrl = "\\images\\Products\\food-1.png",
                            PackagingHeight = 11,
                            PackagingWidth = 22
                        },
                        new Packaging {
                            Category = "Fast food",
                            Title = "Seven Eleven",
                            AltText = "Rarama",
                            Contents = "Weinig",
                            Description = "Korte description",
                            ImageUrl = "\\images\\Products\\Dog.jpg",
                            PackagingHeight = 23,
                            PackagingWidth = 33
                        },
                        new Packaging {
                            Category = "Slow food",
                            Title = "Subway",
                            AltText = "Supreme",
                            Contents = "Pewdiepie",
                            Description = "Omschrijving",
                            ImageUrl = "\\images\\Products\\burgers.jpg",
                            PackagingHeight = 34,
                            PackagingWidth = 44
                        },
                        new Packaging {
                            Category = "Krakau",
                            Title = "KFC",
                            AltText = "Aliexpress",
                            Contents = "Ad revenue",
                            Description = "Gek",
                            ImageUrl = "\\images\\Products\\vi.jpg",
                            PackagingHeight = 45,
                            PackagingWidth = 55
                        },
                        new Packaging {
                            Category = "Lama",
                            Title = "McDonalds",
                            AltText = "Ikea",
                            Contents = "Big",
                            Description = "Hoe dan",
                            ImageUrl = "\\images\\Products\\T.jpg",
                            PackagingHeight = 56,
                            PackagingWidth = 66
                        }
                    };

                List<Product> SeedProducts = new List<Product>
                    {
                        new Product {
                            Title = "Froot loops",
                            Category = "Categorie een",
                            Contents = "Veel",
                            ImageUrl = "\\images\\products\\froot loops.jpg",
                            Description = "Omschrijving",
                            DownloadLink = "12345.com",
                            ProductDetails = new List<ProductDetails>
                            {
                                new ProductDetails {
                                    Details = "Een goede combinatie"
                                },
                                new ProductDetails {
                                    Details = "Een slechte combinatie"
                                }
                            },
                            ThemeProducts = new List<ThemeProduct>
                            {
                                new ThemeProduct
                                {
                                    Theme = new Theme
                                    {
                                        ThemeName = "BBQ",
                                        Description = "Barbeque",
                                        ImageUrl = "\\images\\themes\\bbq.jpg",
                                        Stories = new List<Story>()
                                        {
                                            new Story
                                            {
                                                Title = "Nieuwe story",
                                                Description = "Nieuw",
                                                Category = "Categorie een",
                                                ImageUrl = "\\images\\stories\\storyeen.jpg"
                                            }
                                        }
                                    }
                                }
                            }
                        },
                        new Product {
                            Title = "Cornflakes",
                            Category = "Categorie een",
                            Contents = "Weinig",
                            ImageUrl = "\\images\\products\\cornflakes.jpg",
                            Description = "Om aan het schrijven",
                            DownloadLink = "GoogleAPI.com",
                            ProductDetails = new List<ProductDetails>
                            {
                                new ProductDetails {
                                    Details = "Goed product"
                                },
                                new ProductDetails {
                                    Details = "Raar product"
                                },
                                new ProductDetails {
                                    Details = "Vaag product"
                                }
                            },
                            ThemeProducts = new List<ThemeProduct>
                            {
                                new ThemeProduct
                                {
                                    Theme = new Theme
                                    {
                                        ThemeName = "Easter",
                                        Description = "Easter description",
                                        ImageUrl = "\\images\\themes\\easter.jpg",
                                        Stories = new List<Story>()
                                        {
                                            new Story
                                            {
                                                Title = "Spannend verhaal",
                                                Description = "Een spannend verhaal",
                                                Category = "Categorie een",
                                                ImageUrl = "\\images\\stories\\storytwee.jpg"
                                            }
                                        }
                                    }
                                }
                            }
                        },

                        new Product {
                            Title = "Honey loops",
                            Category = "Categorie twee",
                            Contents = "Heel erg",
                            ImageUrl = "\\images\\products\\honey loops.jpg",
                            Description = "Undercover",
                            DownloadLink = "Yaka.com",
                            ProductDetails = new List<ProductDetails>
                            {
                                new ProductDetails {
                                    Details = "Bijzonder"
                                },
                                new ProductDetails {
                                    Details = "Goede prijs"
                                }
                            },
                            ThemeProducts = new List<ThemeProduct>
                            {
                                new ThemeProduct
                                {
                                    Theme = new Theme
                                    {
                                        ThemeName = "Turkey",
                                        Description = "Turkey meat",
                                        ImageUrl = "\\images\\themes\\turkey.jpg",
                                        Stories = new List<Story>()
                                        {
                                            new Story
                                            {
                                                Title = "Actie verhaal",
                                                Description = "Een actie verhaal",
                                                Category = "Categorie twee",
                                                ImageUrl = "\\images\\stories\\storydrie.jpg"
                                            }
                                        }
                                    }
                                },
                                new ThemeProduct
                                {
                                    Theme = new Theme
                                    {
                                        ThemeName = "Christmas",
                                        Description = "Christmas food",
                                        ImageUrl = "\\images\\themes\\christmasfood.jpg",
                                        Stories = new List<Story>()
                                        {
                                            new Story
                                            {
                                                Title = "Andere story",
                                                Description = "Anders",
                                                Category = "Categorie een",
                                                ImageUrl = "\\images\\stories\\storyzes.jpg"
                                            },
                                        }
                                    }
                                }
                            }
                        },
                        new Product {
                            Title = "Coco pops",
                            Category = "Categorie twee",
                            Contents = "Niet heel erg",
                            ImageUrl = "\\images\\products\\coco pops.jpg",
                            Description = "Onder de koffer",
                            DownloadLink = "Ukelele.nl",
                            ProductDetails = new List<ProductDetails>
                            {
                                new ProductDetails {
                                    Details = "Goedkoop"
                                }
                            },
                            ThemeProducts = new List<ThemeProduct>
                            {
                                new ThemeProduct
                                {
                                    Theme = new Theme
                                    {
                                        ThemeName = "Kalfsvlees",
                                        Description = "Vlees van een kalf",
                                        ImageUrl = "\\images\\themes\\cocktails.jpg",
                                        Stories = new List<Story>()
                                        {
                                            new Story
                                            {
                                                Title = "Sprookjes verhaal",
                                                Description = "Een sprookjes verhaal",
                                                Category = "Categorie drie",
                                                ImageUrl = "\\images\\stories\\storyvier.jpg"
                                            },
                                        }
                                    }
                                }
                            }
                        },
                        new Product {
                            Title = "Product naam123",
                            Category = "Categorie drie",
                            Contents = "Vol",
                            ImageUrl = "\\images\\products\\burgers.jpg",
                            Description = "Kaal",
                            DownloadLink = "Raak.gov",
                            ProductDetails = new List<ProductDetails>
                            {
                                new ProductDetails {
                                    Details = "Spicy"
                                },
                                new ProductDetails {
                                    Details = "Hard"
                                }
                            },
                            ThemeProducts = new List<ThemeProduct>
                            {
                                new ThemeProduct
                                {
                                    Theme = new Theme
                                    {
                                        ThemeName = "Feast!",
                                        Description = "Feasten",
                                        ImageUrl = "\\images\\themes\\feast.jpg",
                                        Stories = new List<Story>()
                                        {
                                            new Story
                                            {
                                                Title = "Realistisch",
                                                Description = "Een realistisch verhaal",
                                                Category = "Categorie twee",
                                                ImageUrl = "\\images\\stories\\storyvijf.jpg"
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    };

                List<Blog> SeedBlogs = new List<Blog>
                    {
                        new Blog
                        {
                            Title = "Sport blog",
                            Subtitle = "Blog met veel informatie",
                            Category = "Categorie vier",
                            Content = "Relatie tussen sport en eten",
                            ImageUrl = "\\images\\blogs\\ontbijt.jpg",
                        },
                        new Blog
                        {
                            Title = "Groenten blog",
                            Subtitle = "Blog over groenten",
                            Category = "Categorie vier",
                            Content = "Waarom groenten goed zijn",
                            ImageUrl = "\\images\\blogs\\Dog.jpg",
                        },
                        new Blog
                        {
                            Title = "Eten met veel vitaminen",
                            Subtitle = "Vitamines in eten",
                            Category = "Categorie een",
                            Content = "Vitamine A, B, C, D",
                            ImageUrl = "\\images\\blogs\\rosemary.png",
                        },
                        new Blog
                        {
                            Title = "Blog over veel",
                            Subtitle = "Veel blog",
                            Category = "Categorie twee",
                            Content = "Veel text",
                            ImageUrl = "\\images\\blogs\\vegan-cooking-lessons.jpg",
                        },
                        new Blog
                        {
                            Title = "Vis blog",
                            Subtitle = "Vissen en hoe cool ze zijn",
                            Category = "Categorie drie",
                            Content = "Veel vissen",
                            ImageUrl = "\\images\\blogs\\2013.jpg",
                        }
                    };

                List<SpotLightConnector> SeedSpotlightConnector = new List<SpotLightConnector>
                {
                    new SpotLightConnector
                    {
                        actionName = "Index",
                        controllerName = "Home",
                        SpotlightContent = new List<SpotlightContent>
                        {
                            new SpotlightContent
                            {
                                SpType = "Product",
                                SpId = 1
                            },
                            new SpotlightContent
                            {
                                SpType = "Product",
                                SpId = 2
                            },
                            new SpotlightContent
                            {
                                SpType = "Product",
                                SpId = 3
                            },
                            new SpotlightContent
                            {
                                SpType = "Product",
                                SpId = 4
                            },
                            new SpotlightContent
                            {
                                SpType = "Product",
                                SpId = 5
                            },
                            new SpotlightContent
                            {
                                SpType = "Recipe",
                                SpId = 1
                            },
                            new SpotlightContent
                            {
                                SpType = "Story",
                                SpId = 1
                            },
                            new SpotlightContent
                            {
                                SpType = "Blog",
                                SpId = 2
                            }
                        }
                    }
                };

                List<Category> SeedCategories = new List<Category>()
                {
                    new Category
                    {
                        Title = "Categorie een"
                    },
                    new Category
                    {
                        Title = "Categorie twee"
                    },
                    new Category
                    {
                        Title = "Categorie drie"
                    },
                    new Category
                    {
                        Title = "Categorie vier"
                    },
                    new Category
                    {
                        Title = "Categorie vijf"
                    },
                    new Category
                    {
                        Title = "Filler"
                    },
                    new Category
                    {
                        Title = "Filler"
                    },
                    new Category
                    {
                        Title = "Filler"
                    },
                    new Category
                    {
                        Title = "Filler"
                    },
                    new Category
                    {
                        Title = "Filler"
                    },
                    new Category
                    {
                        Title = "Filler"
                    },
                    new Category
                    {
                        Title = "Filler"
                    },
                    new Category
                    {
                        Title = "Filler"
                    },
                    new Category
                    {
                        Title = "Filler"
                    },
                };

                context.Certificates.AddRange(SeedCertificates);
                context.Packagings.AddRange(SeedPackagings);
                context.Products.AddRange(SeedProducts);
                context.Counties.AddRange(SeedProvinces);
                context.RecipeIngredients.AddRange(SeedRecipeIngredients);
                context.Blogs.AddRange(SeedBlogs);
                context.SpotLightConnectors.AddRange(SeedSpotlightConnector);
                context.Categories.AddRange(SeedCategories);

                context.SaveChanges();
            }
        }
    }
}

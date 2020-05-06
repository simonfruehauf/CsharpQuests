using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloNamespace
{
    class BuildMenu
    {
        
        class MenuItem
        {
            public string Text { get; set; }

            public bool HasSubMenu { get; set; }

            public int? SubMenuId { get; set; }

            public Action Action { get; set; } //anonymous action, to allow for a menu item to do a thing
        }

        class Menu
        {
            public Menu()
            {
                MenuItems = new List<MenuItem>();
            }
            public string Title { get; set; }

            public int MenuId { get; set; }
            public List<MenuItem> MenuItems { get; set; }

            public void PrintToConsole()
            {
                if (Title != "")
                {
                    Console.WriteLine(Title);
                }
                foreach (MenuItem item in MenuItems)
                {
                    Console.WriteLine(MenuItems.IndexOf(item) + " : " + item.Text);
                }
            }
        }

        class MenuCollection
        {
            public MenuCollection()
            {
                Menus = new List<Menu>();
            }

            public List<Menu> Menus { get; set; }
            
            public void ShowMenu(int id)
            {
                var currentMenu = Menus.Where(m => m.MenuId == id).Single();
                currentMenu.PrintToConsole();

                //wait for user input
    
                string choice = Read.String("Please input a number.");
                int choiceIndex;

                if (!int.TryParse(choice, out choiceIndex) || currentMenu.MenuItems.Count < choiceIndex || choiceIndex < 0)
                {
                    Console.Clear();
                    Console.WriteLine("Invalid selection - try again.");
                    ShowMenu(id);
                }
                else
                {
                    var menuItemSelected = currentMenu.MenuItems[choiceIndex];

                    if (menuItemSelected.HasSubMenu)
                    {
                        Console.Clear();
                        ShowMenu(menuItemSelected.SubMenuId.Value);
                    }
                    else
                    {
                        menuItemSelected.Action();
                    }
                }
            }
        }

        public static bool Build()
        {
            MenuCollection collection = new MenuCollection()
            {
                Menus =
            {
                new Menu()
                {
                    Title = "Main",
                    MenuId = 1,
                    MenuItems =
                    {
                        new MenuItem()
                        {
                            Text = "Go to Settings",
                            HasSubMenu = true,
                            SubMenuId = 2
                        },
                        new MenuItem()
                        {
                            Text = "Go to Other",
                            HasSubMenu = true,
                            SubMenuId = 3
                        },
                        new MenuItem()
                        {
                            Text = "Go to Games",
                            HasSubMenu = true,
                            SubMenuId = 5
                        },
                        new MenuItem()
                        {
                            Text = "Quit",
                            HasSubMenu = false,
                            Action =  () =>
                            {
                                Environment.Exit(1);
                            }
                        }
                    }
                },
                new Menu()
                {
                    Title = "Settings",
                    MenuId = 2,
                    MenuItems =
                    {
                        new MenuItem()
                        {
                            Text = "Graphics Settings",
                            HasSubMenu = true,
                            SubMenuId = 4
                        },
                        new MenuItem()
                        {
                            Text = "Back to the top menu",
                            HasSubMenu = true,
                            SubMenuId = 1
                        }
                    }
                },
                new Menu()
                {
                    Title = "Other",
                    MenuId = 3,
                    MenuItems =
                    {
                        new MenuItem()
                        {
                            Text = "Open password protected file",
                            HasSubMenu = false,
                            Action = () =>
                            {
                                string input = Read.String("Please input the password.");
                                if (input == "fuck")
                                    Console.WriteLine("okay");
                                else
                                    Console.WriteLine("wrong password.");
                            }
                        },
                        new MenuItem()
                        {
                            Text = "Say Hello",
                            HasSubMenu = false,
                            Action = () =>
                            {
                                Console.WriteLine("Hello.");
                            }
                        },
                        new MenuItem()
                        {
                            Text = "Back to the top menu",
                            HasSubMenu = true,
                            SubMenuId = 1
                        }
                    }
                },
                new Menu()
                {
                    Title = "Graphics Settings",
                    MenuId = 4,
                    MenuItems =
                    {
                        new MenuItem()
                        {
                            Text = "Say hello",
                            HasSubMenu = false,
                            Action = () =>
                            {
                                Console.WriteLine("Hello.");
                            }
                        },
                        new MenuItem()
                        {
                            Text = "Back to the main menu",
                            HasSubMenu = true,
                            SubMenuId = 1
                        },
                        new MenuItem()
                        {
                            Text = "Back to the previous menu",
                            HasSubMenu = true,
                            SubMenuId = 2
                        }
                    }
                },
                new Menu()
                {
                    Title = "Games",
                    MenuId = 5,
                    MenuItems =
                    {
                        new MenuItem()
                        {
                            Text = "Play Rock Paper Scissors (\"lottery\")",
                            HasSubMenu = false,
                            Action = () =>
                            {
                                RPS RockPaperScissors = new RPS();
                                RockPaperScissors.Play();
                            }
                        },
                        new MenuItem()
                        {
                            Text = "Back to the main menu",
                            HasSubMenu = true,
                            SubMenuId = 1
                        }
                    }
                }
            }
            };

            collection.ShowMenu(1);
            return false;
        }
    }
}

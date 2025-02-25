using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;

namespace DzDapper_1
{
    class MainClass
    {
        static string? connectionString;
        static bool isConnected = false;

        static void Main()
        {
            var builder = new ConfigurationBuilder();
            string path = Directory.GetCurrentDirectory();
            builder.SetBasePath(path);
            builder.AddJsonFile("db.json");
            var config = builder.Build();
            connectionString = config.GetConnectionString("DefaultConnection");

            try
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("1. Подключиться к БД");
                    Console.WriteLine("2. Отображение всех покупателей");
                    Console.WriteLine("3. Отображение Email покупателей");
                    Console.WriteLine("4. Отображение списка разделов");
                    Console.WriteLine("5. Отображение списка акционных товаров");
                    Console.WriteLine("6. Отображение всех городов");
                    Console.WriteLine("7. Отображение всех стран");
                    Console.WriteLine("8. Отображение покупателей по городу");
                    Console.WriteLine("9. Отображение покупателей по стране");
                    Console.WriteLine("10. Отображение акций по стране");
                    Console.WriteLine("0. Выход");
                    Console.Write("\nВыберите действие: ");
                    int result = int.Parse(Console.ReadLine()!);

                    switch (result)
                    {
                        case 1:
                            JoinToDb();
                            break;
                        case 2:
                            ShowCustomers();
                            break;
                        case 3:
                            ShowCustomersEmail();
                            break;
                        case 4:
                            ShowCategories();
                            break;
                        case 5:
                            ShowPromotions();
                            break;
                        case 6:
                            ShowCities();
                            break;
                        case 7:
                            ShowCountries();
                            break;
                        case 8:
                            ShowCustomersByCity();
                            break;
                        case 9:
                            ShowCustomersByCountry();
                            break;
                        case 10:
                            ShowPromotionsByCountry();
                            break;
                        case 0:
                            Exit();
                            return;
                        default:
                            Console.WriteLine("Некорректный выбор. Повторите ввод.");
                            Console.ReadLine();
                            break;
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void JoinToDb()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    isConnected = true;
                    Console.WriteLine("Подключение к базе данных успешно!");
                }
                catch (Exception ex)
                {
                    isConnected = false;
                    Console.WriteLine($"Ошибка подключения к базе данных: {ex.Message}");
                }
            }
            Console.ReadLine();
        }

        static void Exit()
        {
            using (var connection = new SqlConnection(connectionString)) 
            {
                connection.Close(); 
                isConnected = false;
                Console.WriteLine("База данных отключена!");
            }
        }
        static void ShowCustomers()
        {
            if (!isConnected)
            {
                Console.WriteLine("Для отображения покупателей необходимо подключиться к базе данных!");
                Console.ReadLine();
                return;
            }

            using (var connection = new SqlConnection(connectionString))
            {
                var customers = connection.Query("SELECT FirstName, LastName FROM Customers");
                Console.WriteLine("\nСписок всех покупателей:");
                foreach (var customer in customers)
                {
                    Console.WriteLine($"{customer.FirstName} {customer.LastName}");
                }
            }
            Console.ReadLine();
        }

        static void ShowCustomersEmail()
        {
            if (!isConnected)
            {
                Console.WriteLine("Для отображения Email покупателей необходимо подключиться к базе данных!");
                Console.ReadLine();
                return;
            }

            using (var connection = new SqlConnection(connectionString))
            {
                var customers = connection.Query("SELECT FirstName, LastName, Email FROM Customers");
                Console.WriteLine("\nСписок Email покупателей:");
                foreach (var customer in customers)
                {
                    Console.WriteLine($"{customer.FirstName} {customer.LastName} - {customer.Email}");
                }
            }
            Console.ReadLine();
        }

        static void ShowCategories()
        {
            if (!isConnected)
            {
                Console.WriteLine("Для отображения разделов необходимо подключиться к базе данных!");
                Console.ReadLine();
                return;
            }

            using (var connection = new SqlConnection(connectionString))
            {
                var categories = connection.Query("SELECT CategoryName FROM Categories");
                Console.WriteLine("\nСписок разделов:");
                foreach (var category in categories)
                {
                    Console.WriteLine($"{category.CategoryName}");
                }
            }
            Console.ReadLine();
        }

        static void ShowPromotions()
        {
            if (!isConnected)
            {
                Console.WriteLine("Для отображения акционных товаров необходимо подключиться к базе данных!");
                Console.ReadLine();
                return;
            }

            using (var connection = new SqlConnection(connectionString))
            {
                var promotions = connection.Query(@"
                    SELECT c.CategoryName, co.CountryName, p.Description, p.StartDate, p.EndDate 
                    FROM Promotions p
                    INNER JOIN Categories c ON p.CategoryID = c.CategoryID
                    INNER JOIN Countries co ON p.CountryID = co.CountryID");

                Console.WriteLine("\nСписок акционных товаров:");
                foreach (var promotion in promotions)
                {
                    Console.WriteLine($"{promotion.CategoryName} ({promotion.CountryName}) - {promotion.Description} " +
                                      $"с {promotion.StartDate:yyyy-MM-dd} по {promotion.EndDate:yyyy-MM-dd}");
                }
            }
            Console.ReadLine();
        }

        static void ShowCities()
        {
            if (!isConnected)
            {
                Console.WriteLine("Для отображения городов необходимо подключиться к базе данных!");
                Console.ReadLine();
                return;
            }

            using (var connection = new SqlConnection(connectionString))
            {
                var cities = connection.Query("SELECT CityName FROM Cities");
                Console.WriteLine("\nСписок всех городов:");
                foreach (var city in cities)
                {
                    Console.WriteLine($"{city.CityName}");
                }
            }
            Console.ReadLine();
        }

        static void ShowCountries()
        {
            if (!isConnected)
            {
                Console.WriteLine("Для отображения стран необходимо подключиться к базе данных!");
                Console.ReadLine();
                return;
            }

            using (var connection = new SqlConnection(connectionString))
            {
                var countries = connection.Query("SELECT CountryName FROM Countries");
                Console.WriteLine("\nСписок всех стран:");
                foreach (var country in countries)
                {
                    Console.WriteLine($"{country.CountryName}");
                }
            }
            Console.ReadLine();
        }

        static void ShowCustomersByCity()
        {
            if (!isConnected)
            {
                Console.WriteLine("Для отображения покупателей необходимо подключиться к базе данных!");
                Console.ReadLine();
                return;
            }

            Console.Write("Введите название города: ");
            string city = Console.ReadLine()!;

            using (var connection = new SqlConnection(connectionString))
            {
                var customers = connection.Query(@"
                    SELECT c.FirstName, c.LastName 
                    FROM Customers c
                    INNER JOIN CustomerAddresses ca ON c.CustomerID = ca.CustomerID
                    INNER JOIN Cities ci ON ca.CityID = ci.CityID
                    WHERE ci.CityName = @CityName", new { CityName = city });

                Console.WriteLine($"\nПокупатели из города {city}:");
                foreach (var customer in customers)
                {
                    Console.WriteLine($"{customer.FirstName} {customer.LastName}");
                }
            }
            Console.ReadLine();
        }

        static void ShowCustomersByCountry()
        {
            if (!isConnected)
            {
                Console.WriteLine("Для отображения покупателей необходимо подключиться к базе данных!");
                Console.ReadLine();
                return;
            }

            Console.Write("Введите название страны: ");
            string country = Console.ReadLine()!;

            using (var connection = new SqlConnection(connectionString))
            {
                var customers = connection.Query(@"
                    SELECT c.FirstName, c.LastName 
                    FROM Customers c
                    INNER JOIN CustomerAddresses ca ON c.CustomerID = ca.CustomerID
                    INNER JOIN Cities ci ON ca.CityID = ci.CityID
                    INNER JOIN Countries co ON ci.CountryID = co.CountryID
                    WHERE co.CountryName = @CountryName", new { CountryName = country });

                Console.WriteLine($"\nПокупатели из страны {country}:");
                foreach (var customer in customers)
                {
                    Console.WriteLine($"{customer.FirstName} {customer.LastName}");
                }
            }
            Console.ReadLine();
        }

        static void ShowPromotionsByCountry()
        {
            if (!isConnected)
            {
                Console.WriteLine("Для отображения акций необходимо подключиться к базе данных!");
                Console.ReadLine(); 
                return;
            }

            Console.Write("Введите название страны: ");
            string country = Console.ReadLine()!;

            using (var connection = new SqlConnection(connectionString))
            {
                var promotions = connection.Query(@"
                    SELECT c.CategoryName, p.Description, p.StartDate, p.EndDate
                    FROM Promotions p
                    INNER JOIN Categories c ON p.CategoryID = c.CategoryID
                    INNER JOIN Countries co ON p.CountryID = co.CountryID
                    WHERE co.CountryName = @CountryName", new { CountryName = country });

                Console.WriteLine($"\nАкции в стране {country}:");
                foreach (var promotion in promotions)
                {
                    Console.WriteLine($"{promotion.CategoryName} - {promotion.Description} с {promotion.StartDate:yyyy-MM-dd} по {promotion.EndDate:yyyy-MM-dd}");
                }
            }
            Console.ReadLine();
        }
    }
}

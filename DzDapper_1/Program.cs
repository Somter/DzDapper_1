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
                    Console.WriteLine("11. Вставка информации о новых покупателях;");
                    Console.WriteLine("12. Вставка новых стран");
                    Console.WriteLine("13. Вставка новых городов");
                    Console.WriteLine("14. Вставка информации о новых разделах");
                    Console.WriteLine("15. Вставка информации о новых акционных товарах");
                    Console.WriteLine("16. Обновление информации о покупателях");
                    Console.WriteLine("17. Обновление информации о странах");
                    Console.WriteLine("18. Обновление информации о городах");
                    Console.WriteLine("19. Обновление информации о разделах");
                    Console.WriteLine("21. Удаление покупателя");
                    Console.WriteLine("22. Удаление страны");
                    Console.WriteLine("23. Удаление города");
                    Console.WriteLine("24. Удаление раздела");
                    Console.WriteLine("25. Удаление акции");
                    Console.WriteLine("26. Отображение списка городов конкретной страны");
                    Console.WriteLine("27. Отображение списка разделов конкретного покупателя");
                    Console.WriteLine("28. Отображение списка акционных товаров конкретного раздела");

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
                        case 11:
                            AddNewCustomer();  
                            break;
                        case 12:
                            AddCountry();
                            break;
                        case 13:
                            AddCity();
                            break;
                        case 14:
                            AddCategory();
                            break;
                        case 15:
                            AddPromotion();
                            break;
                        case 16:
                            UpdateCustomer();
                            break;
                        case 17:
                            UpdateCountry();
                            break;
                        case 18:
                            UpdateCity();
                            break;
                        case 19:
                            UpdateCategory();
                            break;
                        case 20:
                            UpdatePromotion();
                            break;
                        case 21:
                            DeleteCustomer();
                            break;
                        case 22:
                            DeleteCountry();
                            break;
                        case 23:
                            DeleteCity();
                            break;
                        case 24:
                            DeleteCategory();
                            break;
                        case 25:
                            DeletePromotion();
                            break;
                        case 26: 
                            ShowCitiesByCountry();
                            break;
                        case 27: 
                            ShowCategoriesByCustomer();
                            break;
                        case 28:
                            ShowPromotionsByCategory();
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

        static void AddNewCustomer()
        {
            Console.Clear();
            Console.Write("Введите имя: ");
            string firstName = Console.ReadLine()!;
            Console.Write("Введите фамилию: ");
            string lastName = Console.ReadLine()!;
            Console.Write("Введите дату рождения (гггг-мм-дд): ");
            DateTime birthDate = DateTime.Parse(Console.ReadLine()!);
            Console.Write("Введите пол (М/Ж): ");
            string gender = Console.ReadLine()!;
            Console.Write("Введите Email: ");
            string email = Console.ReadLine()!;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = "INSERT INTO Customers (FirstName, LastName, BirthDate, Gender, Email) VALUES (@FirstName, @LastName, @BirthDate, @Gender, @Email)";
                db.Execute(sqlQuery, new { FirstName = firstName, LastName = lastName, BirthDate = birthDate, Gender = gender, Email = email });
                Console.WriteLine("Покупатель добавлен.");
            }
            Console.ReadKey();
        }

        static void AddCountry()
        {
            Console.Write("Введите название страны: ");
            string countryName = Console.ReadLine()!;
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute("INSERT INTO Countries (CountryName) VALUES (@CountryName)", new { CountryName = countryName });
                Console.WriteLine("Страна добавлена.");
            }
            Console.ReadLine();
        }

        static void AddCity()
        {
            Console.Write("Введите название города: ");
            string cityName = Console.ReadLine()!;
            Console.Write("Введите ID страны: ");
            int countryId = int.Parse(Console.ReadLine()!);
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute("INSERT INTO Cities (CityName, CountryID) VALUES (@CityName, @CountryID)", new { CityName = cityName, CountryID = countryId });
                Console.WriteLine("Город добавлен.");
            }
            Console.ReadLine();
        }

        static void AddCategory()
        {
            Console.Write("Введите название раздела: ");
            string categoryName = Console.ReadLine()!;
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute("INSERT INTO Categories (CategoryName) VALUES (@CategoryName)", new { CategoryName = categoryName });
                Console.WriteLine("Раздел добавлен.");
            }
            Console.ReadLine();
        }

        static void AddPromotion()
        {
            Console.Write("Введите ID категории: ");
            int categoryId = int.Parse(Console.ReadLine()!);
            Console.Write("Введите ID страны: ");
            int countryId = int.Parse(Console.ReadLine()!);
            Console.Write("Введите дату начала (гггг-мм-дд): ");
            DateTime startDate = DateTime.Parse(Console.ReadLine()!);
            Console.Write("Введите дату окончания (гггг-мм-дд): ");
            DateTime endDate = DateTime.Parse(Console.ReadLine()!);
            Console.Write("Введите описание: ");
            string description = Console.ReadLine()!;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Execute("INSERT INTO Promotions (CategoryID, CountryID, StartDate, EndDate, Description) VALUES (@CategoryID, @CountryID, @StartDate, @EndDate, @Description)",
                    new { CategoryID = categoryId, CountryID = countryId, StartDate = startDate, EndDate = endDate, Description = description });
                Console.WriteLine("Акция добавлена.");
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

        static void UpdateCustomer()
        {
            Console.Write("Введите ID покупателя: ");
            int id = int.Parse(Console.ReadLine()!);
            Console.Write("Введите новое имя: ");
            string firstName = Console.ReadLine()!;
            Console.Write("Введите новую фамилию: ");
            string lastName = Console.ReadLine()!;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = "UPDATE Customers SET FirstName = @FirstName, LastName = @LastName WHERE CustomerID = @ID";
                db.Execute(sqlQuery, new { ID = id, FirstName = firstName, LastName = lastName });
                Console.WriteLine("Данные покупателя обновлены.");
            }
            Console.ReadKey();
        }

        static void UpdateCountry()
        {
            Console.Write("Введите ID страны: ");
            int id = int.Parse(Console.ReadLine()!);
            Console.Write("Введите новое название страны: ");
            string countryName = Console.ReadLine()!;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = "UPDATE Countries SET CountryName = @CountryName WHERE CountryID = @ID";
                db.Execute(sqlQuery, new { ID = id, CountryName = countryName });
                Console.WriteLine("Данные страны обновлены.");
            }
            Console.ReadKey();
        }

        static void UpdateCity()
        {
            Console.Write("Введите ID города: ");
            int id = int.Parse(Console.ReadLine()!);
            Console.Write("Введите новое название города: ");
            string cityName = Console.ReadLine()!;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = "UPDATE Cities SET CityName = @CityName WHERE CityID = @ID";
                db.Execute(sqlQuery, new { ID = id, CityName = cityName });
                Console.WriteLine("Данные города обновлены.");
            }
            Console.ReadKey();
        }

        static void UpdateCategory()
        {
            Console.Write("Введите ID раздела: ");
            int id = int.Parse(Console.ReadLine()!);
            Console.Write("Введите новое название раздела: ");
            string categoryName = Console.ReadLine()!;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = "UPDATE Categories SET CategoryName = @CategoryName WHERE CategoryID = @ID";
                db.Execute(sqlQuery, new { ID = id, CategoryName = categoryName });
                Console.WriteLine("Данные раздела обновлены.");
            }
            Console.ReadKey();
        }

        static void UpdatePromotion()
        {
            Console.Write("Введите ID акции: ");
            int id = int.Parse(Console.ReadLine()!);
            Console.Write("Введите новое описание акции: ");
            string description = Console.ReadLine()!;

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = "UPDATE Promotions SET Description = @Description WHERE PromotionID = @ID";
                db.Execute(sqlQuery, new { ID = id, Description = description });
                Console.WriteLine("Данные акции обновлены.");
            }
            Console.ReadKey();
        }

        static void DeleteCustomer()
        {
            Console.Write("Введите ID покупателя для удаления: ");
            int id = int.Parse(Console.ReadLine()!);

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = "DELETE FROM Customers WHERE CustomerID = @ID";
                db.Execute(sqlQuery, new { ID = id });
                Console.WriteLine("Покупатель удален.");
            }
            Console.ReadKey();
        }

        static void DeleteCountry()
        {
            Console.Write("Введите ID страны для удаления: ");
            int id = int.Parse(Console.ReadLine()!);

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = "DELETE FROM Countries WHERE CountryID = @ID";
                db.Execute(sqlQuery, new { ID = id });
                Console.WriteLine("Страна удалена.");
            }
            Console.ReadKey();
        }

        static void DeleteCity()
        {
            Console.Write("Введите ID города для удаления: ");
            int id = int.Parse(Console.ReadLine()!);

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = "DELETE FROM Cities WHERE CityID = @ID";
                db.Execute(sqlQuery, new { ID = id });
                Console.WriteLine("Город удален.");
            }
            Console.ReadKey();
        }

        static void DeleteCategory()
        {
            Console.Write("Введите ID раздела для удаления: ");
            int id = int.Parse(Console.ReadLine()!);

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = "DELETE FROM Categories WHERE CategoryID = @ID";
                db.Execute(sqlQuery, new { ID = id });
                Console.WriteLine("Раздел удален.");
            }
            Console.ReadKey();
        }

        static void DeletePromotion()
        {
            Console.Write("Введите ID акции для удаления: ");
            int id = int.Parse(Console.ReadLine()!);

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string sqlQuery = "DELETE FROM Promotions WHERE PromotionID = @ID";
                db.Execute(sqlQuery, new { ID = id });
                Console.WriteLine("Акция удалена.");
            }
            Console.ReadKey();
        }

        public static void ShowCitiesByCountry()
        {
            try
            {
                Console.Write("Введите ID страны: ");
                int countryId = int.Parse(Console.ReadLine()!);

                using (var connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT CityName FROM Cities WHERE CountryID = @CountryID";
                    var cities = connection.Query<string>(query, new { CountryID = countryId }).ToList();

                    if (cities.Count > 0)
                    {
                        Console.WriteLine("Города в выбранной стране:");
                        foreach (var city in cities)
                        {
                            Console.WriteLine(city);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Нет городов для выбранной страны.");
                    }
                }
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        public static void ShowCategoriesByCustomer()
        {
            try
            {
                Console.Write("Введите ID покупателя: ");
                int customerId = int.Parse(Console.ReadLine()!);

                using (var connection = new SqlConnection(connectionString))
                {
                    string query = @"
                    SELECT c.CategoryName
                    FROM Categories c
                    INNER JOIN CustomerInterests ci ON c.CategoryID = ci.CategoryID
                    WHERE ci.CustomerID = @CustomerID";
                    var categories = connection.Query<string>(query, new { CustomerID = customerId }).ToList();

                    if (categories.Count > 0)
                    {
                        Console.WriteLine("Разделы интересов покупателя:");
                        foreach (var category in categories)
                        {
                            Console.WriteLine(category);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Покупатель не имеет интересов в разделах.");
                    }
                }
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        public static void ShowPromotionsByCategory()
        {
            try
            {
                Console.Write("Введите ID раздела: ");
                int categoryId = int.Parse(Console.ReadLine()!);

                using (var connection = new SqlConnection(connectionString))
                {
                    string query = @"
                    SELECT p.Description, p.StartDate, p.EndDate
                    FROM Promotions p
                    WHERE p.CategoryID = @CategoryID";
                    var promotions = connection.Query<(string Description, DateTime StartDate, DateTime EndDate)>(query, new { CategoryID = categoryId }).ToList();

                    if (promotions.Count > 0)
                    {
                        Console.WriteLine("Акции для выбранного раздела:"); 
                        foreach (var promo in promotions)
                        {
                            Console.WriteLine($"{promo.Description} (С {promo.StartDate.ToShortDateString()} по {promo.EndDate.ToShortDateString()})");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Нет акций для выбранного раздела.");
                    }
                }
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}

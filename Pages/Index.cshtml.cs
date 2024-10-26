using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace ASP2.Pages;

public class IndexModel : PageModel
{
    public List<CarInfo> CarList { get; set; } = new List<CarInfo>();
    public void OnGet()
    {
        // Optionally load all cars on the initial page load or leave it empty.
    }
    public void OnPost(string pickupDate, string returnDate, string year)
    {
        try
        {
            string connectionString = "Server=JOE;Database=rentcars;Trusted_Connection=True;TrustServerCertificate=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();


                // string sql = @"
                // SELECT c.*, ci.image_link
                // FROM MsCar c
                // LEFT JOIN MsCarImages ci ON c.Car_id = ci.Car_id
                // LEFT JOIN TrRental r ON c.Car_id = r.car_id
                // WHERE (r.rental_date <= @ReturnDate AND r.return_date >= @PickupDate)
                // " + (string.IsNullOrEmpty(year) ? "" : " AND c.[year] = @Year")+
                // " GROUP BY c.Car_id, c.name, c.model, c.year, c.license_plate, c.number_of_car_seats, c.transmission, c.price_per_day, c.status";

                string sql = @"
                SELECT c.Car_id, c.name, c.model, c.year, c.license_plate, c.number_of_car_seats, 
                c.transmission, c.price_per_day, c.status, 
                MAX(ci.image_link) AS image_link
                FROM MsCar c
                LEFT JOIN MsCarImages ci ON c.Car_id = ci.Car_id
                LEFT JOIN TrRental r ON c.Car_id = r.car_id
                WHERE (r.rental_date <= @ReturnDate AND r.return_date >= @PickupDate)
                " + (string.IsNullOrEmpty(year) ? "" : " AND c.[year] = @Year") +
                " GROUP BY c.Car_id, c.name, c.model, c.year, c.license_plate, c.number_of_car_seats, c.transmission, c.price_per_day, c.status";



                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@PickupDate", DateTime.Parse(pickupDate));
                    command.Parameters.AddWithValue("@ReturnDate", DateTime.Parse(returnDate));

                    if (!string.IsNullOrEmpty(year))
                    {
                        command.Parameters.AddWithValue("@Year", int.Parse(year));
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CarInfo carInfo = new CarInfo
                            {
                                Car_id = reader.GetString(reader.GetOrdinal("Car_id")),
                                name = reader.GetString(reader.GetOrdinal("name")),
                                model = reader.IsDBNull(reader.GetOrdinal("model")) ? null : reader.GetString(reader.GetOrdinal("model")),
                                year = reader.GetInt32(reader.GetOrdinal("year")),
                                license_plate = reader.IsDBNull(reader.GetOrdinal("license_plate")) ? null : reader.GetString(reader.GetOrdinal("license_plate")),
                                number_of_car_seats = reader.GetInt32(reader.GetOrdinal("number_of_car_seats")),
                                transmission = reader.IsDBNull(reader.GetOrdinal("transmission")) ? null : reader.GetString(reader.GetOrdinal("transmission")),
                                price_per_day = reader.GetDecimal(reader.GetOrdinal("price_per_day")),
                                status = reader.GetBoolean(reader.GetOrdinal("status")),
                                image_link = reader.IsDBNull(reader.GetOrdinal("image_link")) ? null : reader.GetString(reader.GetOrdinal("image_link"))

                            };

                            // Add the carInfo object to the CarList
                            CarList.Add(carInfo);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Log the error or handle it appropriately
            Console.WriteLine("Error: " + ex.Message);
        }
    }
    public class CarInfo
    {
        public string Car_id { get; set; }
        public string name { get; set; }
        public string model { get; set; }
        public int year { get; set; }
        public string license_plate { get; set; }
        public int number_of_car_seats { get; set; }
        public string transmission { get; set; }
        public decimal price_per_day { get; set; }
        public bool status { get; set; }
        public string image_link { get; set; }

    }

}

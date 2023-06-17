using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsAppLab2.Entities;

namespace WindowsFormsApp
{
    class DbUtils
    {
        static string connectionString = "Server=localhost;Database=sensor_temperature_data;Uid=root;Pwd=vladyslav228;";

        public static void InsertData(int sensorId, double temperature, DateTime timestamp)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO sensor_readings (sensor_id, temperature, timestamp) VALUES (@sensor_id, @temperature, @timestamp)";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@sensor_id", sensorId);
                    command.Parameters.AddWithValue("@temperature", temperature);
                    command.Parameters.AddWithValue("@timestamp", timestamp);

                    command.ExecuteNonQuery();
                }
                connection.Close();

            }

        }

        public static User GetUser(string username, string password)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = $"SELECT * FROM users WHERE name='{username}' && password = SHA('{password}')";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return new User
                            {
                                Id = reader.GetInt32("id"),
                                Name = reader.GetString("name"),
                                Password = reader.GetString("password"),
                                Permission = reader.GetString("permission")
                            };
                        }
                    }
                }
                connection.Close();
            }
            return null;
        }

        public static void RetrieveData()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM sensor_readings";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int sensorId = reader.GetInt32("sensor_id");
                            double temperature = reader.GetDouble("temperature");
                            DateTime timestamp = reader.GetDateTime("timestamp");

                            Console.WriteLine($"Sensor ID: {sensorId}, Temperature: {temperature}, Timestamp: {timestamp}");
                        }
                    }
                }
                connection.Close();
            }
        }

        public static int GetMaxTemperature(int sensorId, int durationInSeconds)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = $"SELECT MAX(temperature) AS temperature FROM sensor_readings WHERE timestamp >= NOW() - INTERVAL {durationInSeconds} SECOND and sensor_id={sensorId};";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int temperature = reader.GetInt32("temperature");
                            connection.Close();

                            return temperature;
                        }
                    }
                }
            }
            return 0;
        }

        public static int GetMinTemperature(int sensorId, int durationInSeconds)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = $"SELECT MIN(temperature) AS temperature FROM sensor_readings WHERE timestamp >= NOW() - INTERVAL {durationInSeconds} SECOND and sensor_id={sensorId};";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int temperature = reader.GetInt32("temperature");
                            connection.Close();

                            return temperature;
                        }
                    }
                }
            }
            return 0;
        }

        public static int GetAvgTemperature(int sensorId, int durationInSeconds)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = $"SELECT AVG(temperature) AS temperature FROM sensor_readings WHERE timestamp >= NOW() - INTERVAL {durationInSeconds} SECOND and sensor_id={sensorId};";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int temperature = reader.GetInt32("temperature");
                            connection.Close();

                            return temperature;
                        }
                    }
                }
            }
            return 0;
        }

        public static int GetLatestTemperature(int sensorId)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = $"SELECT temperature AS temperature FROM sensor_readings WHERE sensor_id={sensorId} order by timestamp desc limit 1;";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int temperature = reader.GetInt32("temperature");
                            connection.Close();

                            return temperature;
                        }
                    }
                }
            }
            return 0;
        }
    }
}

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.Models
{
    public class DotNetCoreContext
    {
        public string ConnectionString { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public DotNetCoreContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<CityInfo> GetAllCities()
        {
            List<CityInfo> list = new List<CityInfo>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM cityinfo", conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new CityInfo()
                        {
                            ID = reader.GetInt32("id"),
                            City = reader.GetString("city"),
                            Lat = reader.GetDecimal("lat"),
                            Lon = reader.GetDecimal("lon")
                        });
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CityInfo GetCity(int id)
        {
            CityInfo city = new CityInfo();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM cityinfo WHERE id = " + id, conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        city.ID = reader.GetInt32("id");
                        city.City = reader.GetString("city");
                        city.Lat = reader.GetDecimal("lat");
                        city.Lon = reader.GetDecimal("lon");
                    }
                }
            }

            return city;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="city"></param>
        public int InsertCity(CityInfo city)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO cityinfo(city,lat,lon) VALUES(@city, @lat,@lon)", conn);
                cmd.Parameters.AddWithValue("@city", city.City);
                cmd.Parameters.AddWithValue("@lat", city.Lat);
                cmd.Parameters.AddWithValue("@lon", city.Lon);
                cmd.ExecuteNonQuery();
                return (int)cmd.LastInsertedId;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public int UpdateCity(CityInfo city)
        {
            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("UPDATE cityinfo SET city = @city,lat = @lat,lon = @lon WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", city.ID);
                    cmd.Parameters.AddWithValue("@city", city.City);
                    cmd.Parameters.AddWithValue("@lat", city.Lat);
                    cmd.Parameters.AddWithValue("@lon", city.Lon);
                    cmd.ExecuteNonQuery();
                    return city.ID;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteCity(int id)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("DELETE FROM cityinfo WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                return id;
            }
        }
    }
}

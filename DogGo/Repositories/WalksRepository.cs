using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public class WalksRepository : IWalksRepository
    {
        private readonly IConfiguration _config;

        public WalksRepository(IConfiguration config)
        {
            _config = config;
        }
        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public List<Walks> GetAllWalks()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT w.Id,
                           w.Date,
                           w.Duration,
                           w.dogId,
                           w.walkerId ,
                           d.Name as DogName,
                           ww.Name as WalkerName
                    FROM Walks w
                    JOIN Dog d ON w.dogId = d.Id
                    JOIN Walker ww on w.walkerId = ww.Id
                    
                    ";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Walks> walks = new List<Walks>();
                    while (reader.Read())
                    {
                        Walks walk = new Walks
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                            Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                            DogId = reader.GetInt32(reader.GetOrdinal("Id")),
                            WalkerId = reader.GetInt32(reader.GetOrdinal("walkerId")),
                            Dog = new Dog()
                            {
                                Name = reader.GetString(reader.GetOrdinal("DogName"))
                            },
                            Walker = new Walker()
                            {
                                Name = reader.GetString(reader.GetOrdinal("WalkerName"))
                            }

                        };

                        walks.Add(walk);
                    }

                    reader.Close();

                    return walks;
                }
            }
        }
        public Walks GetWalkByIdDetails(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT w.Id,
                            w.Date,
                            w.Duration,
                            w.dogId,
                            w.walkerId ,
                            d.Name as DogName,
                            ww.Name as WalkerName
                    FROM Walks w
                    JOIN Dog d ON w.dogId = d.Id
                    JOIN Walker ww on w.walkerId = ww.Id
                    WHERE w.Id = @id
                    ";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Walks walk = new Walks
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                            Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                            DogId = reader.GetInt32(reader.GetOrdinal("Id")),
                            WalkerId = reader.GetInt32(reader.GetOrdinal("walkerId")),
                            Dog = new Dog()
                            {
                                Name = reader.GetString(reader.GetOrdinal("DogName"))
                            },
                            Walker = new Walker()
                            {
                                Name = reader.GetString(reader.GetOrdinal("WalkerName"))
                            }

                        };

                        reader.Close();
                        return walk;
                    }
                    else
                    {
                        reader.Close();
                        return null;
                    }
                }
            }
        }

        public List<Walks> GetWalksById(int walkerId)
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    SELECT w.Id,
                           w.Date,
                           w.Duration,
                           w.dogId,
                           d.ownerId as ownerId,
                           o.Name as OwnerName
                    FROM Walks w
                    JOIN Dog d on w.dogId = d.Id
                    JOIN Owner o on d.ownerId = o.Id
                    WHERE w.WalkerId = @walkerId
                    Order By OwnerName
                    ";
                    cmd.Parameters.AddWithValue("@walkerId", walkerId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Walks> walks = new List<Walks>();

                    while (reader.Read())
                    {
                        Walks walk = new Walks()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                            Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                            DogId = reader.GetInt32(reader.GetOrdinal("dogId")),
                            Dog = new Dog()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("dogId")),
                                OwnerId = reader.GetInt32(reader.GetOrdinal("ownerId"))
                            },
                            Owner = new Owner()
                            {
                                Name = reader.GetString(reader.GetOrdinal("OwnerName"))
                            }
                        };
                        walks.Add(walk);
                    }
                    reader.Close();
                    return walks;
                }
               
            }
        }

        public void AddWalk(Walks walk)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    INSERT INTO Walks (
                                       Date,
                                       Duration,
                                       walkerId,
                                       dogId)
                    OUTPUT INSERTED.ID
                    VALUES (@date, @duration, @walkerId, @dogId);
                ";

                    cmd.Parameters.AddWithValue("@date", walk.Date);
                    cmd.Parameters.AddWithValue("@duration", walk.Duration);
                    cmd.Parameters.AddWithValue("@walkerId", walk.WalkerId);
                    cmd.Parameters.AddWithValue("@dogId", walk.DogId);
                    

                    int id = (int)cmd.ExecuteScalar();

                    walk.Id = id;
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ClientManagementServiceRTT.Models;
namespace ClientManagementServiceRTT.Data
{
    public class ClientRepository
    {
        private readonly DatabaseHelper _dbHelper;
        private readonly string _connectionString = "Server=DESKTOP-41RHM0I\\SQLEXPRESS;database=shiraaztest;trusted_connection=true;Encrypt=False;TrustServerCertificate=True;";


        public ClientRepository(string connectionString)
        {
            _dbHelper = new DatabaseHelper(connectionString);

        }

        public int AddClient(string name, string gender, string details)
        {
            string query = "INSERT INTO Client (Name, Gender, Details) VALUES (@Name, @Gender, @Details); SELECT SCOPE_IDENTITY();";
            var parameters = new[]
            {
            new SqlParameter("@Name", name),
            new SqlParameter("@Gender", gender),
            new SqlParameter("@Details", details)
        };

            return Convert.ToInt32(_dbHelper.ExecuteQuery(query, parameters).Rows[0][0]);
        }


        public int AddEmp(Client client)
        {
            // Connection string to your database
            //   string connectionString = "YourConnectionStringHere";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Start a transaction
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Insert the client
                        string insertClientQuery = "INSERT INTO Client (Name, Gender, Details) OUTPUT INSERTED.ClientId VALUES (@Name, @Gender, @Details)";
                        int clientId;

                        using (SqlCommand cmd = new SqlCommand(insertClientQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@Name", client.Name);
                            cmd.Parameters.AddWithValue("@Gender", client.Gender);
                            cmd.Parameters.AddWithValue("@Details", client.Details);

                            // Get the newly inserted ClientId
                            clientId = (int)cmd.ExecuteScalar();
                        }

                        // Insert addresses
                        if (client.Addresses != null)
                        {
                            foreach (var address in client.Addresses)
                            {
                                string insertAddressQuery = "INSERT INTO Address (ClientId, AddressType, AddressLine) VALUES (@ClientId, @AddressType, @AddressLine)";
                                using (SqlCommand cmd = new SqlCommand(insertAddressQuery, connection, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@ClientId", clientId);
                                    cmd.Parameters.AddWithValue("@AddressType", address.AddressType);
                                    cmd.Parameters.AddWithValue("@AddressLine", address.AddressLine);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }

                        // Insert contacts
                        if (client.Contacts != null)
                        {
                            foreach (var contact in client.Contacts)
                            {
                                string insertContactQuery = "INSERT INTO Contact (ClientId, ContactType, ContactValue) VALUES (@ClientId, @ContactType, @ContactValue)";
                                using (SqlCommand cmd = new SqlCommand(insertContactQuery, connection, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@ClientId", clientId);
                                    cmd.Parameters.AddWithValue("@ContactType", contact.ContactType);
                                    cmd.Parameters.AddWithValue("@ContactValue", contact.ContactValue);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }

                        // Commit the transaction
                        transaction.Commit();
                        return clientId; // Return the newly created ClientId
                    }
                    catch (Exception)
                    {
                        // Rollback the transaction if any error occurs
                        transaction.Rollback();
                        throw; // Rethrow the exception for further handling
                    }
                }
            }
        }

        public void AddAddress(int clientId, string addressType, string addressLine)
        {
            string query = "INSERT INTO Addresses (ClientId, AddressType, AddressLine) VALUES (@ClientId, @AddressType, @AddressLine);";
            var parameters = new[]
            {
            new SqlParameter("@ClientId", clientId),
            new SqlParameter("@AddressType", addressType),
            new SqlParameter("@AddressLine", addressLine)
        };

            _dbHelper.ExecuteNonQuery(query, parameters);
        }

        public void AddContact(int clientId, string contactType, string contactValue)
        {
            string query = "INSERT INTO Contacts (ClientId, ContactType, ContactValue) VALUES (@ClientId, @ContactType, @ContactValue);";
            var parameters = new[]
            {
            new SqlParameter("@ClientId", clientId),
            new SqlParameter("@ContactType", contactType),
            new SqlParameter("@ContactValue", contactValue)
        };

            _dbHelper.ExecuteNonQuery(query, parameters);
        }

        // Get Client Method
        public Client GetClient(int clientId)
        {
            Client client = null;
            string query = @"
            SELECT * FROM Client WHERE ClientId = @ClientId;
            SELECT * FROM Address WHERE ClientId = @ClientId;";

            using (var connection = _dbHelper.GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ClientId", clientId);

                    using (var reader = command.ExecuteReader())
                    {
                        // Read client details
                        if (reader.Read())
                        {
                            client = new Client
                            {
                                ClientId = (int)reader["ClientId"],
                                Name = reader["Name"].ToString(),
                                Gender = reader["Gender"].ToString(),
                                Details = reader["Details"].ToString(),
                                Addresses = new List<Address>() // Initialize Addresses
                            };
                        }

                        // Read addresses
                        if (reader.NextResult()) // Move to the next result set (Addresses)
                        {
                            while (reader.Read())
                            {
                                client.Addresses.Add(new Address
                                {
                                    AddressId = (int)reader["AddressId"],
                                    AddressType = reader["AddressType"].ToString(),
                                    AddressLine = reader["AddressLine"].ToString()
                                });
                            }
                        }
                    }
                }
            }

            return client;
        }


        public List<Client> GetAllClients()
        {
            var clients = new List<Client>();

            string query = "SELECT ClientId, Name, Gender, Details FROM Client";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        clients.Add(new Client
                        {
                            ClientId = (int)reader["ClientId"],
                            Name = reader["Name"].ToString(),
                            Gender = reader["Gender"].ToString(),
                            Details = reader["Details"].ToString(),
                            Addresses = new List<Address>() // Initialize empty list for addresses
                        });
                    }
                }
            }

            return clients.ToList();
        }


        public List<Client> GetClientsWithAddresses()
        {
            var clients = new List<Client>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Query to fetch clients with their addresses
                    string query = @"
                        SELECT 
                            c.ClientId, c.Name, c.Gender, 
                            a.AddressType, a.AddressLine
                        FROM 
                            Client c
                        LEFT JOIN 
                            Address a ON c.ClientId = a.ClientId
                        ORDER BY 
                            c.ClientId";

                    using (var command = new SqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        Client currentClient = null;

                        while (reader.Read())
                        {
                            var clientId = reader.GetInt32(reader.GetOrdinal("ClientId"));

                            // Check if we're on a new client
                            if (currentClient == null || currentClient.ClientId != clientId)
                            {
                                // Add the previous client to the list
                                if (currentClient != null)
                                {
                                    clients.Add(currentClient);
                                }

                                // Create a new client object
                                currentClient = new Client
                                {
                                    ClientId = clientId,
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    Gender = reader.GetString(reader.GetOrdinal("Gender")),
                                    Addresses = new List<Address>()
                                };
                            }

                            // Add the address if present
                            if (!reader.IsDBNull(reader.GetOrdinal("AddressType")))
                            {
                                currentClient.Addresses.Add(new Address
                                {
                                    AddressType = reader.GetString(reader.GetOrdinal("AddressType")),
                                    AddressLine = reader.GetString(reader.GetOrdinal("AddressLine"))
                                });
                            }
                        }

                        // Add the last client to the list
                        if (currentClient != null)
                        {
                            clients.Add(currentClient);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
                throw new Exception("Error retrieving clients with addresses", ex);
            }

            return clients;
        }

    }
}

using Amazon.Runtime.Internal.Settings;
using DataAccess.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;

namespace DataAccess.MongoDBAccess
{
    public class ChoreDataAccess
    {
        private const string connectionString = "mongodb://localhost:27017";
        private const string databaseName = "ChoreTracker";
        private const string choreCollection = "Chores_chart";
        private const string usersCollection = "Users";
        private const string choreHistoryCollection = "Chores_history";

        // Method to connect to the MongoDB database and retrieve a collection of documents of type T
        private IMongoCollection<T> ConnectToMongo<T>(in string collection)
        {
            // Instantiate a new MongoDB client with the connection string
            var client = new MongoClient(connectionString);

            // Access the specified database
            var database = client.GetDatabase(databaseName);

            // Retrieve the collection of documents with the specified name
            return database.GetCollection<T>(collection);
        }

        // Method to asynchronously retrieve all user documents from the MongoDB users collection
        public async Task<List<UserModel>> GetAllUser()
        {
            // Connect to the users collection in the MongoDB database
            var usersCollection = ConnectToMongo<UserModel>(ChoreDataAccess.usersCollection);

            // Find all documents in the collection (equivalent to "SELECT * FROM users" in SQL)
            var results = await usersCollection.FindAsync(_ => true);

            // Convert the result cursor to a list of UserModel objects and return it
            return results.ToList();
        }

        // Method to asynchronously retrieve all chore documents assigned to a user with the specified email
        public async Task<List<ChoreModel>> GetAllChores(string email)
        {
            // Connect to the chores collection in the MongoDB database
            var choresCollection = ConnectToMongo<ChoreModel>(choreCollection);

            // Define a filter to find chore documents assigned to the user with the specified email
            var filter = await choresCollection.FindAsync(c => c.AssignedTo.Email == email);

            // Convert the result cursor to a list of ChoreModel objects and return it
            return filter.ToList();
        }

        // Method to asynchronously retrieve a user document by their email
        public async Task<UserModel> GetUserByEmail(string email)
        {
            // Connect to the users collection in the MongoDB database
            var usersCollection = ConnectToMongo<UserModel>(ChoreDataAccess.usersCollection);

            // Find the first user document with the specified email
            var result = await usersCollection.Find(u => u.Email == email).FirstOrDefaultAsync();

            // Return the found user document (or null if not found)
            return result;
        }

        // Method to asynchronously validate a user's password
        public async Task<bool> ValidatePassword(string email, string password)
        {
            // Retrieve the user from the database based on the email
            UserModel user = await GetUserByEmail(email);

            // Check if the user exists and if the provided password matches the stored password
            return user != null && user.Password == password;
        }

        // Method to asynchronously retrieve all chore documents assigned to a specific user
        public async Task<List<ChoreModel>> GetAllChoresForUser(UserModel user)
        {
            // Connect to the chores collection in the MongoDB database
            var choresCollection = ConnectToMongo<ChoreModel>(choreCollection);

            // Find documents in the collection assigned to the specified user
            var results = await choresCollection.FindAsync(c => c.AssignedTo.Id == user.Id);

            // Convert the result cursor to a list of ChoreModel objects and return it
            return results.ToList();
        }

        // Method to asynchronously create a new user document in the MongoDB users collection
        public Task CreateUser(UserModel user)
        {
            // Connect to the users collection in the MongoDB database
            var usersCollection = ConnectToMongo<UserModel>(ChoreDataAccess.usersCollection);

            // Insert the user document into the collection
            return usersCollection.InsertOneAsync(user);
        }

        // Method to asynchronously create a new chore document in the MongoDB chores collection
        public Task CreateChore(ChoreModel chore)
        {
            // Connect to the chores collection in the MongoDB database
            var choresCollection = ConnectToMongo<ChoreModel>(choreCollection);

            // Insert the chore document into the collection
            return choresCollection.InsertOneAsync(chore);
        }

        // Method to asynchronously update an existing chore document in the MongoDB chores collection
        public Task UpdateChore(ChoreModel chore)
        {
            // Connect to the chores collection in the MongoDB database
            var choresCollection = ConnectToMongo<ChoreModel>(choreCollection);

            // Define a filter to find the chore document by its ID
            var filter = Builders<ChoreModel>.Filter.Eq("Id", chore.Id);

            // Replace the existing document with the new chore document
            return choresCollection.ReplaceOneAsync(filter, chore, new ReplaceOptions { IsUpsert = true });
        }

        // Method to asynchronously delete an existing chore document from the MongoDB chores collection
        public Task DeleteChore(ChoreModel chore)
        {
            // Connect to the chores collection in the MongoDB database
            var choresCollection = ConnectToMongo<ChoreModel>(choreCollection);

            // Define a filter to find the chore document by its ID and delete it
            return choresCollection.DeleteOneAsync(c => c.Id == chore.Id);
        }

        // Method to check if a user with the specified email exists in the MongoDB users collection
        public bool UserExists(string email)
        {
            // Connect to the users collection in the MongoDB database
            var userCollection = ConnectToMongo<UserModel>(usersCollection);

            // Define a filter to find a user document by email
            var filter = Builders<UserModel>.Filter.Eq("Email", email);

            // Find the user document(s) that match the filter
            var result = userCollection.Find(filter);

            // Check if any user documents were found
            return result != null;
        }

    }


}

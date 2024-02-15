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

        private IMongoCollection<T> ConnectToMongo<T>(in string collection)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            return database.GetCollection<T>(collection);
        }

        public async Task<List<UserModel>> GetAllUser()
        {
            var usersCollection = ConnectToMongo<UserModel>(ChoreDataAccess.usersCollection);
            var results = await usersCollection.FindAsync(_ => true);
            return results.ToList();
        }

        public async Task<List<ChoreModel>> GetAllChores()
        {
            var choresCollection = ConnectToMongo<ChoreModel>(choreCollection);
            var results = await choresCollection.FindAsync(_ => true);
            return results.ToList();
        }

        public async Task<UserModel> GetUserByEmail(string email)
        {
            var usersCollection = ConnectToMongo<UserModel>(ChoreDataAccess.usersCollection);
            var result = await usersCollection.Find(u => u.Email == email).FirstOrDefaultAsync();
            return result;
        }

        public async Task<bool> ValidatePassword(string email, string password)
        {
            // Retrieve user from the database based on the email
            UserModel user = await GetUserByEmail(email);
            // Check if user exists and if the provided password matches the stored password
            return user != null && user.Password == password;
        }

        public async Task<List<ChoreModel>> GetAllChoresForUser(UserModel user)
        {
            var choresCollection = ConnectToMongo<ChoreModel>(choreCollection);
            var results = await choresCollection.FindAsync(c => c.AssignedTo.Id == user.Id);
            return results.ToList();
        }

        public Task CreateUser(UserModel user)
        {
            var usersCollection = ConnectToMongo<UserModel>(ChoreDataAccess.usersCollection);
            return usersCollection.InsertOneAsync(user);
        }

        public Task CreateChore(ChoreModel chore)
        {
            var choresCollection = ConnectToMongo<ChoreModel>(choreCollection);
            return choresCollection.InsertOneAsync(chore);
        }

        public Task UpdateChore(ChoreModel chore)
        {
            var choresCollection = ConnectToMongo<ChoreModel>(choreCollection);
            var filter = Builders<ChoreModel>.Filter.Eq("Id", chore.Id);
            return choresCollection.ReplaceOneAsync(filter, chore, new ReplaceOptions { IsUpsert = true });
        }

        public Task DeleteChore(ChoreModel chore)
        {
            var choresCollection = ConnectToMongo<ChoreModel>(choreCollection);
            return choresCollection.DeleteOneAsync(c => c.Id == chore.Id);
        }

        //This method checks if the user exists in the database 
        public bool UserExists(string email)
        {
            var userCollection = ConnectToMongo<UserModel>(usersCollection);
            var filter = Builders<UserModel>.Filter.Eq("Email", email); // Adjust the filter type to UserModel
            var result = userCollection.Find(filter);
            return result != null;
        }

        //This method helps with the caching of the user details to make assigning of chores easier
        private Dictionary<string, UserModel> _cache = new Dictionary<string, UserModel>();

        public async Task<UserModel> GetCachedUser(string email)
        {
            // Check if user is already in cache
            if (_cache.ContainsKey(email))
            {
                return _cache[email];
            }

            // If not in cache, retrieve from the database
            UserModel user = await GetUserByEmail(email);

            // Add user to cache
            if (user != null)
            {
                _cache[email] = user;
            }

            return user;
        }


    }


}

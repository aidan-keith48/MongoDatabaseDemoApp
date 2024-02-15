using DataAccess.Model;
using DataAccess.MongoDBAccess;
using MongoDatabaseDemoApp.Model;
using MongoDB.Driver;

namespace MongoDatabaseDemoApp.HelperFunctions
{
    public class functions
    {
        private Dictionary<string, UserModel> _cache = new Dictionary<string, UserModel>();


        Validation validation = new Validation();

        ChoreDataAccess choreDataAccess = new ChoreDataAccess();
        UserModel user = new UserModel();
        ChoreModel chores = new ChoreModel();
        ChoreHistory choreHistory = new ChoreHistory();

        public List<Employee> employees = new List<Employee>();

        //This function will handle the start screen of the console app
        public async Task StartScreen()
        {
            Console.WriteLine("Welcome to the Chore Tracker Console App");
            bool busy = true;

            while (busy)
            {
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Register");
                Console.WriteLine("3. Exit");

                Console.Write("Enter your choice (1-3): ");
                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "1":
                        LoginComponent();
                        busy = false;
                        break;
                    case "2":
                        RegisterComponet();
                        break;
                    case "3":
                        Console.WriteLine("Exiting Program. Goodbye!");
                        System.Environment.Exit(0);
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please enter a number between 1 and 3.");
                        break;
                }
            }

        }

        //This function will be a component for the login feature of the console app
        public async Task LoginComponent()
        {
            while (true)
            {
                Console.WriteLine("\nEnter your username: ");
                string username = validation.GetValidatedInput();
                Console.WriteLine("\nEnter your password: ");
                string password = validation.GetValidatedInput();

                bool isValid = choreDataAccess.ValidatePassword(username, password).Result;

                if (isValid)
                {
                    Console.WriteLine("\nLogin successful! Welcome back, " + username + "!");
                    _cache[username] = await choreDataAccess.GetUserByEmail(username);
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid username or password. Please try again.");
                }
            }
        }



        //This function will be a component for the register feature of the console app
        public void RegisterComponet()
        {
            Console.WriteLine("Welcome To The Register Page\n");

            string name = validation.GetValidatedInput();
            string surname = validation.GetValidatedInput();
            string email = validation.GetValidatedEmail();
            string password = validation.GetValidatedPassword();

            UserModel user = new UserModel
            {
                Name = name,
                Surname = surname,
                Email = email,
                Password = password
            };

            if (!choreDataAccess.UserExists(email))
            {
                try
                {
                    choreDataAccess.CreateUser(user);
                    Console.WriteLine("\nUser Registered Successfully");
                }
                catch (MongoWriteException)
                {
                    Console.WriteLine("User registration failed. Please try again later.");
                }
            }
            else
            {
                Console.WriteLine("User with this email already exists.");
            }
        }

        //This function will be a component for the CRUD operations of the console app
        public async Task DisplayMenu()
        {
            while (true)
            {
                Console.WriteLine("MongoDB Operations Menu:");
                Console.WriteLine("1. Create Chore");
                Console.WriteLine("2. Read");
                Console.WriteLine("3. Update");
                Console.WriteLine("4. Delete");
                Console.WriteLine("5. Exit");

                Console.Write("Enter your choice (1-5): ");
                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "1":
                        // Call your create method here
                        //Note : Create a method that grabs the user details of the a user who is logged in to eliemante the need to enter the email again.
                        await captureChores();

                        break;
                    case "2":
                        // Call your read method here
                        Console.WriteLine("\nPerform Read Operation");
                        break;
                    case "3":
                        // Call your update method here
                        Console.WriteLine("\nPerform Update Operation");
                        break;
                    case "4":
                        // Call your delete method here
                        Console.WriteLine("\nPerform Delete Operation");
                        break;
                    case "5":
                        Console.WriteLine("\nExiting Program. Goodbye!");
                        return;
                    default:
                        Console.WriteLine("\nInvalid choice. Please enter a number between 1 and 5.");
                        break;
                }

                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
                Console.Clear(); // Clear the console for a cleaner display
            }
        }

        //This Method will capture the chores
        public async Task captureChores()
        {
            var user = await choreDataAccess.GetAllUser();

            Console.WriteLine("\nPerform Create Operation");

            Console.WriteLine("Enter Chore Name: ");
            string choreName = validation.GetValidatedInput();

            Console.WriteLine("Enter Chore Frequency in days: ");
            int choreFrequency = validation.GetValidatedNumber();

            await displayPerson();

            Console.WriteLine($"{choreName} is Assigned To:");
            string choreAssignedTo = validation.GetValidatedInput();

            Console.WriteLine("Enter Chore Last Completed: ");
            DateTime choreLastCompleted = validation.GetValidatedDate();

            ChoreModel chores = new ChoreModel()
            {
                AssignedTo = user.FirstOrDefault(x => x.Email == choreAssignedTo),
                ChoreText = choreName,
                FreqencyOfChores = choreFrequency,
                LastCompleted = choreLastCompleted
            };

            await choreDataAccess.CreateChore(chores);
        }

        //This Method will display the user details
        public async Task displayPerson()
        {
            var results = await choreDataAccess.GetAllUser();

            foreach (var result in results.ToList())
            {                
                Console.WriteLine($"\nUse this field to Assign Chore to user: {result.Email}\nMore Details: {result.fullName}\n");                
            }
        }

        
    }
}


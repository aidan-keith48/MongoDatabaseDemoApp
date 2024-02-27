using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDatabaseDemoApp.HelperFunctions
{
    public class Validation
    {
        public string ValidateName(string name)
        {
           while(string.IsNullOrEmpty(name))
            {
                Console.WriteLine("Field cannot be empty. Please enter a valid name: ");
                name = Console.ReadLine();
            }
           return name;
        }

        public int ValidateNumber(string number)
        {
            int num;
            while (!int.TryParse(number, out num))
            {
                Console.WriteLine("Invalid input. Please enter a valid number: ");
                number = Console.ReadLine();
            }
            return num;
        }

        public bool ValidatePassword(string password)
        {
            if (password.Length < 8)
            {
                Console.WriteLine("Password must be at least 8 characters long.");
                return false;
            }            

            if (!password.Any(char.IsDigit))
            {
                Console.WriteLine("Password must contain at least one digit.");
                return false;
            }

            if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                Console.WriteLine("Password must contain at least one special character.");
                return false;
            }

            return true;
        }

        public bool ValidateEmail(string email)
        {
           if(string.IsNullOrEmpty(email))
            {
                Console.WriteLine("Email cannot be empty. Please enter a valid email: ");
                return false;
            }

           if(!email.Contains("@"))
            {
                Console.WriteLine("Email must contain @ symbol.");
                return false;
            }
           return true;
        }    
        
        public bool ValidateDate(string date)
        {
            if (string.IsNullOrEmpty(date))
            {
                Console.WriteLine("Date cannot be empty. Please enter a valid date: ");
                return false;
            }

            if (!DateTime.TryParse(date, out DateTime result))
            {
                Console.WriteLine("Invalid date format. Please enter a valid date: ");
                return false;
            }

            return true;
        }

        //Validations Results for the user input
        public string GetValidatedName()
        {
            Console.WriteLine("\nEnter Your Name: ");
            string input = Console.ReadLine();
            // Validate input as needed
            return ValidateName(input);
        }

        public string GetValidatedSurname()
        {
            Console.WriteLine("\nEnter Your Surname: ");
            string input = Console.ReadLine();
            // Validate input as needed
            return ValidateName(input);
        }

        public string GetValidatedLoginUsername()
        {
            Console.WriteLine("\nEnter Your Email: ");
            string input = Console.ReadLine();
            // Validate input as needed
            return ValidateName(input);
        }

        public string GetValidatedLoginPassword()
        {
            Console.WriteLine("\nEnter Your Password: ");
            string input = Console.ReadLine();
            // Validate input as needed
            return ValidateName(input);
        }

        public int GetValidatedNumber()
        {
            Console.WriteLine("\nEnter Chore Frequency in days: ");
            string number = Console.ReadLine();
            // Validate number as needed
            return ValidateNumber(number);
        }

        public string GetValidatedEmail()
        {
            Console.WriteLine("\nEnter Your Email: ");
            string email = Console.ReadLine();
            while (!ValidateEmail(email))
            {
                Console.WriteLine("\nInvalid email. Please enter a valid email:");
                email = Console.ReadLine();
            }
            return email;
        }

        public string GetValidatedPassword()
        {
            Console.WriteLine("\nEnter Your Password: ");
            string password = Console.ReadLine();
            while (!ValidatePassword(password))
            {
                Console.WriteLine("\nInvalid password. Please enter a valid password:");
                password = Console.ReadLine();
            }
            return password;
        }

        public DateTime GetValidatedDate()
        {
            Console.WriteLine("\nEnter Chore Last Completed in format(MM/DD/YYYY): ");

            string date = Console.ReadLine();
            while (!ValidateDate(date))
            {
                Console.WriteLine("\nInvalid date. Please enter a valid date:");
                date = Console.ReadLine();
            }
            return Convert.ToDateTime(date);
        }

        public string GetValidatedChoreName()
        {
            Console.WriteLine("Enter The Chore Name/Task: ");
            string input = Console.ReadLine();
            // Validate input as needed
            return ValidateName(input);
        }

        public string GetValidatedAssignedUser(string chore)
        {
            Console.WriteLine($"{chore} is Assigned to:");
            string input = Console.ReadLine();
            // Validate input as needed
            return ValidateName(input);
        }
    }
}

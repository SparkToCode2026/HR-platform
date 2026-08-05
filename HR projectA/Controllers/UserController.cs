using Microsoft.AspNetCore.Mvc;
using ProjectX.Models;

namespace ProjectX.Controllers
{
    public class UserController
    {
        ProjectContext context = new ProjectContext();
        public void AddUser()
        {
            // Logic to create a new user
            User U=new User();
            Console.WriteLine("Enter your username:");
            U.Username = Console.ReadLine();
            Console.WriteLine("Enter User ID:");
            U.UserId = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter your email:");
            U.Email = Console.ReadLine();
            Console.WriteLine("Enter your password:");
            U.Password = Console.ReadLine();
            Console.WriteLine("Enter your role (Admin/Candidate/Employer):");
            U.Role = Console.ReadLine();

            context.Users.Add(U);
            context.SaveChanges();

        }


    }
}

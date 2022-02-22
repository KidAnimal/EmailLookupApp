using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace EmailLookupApp
{
    public class Program
    {
        private readonly ILogger<Program> logger;
        private readonly EmailService emailValidation;

        static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            string filePath = GetUserFileName();
            host.Services.GetRequiredService<EmailService>().Run(filePath);
        }

        public Program(ILogger<Program> logger, EmailService emailValidation)
        {
            this.logger = logger;
            this.emailValidation = emailValidation;
        }
       
        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddTransient<Program>();
                    services.AddTransient<EmailService>();
                });
        }

        private static string GetUserFileName()
        {
            bool fileInvalid = true;
            string filePath; 
            // Prompt For User Input && Check if the File Exists
            // If it Doesn't Exist Loop
            do
            {
                Console.WriteLine("Enter CSV File Path :");
                filePath = Console.ReadLine();
                if (File.Exists(filePath))
                {
                    fileInvalid = false;
                    return filePath;
                }
                else
                {
                    Console.WriteLine("INVALID FILE PATH...");
                }
            }
            while (fileInvalid);
            return filePath;
        }
     
    }
}

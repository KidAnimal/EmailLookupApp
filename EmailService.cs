using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace EmailLookupApp
{
    public class EmailService
    {
        public static List<UserList> InitEmailApplication(string filePath)
        {
            // Opens the valid file path and parse the data
            StreamReader reader = null;
            List<UserList> validList = new List<UserList>();
            List<UserList> invalidList = new List<UserList>();

            reader = new StreamReader(File.OpenRead(filePath));
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                // If valid adds it to an valid list
                if (IsValidEmail(values[2]))
                {
                    validList.Add(new()
                    {
                        firstName = values[0],
                        lastName = values[1],
                        email = values[2]
                    });
                    Console.WriteLine("VALID: " + values[2]);
                }
                // If invalid adds it to an invalid list     
                else
                {
                    invalidList.Add(new()
                    {
                        firstName = values[0],
                        lastName = values[1],
                        email = values[2]
                    });
                    Console.WriteLine("INVALID: " + values[2]);
                }
            }
            return validList;
        }

        internal void Run(string filePath)
        {
            InitEmailApplication(filePath);
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Uses Regex to Check if email is valid
            try
            {
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                string DomainMapper(Match match)
                {
                    var idn = new IdnMapping();
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}

using Alachisoft.NCache.Samples.Dapper.ConsoleUI.SeedData;
using System;

namespace Alachisoft.NCache.Samples.Dapper.ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            
            try
            {
                Console.WriteLine("Creating table dbo.Customers and seeding data");
                CustomerSeed.Initialize();
                DapperDemo.Run();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }
    }
}

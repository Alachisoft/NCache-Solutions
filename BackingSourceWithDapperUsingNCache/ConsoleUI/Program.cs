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

                // Print the full exception (incl. inner exceptions) so failures such as
                // cache connection errors inside the DapperDemo type initializer are visible.
                Console.WriteLine(ex);
            }
        }
    }
}

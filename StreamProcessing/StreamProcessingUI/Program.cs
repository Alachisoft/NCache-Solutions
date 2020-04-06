using Alachisoft.StreamProcessingUI;
using System;

namespace StreamProcessing
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                MainApp instance = new MainApp();
                instance.Run();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}

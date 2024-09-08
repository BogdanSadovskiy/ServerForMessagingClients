using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerForMessagingClients
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SetConsoleSize(35, 20);
            ServerService serverService = new ServerService();
            Console.ReadLine();
        }
        static private void SetConsoleSize(int width, int height)
        {
            try
            {
                Console.SetWindowSize(width, height);
                Console.SetBufferSize(width, height);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error setting console size: " + ex.Message);
            }
        }
    }
}

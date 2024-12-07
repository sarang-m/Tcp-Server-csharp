// See https://aka.ms/new-console-template for more information
using System;
using TcpServerApp.Network;  // Add this if TcpServer is in a different namespace

namespace TcpServerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a server instance and start it
            TcpServer server = new TcpServer(4221);
            server.Start();

            // Wait for the user to press Enter to stop the server
            Console.WriteLine("Press Enter to stop the server.");
            Console.ReadLine();

            // Stop the server
            server.Stop();
        }
    }
}
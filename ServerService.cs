using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ServerForMessagingClients
{
    public class ServerService
    {

        TcpListener server;
        List<Client> clients;
        List<ConsoleColor> colors;
        static ConsoleColor systemColor = ConsoleColor.Red;
        Random random;

        public ServerService()
        {
            colors = new List<ConsoleColor>();
            ColorsAvoid();
            random = new Random();
            server = new TcpListener(IPAddress.Any, 25565);
            clients = new List<Client>();
            ServerWork();
        }
        private void ColorsAvoid()
        {
            colors.Add(ConsoleColor.Red); //system color
            colors.Add(ConsoleColor.Black);
            colors.Add(ConsoleColor.White);
        }
        private void ServerWork()
        {

            StartServer(server);
            WaitingForClients();
            StopServer(server);
  
        }
        static public void SystemMessage(string message, ConsoleColor foreColor)
        {
            Console.ForegroundColor = foreColor;
            Console.Write(message);
            Console.ResetColor();
        }
        public void StartServer(TcpListener serverListener)
        {
            if (serverListener != null) serverListener.Start();
            SystemMessage("Server Started\n", systemColor);
        }
        public void StopServer(TcpListener serverListener)
        {
            if (serverListener != null) serverListener.Stop();
            SystemMessage("Server Stopped\n", systemColor);
        }

        private void WaitingForClients()
        {
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                Client newClient;
                lock (clients)
                {
                    newClient = new Client(client, clients, colors, random);
                    clients.Add(newClient);
                }
                

                Thread clientThread = new Thread(newClient.HandleClient);
                clientThread.Start();
            }
        }


    }
}

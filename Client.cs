using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace ServerForMessagingClients
{
    public class Client
    {
        TcpClient client;
        ConsoleColor color;
        ConsoleColor systemColor;
        List<Client> clients;

        public Client(TcpClient client, List<Client> clients, List<ConsoleColor> colors, Random random)
        {
            this.client = client;
            this.color = SetColor(colors, random);
            this.clients = clients;
            this.systemColor = colors.First();
            ServerService.SystemMessage("Client ", systemColor);
            ServerService.SystemMessage($"{color} ", color);
            ServerService.SystemMessage("connected\n", systemColor);
            SendMessageToOther($"{color} connected");
        }
        private ConsoleColor SetColor(List<ConsoleColor> colors, Random random)
        {
            ConsoleColor newColor;
            do
            {
                newColor = (ConsoleColor)random.Next(Enum.GetValues(typeof(ConsoleColor)).Length);
            }
            while (colors.Contains(newColor));

            colors.Add(newColor);
            return newColor;
        }

        public void HandleClient()
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[256];
            int bytesRead;

            try
            {
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    string formattedMessage = $"\'{color}\': {message}\n";
                    ServerService.SystemMessage(formattedMessage, color);
                    SendMessageToOther(formattedMessage.Substring(0, formattedMessage.Length-1));
                }
            }
            catch (Exception ex)
            {
                ServerService.SystemMessage("Client ", systemColor);
                ServerService.SystemMessage($"{color} ", color);
                ServerService.SystemMessage("disconnected\n", systemColor);
            }
            finally
            {
                client.Close();
                lock (clients)
                {
                    clients.Remove(this); 
                }
               
                SendMessageToOther($"{color} disconnected");

            }
        }

        
        private void SendMessageToOther(string message)
        {
            lock (clients)
            {
                foreach (Client c in clients)
                {
                    if (c != this) // dont message yourself
                    {
                        NetworkStream stream = c.client.GetStream();
                        byte[] response = Encoding.ASCII.GetBytes(message);
                        stream.Write(response, 0, response.Length);
                    }
                }
            }
        }
    }
}


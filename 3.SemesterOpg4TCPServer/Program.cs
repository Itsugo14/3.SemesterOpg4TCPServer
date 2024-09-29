using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

class Server
{
    static void Main()
    {
        TcpListener listener = new TcpListener(IPAddress.Any, 7);
        listener.Start();
        Console.WriteLine("Server started...");

        while (true)
        {
            TcpClient client = listener.AcceptTcpClient();
            Console.WriteLine("Client connected.");

            Task.Run(() => HandleClient(client));
        }
    }

    static void HandleClient(TcpClient client)
    {
        using (NetworkStream stream = client.GetStream())
        using (StreamReader reader = new StreamReader(stream))
        using (StreamWriter writer = new StreamWriter(stream) { AutoFlush = true })
        {
            writer.WriteLine("Welcome! You can use the following commands:");
            writer.WriteLine("1. Random - Generate a random number between two numbers.");
            writer.WriteLine("2. Add - Add two numbers.");
            writer.WriteLine("3. Subtract - Subtract the second number from the first.");
            writer.WriteLine("4. exit - Close the connection.");
            writer.WriteLine("Please enter your command:");

            while (true)
            {
                string command = reader.ReadLine();

                if (string.IsNullOrEmpty(command))
                {
                    break;
                }

                if (command.ToLower() == "exit")
                {
                    writer.WriteLine("You have typed 'exit' and the connection has closed!");
                    break;
                }

                Console.WriteLine("Received command: " + command);

                writer.WriteLine("Enter two numbers:");

                string numbersInput = reader.ReadLine();
                string[] numberParts = numbersInput.Split(' ');

                if (numberParts.Length == 2 && int.TryParse(numberParts[0], out int num1) && int.TryParse(numberParts[1], out int num2))
                {
                    switch (command)
                    {
                        case "Random":
                            Random rand = new Random();
                            int randomValue = rand.Next(num1, num2 + 1);
                            writer.WriteLine("Random number: " + randomValue);
                            break;

                        case "Add":
                            int sum = num1 + num2;
                            writer.WriteLine("Result: " + sum);
                            break;

                        case "Subtract":
                            int difference = num1 - num2;
                            writer.WriteLine("Result: " + difference);
                            break;

                        default:
                            writer.WriteLine("Unknown command. Use Random, Add, Subtract, or exit.");
                            break;
                    }
                }
                else
                {
                    writer.WriteLine("Invalid input. Please provide two valid numbers.");
                }
            }

            client.Close();
            Console.WriteLine("Client disconnected.");
        }
    }
}




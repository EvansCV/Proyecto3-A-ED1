using System.Net.Sockets;
using System.Text;

namespace Proyecto3
{
    public class Client
    {
        private readonly string serverIp = "127.0.0.1";
        private readonly int port = 5000;

        public void Start()
        {
            try
            {
                using TcpClient client = new TcpClient(serverIp, port);
                using NetworkStream stream = client.GetStream();
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

                Console.WriteLine("Conectado al servidor. Escriba una expresión matemática (o 'salir' para terminar):");

                while (true)
                {
                    string? input = Console.ReadLine();
                    if (input?.ToLower() == "salir") break;

                    writer.WriteLine(input);
                    string response = reader.ReadLine();
                    Console.WriteLine($"Respuesta del servidor: {response}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
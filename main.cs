using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Proyecto3;

// Este código implementa el uso de la biblioteca Collection.Generics para el uso de pilas, colas,
// entre otras estructuras de datos que se requieren de forma genérica y su implementación es más
// sencilla de esta manera.
class Program
{   

    static void Main(string[] args)
    {
        // Iniciar el servidor automáticamente en un hilo separado
        Thread serverThread = new Thread(StartServer);
        serverThread.IsBackground = true;
        serverThread.Start();
        Console.WriteLine("Servidor iniciado automáticamente en segundo plano.");

        // Mostrar el menú al usuario
        while (true)
        {
            Console.WriteLine("\nSeleccione una opción:");
            Console.WriteLine("1. Iniciar Cliente");
            Console.WriteLine("2. Evaluar expresión localmente");
            Console.WriteLine("3. Salir");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    // Se inicia el cliente creando su instancia y llamando al método Start
                    Console.WriteLine("Iniciando cliente...");
                    Client client = new Client();
                    client.Start();
                    //CalculatorForm cal = new CalculatorForm(client);
                    // Se crea una nueva gui, donde la cual requiere del cliente para interactuar con el mismo.
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new CalculatorForm(client));
                    break;

                case "2":
                    Console.WriteLine("Ingrese la expresión en notación infija:");
                    string infix = Console.ReadLine() ?? string.Empty;

                    if (string.IsNullOrWhiteSpace(infix))
                    {
                        Console.WriteLine("Expresión inválida.");
                        break;
                    }

                    string logicalOperatorsPattern = @"[&|~^]";
                    string postfix = ConvertToPostfix(infix);
                    Console.WriteLine($"Postfija: {postfix}");

                    ExpressionTree tree = new ExpressionTree(postfix);

                    if (Regex.IsMatch(infix, logicalOperatorsPattern))
                    {
                        Console.WriteLine("Evaluando expresión lógica...");
                        double result = tree.Evaluate();
                        Console.WriteLine($"Resultado lógico: {result == 1}");
                    }
                    else
                    {
                        Console.WriteLine("Evaluando expresión matemática...");
                        double result = tree.Evaluate();
                        Console.WriteLine($"Resultado matemático: {result}");
                    }
                    break;

                case "3":
                    Console.WriteLine("Saliendo...");
                    return;

                default:
                    Console.WriteLine("Opción inválida. Intente de nuevo.");
                    break;
            }
        }
    }

    private static void StartServer()
    {
        Server server = new Server();
        server.Start();
    }

    public static string ConvertToPostfix(string infix)
    {
        string[] tokens = infix.Split(" ");
        Stack<string> operators = new Stack<string>();
        List<string> output = new List<string>();

        foreach (string token in tokens)
        {
            if (double.TryParse(token, out _))
            {
                output.Add(token);
            }
            else if (token == "(")
            {
                operators.Push(token);
            }
            else if (token == ")")
            {
                while (operators.Peek() != "(")
                {
                    output.Add(operators.Pop());
                }
                operators.Pop();
            }
            else
            {
                while (operators.Count > 0 && operators.Peek() != "(" &&
                       GetPrecedence(operators.Peek()) >= GetPrecedence(token) &&
                       !IsRightAssociative(token))
                {
                    output.Add(operators.Pop());
                }
                operators.Push(token);
            }
        }

        while (operators.Count > 0)
        {
            output.Add(operators.Pop());
        }

        return string.Join(" ", output);
    }

    private static int GetPrecedence(string op)
    {
        return op switch
        {
            "+" or "-" => 1,
            "*" or "/" => 2,
            "%" => 2,
            "**" => 3,
            "&" => 1,
            "|" => 1,
            "^" => 2,
            "~" => 3,
            _ => 0
        };
    }

    private static bool IsRightAssociative(string op)
    {
        return op == "**";
    }
}





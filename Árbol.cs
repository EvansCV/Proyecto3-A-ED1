using System.Text.RegularExpressions;
namespace Proyecto3
{
    // Este código implementa el uso de la biblioteca Collection.Generics para el uso de pilas, colas,
    // entre otras estructuras de datos que se requieren de forma genérica y su implementación es más
    // sencilla de esta manera.
    class Program
    {
        static void Main()
        {
            // Esta entrada no puede tener nada pegado, a lo mucho valores el menos para representar
            // números negativos, sino no funciona. Esto se debe arreglar ya sea propiamente aquí en el backend
            // o modificar la interfaz gráfica. 
            string infix = "3 + ( 2 * -4 )";
            // Patrón para evaluarlo como expresión lógica o matemática.
            string logicalOperatorsPattern = @"[&|~^]";

            // Convertir de infija a postfija
            string postfix = ConvertToPostfix(infix);
            Console.WriteLine($"Postfija: {postfix}"); 

            // Crear y evaluar el árbol de expresión
            ExpressionTree tree = new ExpressionTree(postfix);

            if (Regex.IsMatch(infix, logicalOperatorsPattern))
            {
                Console.WriteLine("Evaluando expresión lógica...");
                double result = tree.Evaluate(); // Evaluar como lógica
                Console.WriteLine($"Resultado lógico: {result == 1}");
            }
            else
            {
                Console.WriteLine("Evaluando expresión matemática...");
                double result = tree.Evaluate(); // Evaluar como matemática
                Console.WriteLine($"Resultado matemático: {result}");
            }
        }

        // Función para obtener la precedencia de  un operador.
        private static int GetPrecedence(string op)
        {
            return op switch
            {
                "+" or "-" => 1,
                "*" or "/" => 2,
                "%" => 2,   // Similar a multiplicación/división
                "**" => 3,  // Mayor precedencia
                
                // Operadores lógicos
                "&" => 1, // AND y OR son similares en precedencia a la suma y la resta.
                "|" => 1, 
                "^" => 2, // El XOR es un doble implica negado, así que tiene mayor precedencia.
                "~" => 3, // El NOT posee incluso más precedecia que todos los anteriores.
                _ => 0
            };
        }


        // Función para saber si una operación es asociativa hacia la derecha. Esto solo aplica para el caso de la potencia.
        private static bool IsRightAssociative(string op)
        {
            return op == "**"; // La potencia es asociativa a la derecha
        }

        static string ConvertToPostfix(string infix)
        {
            string[] tokens = infix.Split(" ");
            Stack<string> operators = new Stack<string>();
            List<string> output = new List<string>();

            foreach (string token in tokens)
            {
                if (double.TryParse(token, out _)) // Si es un número
                {
                    output.Add(token);
                }
                else if (token == "(") // Paréntesis de apertura
                {
                    operators.Push(token);
                }
                else if (token == ")") // Paréntesis de cierre
                {
                    while (operators.Peek() != "(")
                    {
                        output.Add(operators.Pop());
                    }
                    operators.Pop(); // Eliminar "(" de la pila
                }
                else // Es un operador
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

            // Vaciar operadores restantes
            while (operators.Count > 0)
            {
                output.Add(operators.Pop());
            }

            return string.Join(" ", output);
        }
    }

    
}




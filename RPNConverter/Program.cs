using System;

namespace RPNConverter
{
    // TODO: Consider adding support for more operators like ^ and % in future versions
    // NOTE: This was quite tricky to get the parentheses right - spent way too much time on edge cases!
    
    class Program
    {
        // Using dependency injection pattern here - makes testing easier later
        private static readonly IRPNParser _parser = new RPNParser();
        
        static void Main(string[] args)
        {
            // Welcome banner - kept it simple but professional
            Console.WriteLine("=== SARS RPN Calculator ===");
            Console.WriteLine("Built for the Customs Risk Engine");
            Console.WriteLine();
            Console.WriteLine("Converts Reverse Polish Notation to readable math expressions");
            Console.WriteLine("Supports: +, -, *, / with integers and decimals");
            Console.WriteLine();
            
            // Handle command line usage first - useful for scripting
            if (args.Length > 0)
            {
                string expression = string.Join(" ", args);
                ProcessExpression(expression);
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
                return;
            }
            
            // Interactive mode - this is where most users will spend their time
            ShowExamples();
            RunInteractiveMode();
        }
        
        // Show some example RPN expressions to help users get started
        // TODO: Maybe add more complex examples later?
        private static void ShowExamples()
        {
            Console.WriteLine("Try these examples:");
            Console.WriteLine("  3 4 +                    (simple addition)");
            Console.WriteLine("  3 4 + 2 *                (order of operations)");
            Console.WriteLine("  15 7 1 1 + - / 3 * 2 1 1 + + -    (complex expression)");
            Console.WriteLine();
            Console.WriteLine("Type 'help' for more info, 'quit' to exit");
            Console.WriteLine();
        }
        
        private static void RunInteractiveMode()
        {
            while (true)
            {
                Console.Write("RPN> ");
                string input = Console.ReadLine();
                
                // Handle empty input gracefully
                if (string.IsNullOrWhiteSpace(input))
                    continue;
                
                string command = input.Trim().ToLower();
                
                if (command == "quit" || command == "exit")
                {
                    Console.WriteLine("Thanks for using the RPN Calculator!");
                    break;
                }
                
                if (command == "help")
                {
                    ShowHelp();
                    continue;
                }
                
                if (command == "clear")
                {
                    Console.Clear();
                    ShowExamples();
                    continue;
                }
                
                ProcessExpression(input);
                Console.WriteLine();
            }
        }
        
        private static void ShowHelp()
        {
            Console.WriteLine();
            Console.WriteLine("=== Help ===");
            Console.WriteLine("RPN (Reverse Polish Notation) puts operators after operands:");
            Console.WriteLine("  Instead of: 3 + 4");
            Console.WriteLine("  Write: 3 4 +");
            Console.WriteLine();
            Console.WriteLine("Commands:");
            Console.WriteLine("  help  - Show this help");
            Console.WriteLine("  clear - Clear screen");
            Console.WriteLine("  quit  - Exit program");
            Console.WriteLine();
        }
        
        private static void ProcessExpression(string expression)
        {
            try
            {
                Console.WriteLine($"Input: {expression}");
                
                // Quick validation first - saves time on obviously wrong input
                if (!_parser.IsValidRPN(expression))
                {
                    Console.WriteLine("Hmm, that doesn't look like valid RPN. Check your syntax!");
                    Console.WriteLine("Remember: operands first, then operators (e.g., '3 4 +' not '3 + 4')");
                    return;
                }
                
                // Convert to readable math notation
                string infixResult = _parser.ConvertToInfix(expression);
                Console.WriteLine($"Math:  {infixResult}");
                
                // Calculate the actual result
                try
                {
                    double result = _parser.Evaluate(expression);
                    // Format nicely - no unnecessary decimals for whole numbers
                    if (result == Math.Floor(result))
                        Console.WriteLine($"= {result:F0}");
                    else
                        Console.WriteLine($"= {result:G}");
                }
                catch (DivideByZeroException)
                {
                    Console.WriteLine("= Error: Can't divide by zero!");
                }
                catch (Exception evalEx)
                {
                    Console.WriteLine($"= Error: {evalEx.Message}");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Oops: {ex.Message}");
            }
            catch (Exception ex)
            {
                // This shouldn't happen, but just in case...
                Console.WriteLine($"Something went wrong: {ex.Message}");
                Console.WriteLine("Please report this if it keeps happening!");
            }
        }
    }
}
using System;
using static ConsoleCalculator.Utilities;
using static System.Console;
using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace ConsoleCalculator
{
    public class Program
    {
        public static Double Calculate(String userInput)
        {
            // Main Calculate function
            //utilizes methods from ConsoleCalculator.Utilities class
            Double result = 0;
            String processedString = "";// temporary variable to store processed input
            List<String> infixTokens = new List<string>();//temporary variable for tokens ordered in infix notation
            Queue<String> postfixTokens = new Queue<string>();//temporary variable for tokens ordered in postifix notation
            
            //Note temporary variables introduced to aid readability, alternatively the code could be reduced to:
            // result = PostfixCalculator(ConvertToPostfix(DetermineOperators(ConvertToTokens(ReduceOperators(RemoveWhitespaces(userInput))))));

            processedString = RemoveWhitespaces(userInput); 
            processedString = ReduceOperators(processedString);// reduces potential -- and +- no white spaces accepted

            // converts operators from symbols to Strings such as Addition, Subtraction, Multiplication, Division and Negation
            //necessary to discriminate between negation and subtraction. New comment: ConvertToTokens() converts operators to operator symbols and DetermineOperators() discirimantes between negation and subtraction and assign descriptive string instead of operator symbol
            infixTokens = DetermineOperators(ConvertToTokens(processedString));

            //converts string-based tokens from infix to posfix notation
            postfixTokens = ConvertToPostfix(infixTokens);

            //calculate the result using the postfix calculator
            result = PostfixCalculator(postfixTokens);


            return result;
        }
        static void Main(string[] args)
        {
            String userInput; //String provided by the user
            Double result = 0;//result of the calculation

            Regex quitKey = new Regex("Q|q"); // regular expression to check if a quit key is pressed
            Boolean wasQuitPressed = false; //flag which stores if Quit key was pressed

            DisplayWelcomeMsg();

            while (!wasQuitPressed)
            {
                DisplayPromptMsg();

                userInput = ReadLine();

                wasQuitPressed = quitKey.IsMatch(userInput);
                if (wasQuitPressed == false)
                {
                    if (IsStrinRightFormat(userInput) == true)
                    {
                        try
                        {
                            result = Calculate(userInput);
                            WriteLine("The result is:\n{0}", result);
                        }
                        catch (DivideByZeroException)
                        {

                            WriteLine("Division by zero not allowed, try again");
                        }


                    }
                    else
                    {
                        WriteLine("Wrong format");
                    }

                }
                else
                {
                    WriteLine("You quit the Calculator\nPress any key to quit the console");
                }

            }

            ReadKey();
        }
    }
}

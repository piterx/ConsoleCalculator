using System;
using static System.Console;
using System.Text.RegularExpressions;
using System.Collections.Generic;
namespace ConsoleCalculator
{
    public class Utilities
    {
        private enum States
        {
            //internal states of DetermineOperators() method
            operatorExpected,
            numberExpected

        }

        //define two regular expressions used by several methods within the Utilities class
        private static Regex uFloatingNumberPattern = new Regex(@"\d+(\.\d+)?");//unsigned floating number format
        private static Regex floatingNumberPattern = new Regex(@"-?\d+(\.\d+)?");//signed floating number format
        private static Regex operatorPattern = new Regex(@"[/*+-]");
        private static Regex arithmeticOperatorPatternString = new Regex(@"Division|Multiplication|Addition|Subtraction");
        private static Regex negationOperatorPatternString = new Regex(@"Negation");


        public static Queue<String> ConvertToPostfix(List<String> infixExpression)
        {
            //1.Converts equation in infix notation to postfix notation.
            //2.Uses simplified shunting yard algorithm without support for parenthesis or exponentiation
            //3.left associativity is assumed
            //4. As exponentiation is not supported, care would need to be taken in case of implementing exponentiation
            //and proper support for unary operators such as negation would be reuired

            Queue<String> outputQueue = new Queue<String>();
            Stack<String> operatorStack = new Stack<String>();
            Boolean negate = false; //negation flag, negate the number if true


            foreach (String token in infixExpression)
            {
                if (negationOperatorPatternString.IsMatch(token)) // remember that negation is represented by "Negation" to avoid ambiguity
                {
                    negate = true;//set negation flag to true
                }
                else if (uFloatingNumberPattern.IsMatch(token))
                {
                    //if an operand is detected enqueue into the output queue
                    outputQueue.Enqueue(negate ? "-" + token : token); //add minus sign to the number if negation required, otherwise copy as it is
                    negate = false; //reset negation flag
                }
                else if (arithmeticOperatorPatternString.IsMatch(token))
                {
                    //if there are operators in the stack and the operator on the top of stack has higher 
                    //or equal precedence compared to the input operator 
                    //then pop the operator from the operator stack and enqueue it in the output queue
                    while (operatorStack.Count > 0 && (GetOperatorPrecedence(operatorStack.Peek()) >= GetOperatorPrecedence(token)))
                    {
                        outputQueue.Enqueue(operatorStack.Pop());
                    }
                    operatorStack.Push(token);//push the input operator onto the operator stack
                }

            }

            //after all input tokens were read, check if there are any operators left on the operator stack
            //and enqueue them into the output queue
            while (operatorStack.Count > 0)
            {
                outputQueue.Enqueue(operatorStack.Pop());
            }
            return outputQueue;

        }

        public static List<String> ConvertToTokens(String inputString)
        {
            //Converts input string to tokens in the format of floating point numbers and following operators: +,-/,*
            //No distinction is made at this stage between unary and binary "-" operators

            List<String> listOfTokens = new List<string>();
            String pattern = @"\d+(\.\d+)?|[/*+-]";//matches floaing point number or operator
            Regex regularExpression = new Regex(pattern);
            MatchCollection matches = regularExpression.Matches(inputString);

            foreach (Match match in matches)
            {
                listOfTokens.Add(match.ToString());
            }

            return listOfTokens;
        }

        public static List<String> DetermineOperators(List<String> inputListOfTokens)
        {
            //determines the operators in an unambiguous way, this is required to ensure correct handling of negative numbers
            //the required input is List of strings where a string is either a number or an operator in an alternate fashion
            //unary negation is an operator and is not a part of number as far as DetermineOperators() method is concerned
            //prefered use with conjuction with ConvertToToken() method

            //simple state machine is implemented with two states possible: numberExpected and operatorExpected


            States currentState = States.numberExpected;//two states possible numberExpected and operatorExpected

            List<String> outputListOfTokens = new List<String>();

            foreach (String token in inputListOfTokens)
            {
                switch (currentState)
                {
                    case States.numberExpected:
                        {
                            if (token == "-")
                            {
                                //if a number is expected then "-" sign represents a negation, a unary operator
                                outputListOfTokens.Add("Negation");
                                //keep numberExpected state
                                currentState = States.numberExpected;
                            }

                            else if (uFloatingNumberPattern.IsMatch(token))
                            {
                                //add a decimal number to the output token list
                                outputListOfTokens.Add(token);
                                // now we expect an operator so change the state accordingly
                                currentState = States.operatorExpected;
                            }
                            else
                            { throw new ArgumentException("Wrong input string format, a string representing unsigned floating number expected "); }
                        }
                        break;
                    case States.operatorExpected:
                        {
                            if (operatorPattern.IsMatch(token))
                            {
                                //change an operator symbol to a corresponding unambiguous string
                                switch (token)
                                {
                                    case "+":
                                        outputListOfTokens.Add("Addition");
                                        break;
                                    case "-":
                                        outputListOfTokens.Add("Subtraction");
                                        break;
                                    case "*":
                                        outputListOfTokens.Add("Multiplication");
                                        break;
                                    case "/":
                                        outputListOfTokens.Add("Division");
                                        break;
                                    default:
                                        break;
                                }
                                //now expect a number so change the state accordingly
                                currentState = States.numberExpected;
                            }
                            else
                            {
                                throw new Exception("wrong string format, *,/,+ or - expected");
                            }
                        }
                        break;
                    default:

                        break;
                }
            }
            return outputListOfTokens;

        }

        public static void DisplayPromptMsg()
        {
            WriteLine("Please provide an expression to calculate and then press Enter");
        }

        public static void DisplayWelcomeMsg()
        {
            WriteLine("Welcome to Console Calculator" + "\r\n" + "PLEASE NOTE: NO parenthesis support avaliable\npress Q or q and Enter to quit the programme\n");

        }

        public static Int32 GetOperatorPrecedence(String operatorString)
        {
            //Returns the precedences for operators stored as strings
            // the higher the precedence number the higher operator precedence
            Int32 precedence = 0;
            if (operatorString == "Multiplication" || operatorString == "Division")
            {
                precedence = 2;
            }
            else if (operatorString == "Addition" || operatorString == "Subtraction")
            {
                precedence = 1;
            }
            else
            {
                throw new ArgumentException("Unsupported operator! Use: \"Multiplication\", \"Division\", \"Addition\" or \"Subtraction\"");
            }
            return precedence;
        }

        public static Boolean IsStrinRightFormat(String inputString)
        {
            //Checks if user input is valid

            //Whitespaces are allowed, negative numbers are allowed, parentheses are not allowed,
            //only +,-*,/ operators are allowed, full decimal number allowed i.e. .2+.3 input is not allowed
            String allowedFormatPattern = @"^(\s*-?\s*\d+(\.\d+)?\s*[/*+-]\s*)*\s*-?\s*\d+(\.\d+)?\s*$";
            Regex regularExpression = new Regex(allowedFormatPattern);

            Boolean isStringRight;

            isStringRight = regularExpression.IsMatch(inputString);

            return isStringRight;
        }

        public static Double PostfixCalculator(Queue<String> postFixExpression)
        {
            //Posfix(or Reverse Polish Notation) calculator
            //Uses stack to store values of the operations
            //Only basic arithmetic operators supported +,-,*,/
            //Operands described as following strings: Addition, Subtraction, Multiplication and Division
            //Input Expression stored as a queue of strings

            String currentToken = "";
            Stack<Double> resultStack = new Stack<Double>();
            Double firstOperand = 0;
            Double secondOperand = 0;

            while (postFixExpression.Count > 0)
            {
                currentToken = postFixExpression.Dequeue();//dequeue current token

                if (floatingNumberPattern.IsMatch(currentToken))
                {
                    //convert a string token into double precision floating point number and push them onto the stack
                    resultStack.Push(Double.Parse(currentToken));
                }
                else if (arithmeticOperatorPatternString.IsMatch(currentToken))
                {
                    // Pop operands from the stack
                    secondOperand = resultStack.Pop();
                    firstOperand = resultStack.Pop();

                    //push the result onto stack
                    resultStack.Push(TwoOperandCalculator(firstOperand, secondOperand, currentToken));
                }
            }

            //the stack at the end of calculation should contain only one value,
            //so a check is performed below and an exception is thrown if stack count is higher than 1
            if (resultStack.Count == 1)
                return resultStack.Pop();
            else
                throw new Exception("result stack contains more than one value");
        }

        public static String ReduceOperators(String inputString)
        {
            //Reduces "--" and "+-" to "+" and "-" respectively
            Regex whitespaces = new Regex(@"\s");
            if (whitespaces.IsMatch(inputString))
            {
                //test for this not yet implemented
                throw new ArgumentException("ReduceOperators method does not accept strings with white spaces, " +
                    "remove whitespaces from the input string before passing it");

            }
            else
            {

                String outputString;
                //replace allowed double operators by equivalent single operators 
                outputString = inputString.Replace("--", "+");
                outputString = outputString.Replace("+-", "-");// note that -+ is not accepted format, thus not included, format checked in a seperate method
                return outputString;
            }
        }

        public static String RemoveWhitespaces(String inputString)
        {
            //Removes white space characters from the inputString 
            String outputString;

            //Split the input string separated by whitespace characters into an array of strings
            String[] stringArray = inputString.Split();

            //Join the strings back together and return inputString with whitespaces removed
            outputString = String.Concat(stringArray);
            return outputString;
        }

        public static Double TwoOperandCalculator(Double firstOperand, Double secondOperand, String operatorToken)
        {
            //simple two operand calculator designed to use with PostfixCalculator()

            Double result = 0;

            //Assign perform desired operation depending on input operator token 
            switch (operatorToken)
            {
                case "Multiplication":
                    result = firstOperand * secondOperand;
                    break;
                case "Division":
                    {
                        //check for division by zero
                        if (secondOperand.Equals(0))
                        {
                            DivideByZeroException divideByZeroException = new DivideByZeroException();
                            throw divideByZeroException; //throw an exception otherwise infinity would be calculated
                        }
                        else
                        {
                            result = firstOperand / secondOperand;
                        }
                    }
                    break;
                case "Subtraction":
                    result = firstOperand - secondOperand;
                    break;
                case "Addition":
                    result = firstOperand + secondOperand;
                    break;
                default:
                    throw new ArgumentException("Unknown operator");//check for unknown operator

            }
            return result;
        }
    }
}

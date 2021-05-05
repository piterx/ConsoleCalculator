using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConsoleCalculator;
using System;
using System.Collections.Generic;
using System.Text;
using static ConsoleCalculator.Utilities;

namespace ConsoleCalculator.Tests
{
    [TestClass()]
    public class UtilitiesTests
    {
        //Test for valid input
        [TestMethod()]
        public void IstStringRightFormatTest()
        {
            //NB: it was written initially as two seperate tests but was merged at the later stage for clarity
            //Arrange test cases
            List<String> validInputStrings = new List<String> {
                "0",
                "1",
                "2+2",
                "2.5+9.8",
                "4.3-9",
                "3.9-2.5",
                "7-12",
                "5*9",
                "7*8-5",
                "56/2+8*3-4.5",
                "-1",
                "-2+2",
                "-2.5+9.8",
                "-4.3-9",
                "-3.9-2.5",
                "-7-12",
                "-5*9",
                "-7*8-5",
                "-56/2+8*3-4.5",
                "- 56 / 2 + 8 * 3 - 4.5",//whitespaces added
                "-56 / 2 + 8*3 -4.5",//whitespaces added
                " -56 / 2 + 8*3 -4.5 ",//whitespaces added
                " -56 / 2 + 8*3\t-4.5 ",//whitespaces added also at the beginning and the end of the string
                "\t-56 / 2 + 8*3\t-4.5\t",//whitespaces added also at the beginning and the end of the string*/
                "4--4"// testing double minus as it's allowed, NB this will be reduced to 4+4 before parsing
            };

            //Arrange test cases for invalid input 

            List<String> invalidInputStrings = new List<String> {
                "a",
                "2.2.1",
                "b",
                "(2+2)*5",
                "2//3",
                "3**2",
                @"2\4",
                "2..1",
                "2.",
                ".2",
                "..2",
                ".-.",
                ".12.-2",
                "8/8..2",
                ". .",
                "-*",
                "-*2",
                "/*2",
                "+*2"


            };



            foreach (String element in validInputStrings)
            {
                Assert.IsTrue(IsStrinRightFormat(element));//Act and Assert in one loop for valid Input
            }

            foreach (String element in invalidInputStrings)
            {
                Assert.IsFalse(IsStrinRightFormat(element));//Act and Assert in one loop for invalid input
            }


        }



        [TestMethod()]
        public void RemoveWhitespacesTest()
        {

            //arrange input
            List<String> inputStrings = new List<string> {"aaabbbccc",
                " aaabbbccc",
                " aaabbbccc ",
                "aaa bbbccc",
                "aaa bbb ccc",
                " aaa bbb ccc ",
                "\taaa\tbbb\tccc",
                " 2.1 + 3/ 4 +8* 9 -1 ",
                "-     2.1 + 3/ 4 +8* 9 -1 "
            };

            //arrange expected output
            List<String> expectedOutput = new List<string> {"aaabbbccc",
                "aaabbbccc",
                "aaabbbccc",
                "aaabbbccc",
                "aaabbbccc",
                "aaabbbccc",
                "aaabbbccc",
                "2.1+3/4+8*9-1",
                "-2.1+3/4+8*9-1"
            };

            List<String> actualOutput = new List<string>();

            //act
            foreach (String element in inputStrings)
            {
                actualOutput.Add(RemoveWhitespaces(element));//call the function under test
            }

            //assert
            CollectionAssert.AreEqual(expectedOutput, actualOutput); //compare expected and actual outputs
        }

        [TestMethod()]
        public void ConvertToTokensTest()
        {
            //arrange inputs
            List<String> inputStrings = new List<string> { "-2.5/-4.2+3-7*8-6",
            " - 2.5 / -4.2 +3 - 7*8 -6"//check if passes the test with whitespaces
            };
            //arrange the expected output
            List<String> expectedOuput = new List<string>
                {
                "-","2.5","/","-","4.2","+","3","-","7","*","8","-","6"
            };

            foreach (String element in inputStrings)
            {
                List<String> actualTokenList = ConvertToTokens(element);//act - call the function under test
                CollectionAssert.AreEqual(expectedOuput, actualTokenList);//compare expected and actual outputs
            }
        }

        [TestMethod()]
        public void DetermineOperatorsTest()
        {
            //arrange inputs and expected outputs
            List<String> inputListOfTokens = new List<String> { "-", "2.5", "/", "-", "4.2", "+", "3", "-", "7", "*", "8", "-", "6" };
            List<String> expectedListOfTokens = new List<String> { "Negation", "2.5", "Division", "Negation", "4.2", "Addition", "3", "Subtraction", "7", "Multiplication", "8", "Subtraction", "6" };


            //act - call the function under test
            List<String> actualListOfTokens = DetermineOperators(inputListOfTokens);

            //assert - compare expected and actual outputs
            CollectionAssert.AreEqual(expectedListOfTokens, actualListOfTokens);

        }

        [TestMethod()]
        public void ConvertToPostfixTest()
        {
            //arrange inputs and expected outputs
            List<String> inputTokens = new List<String> { "Negation", "2.5", "Division", "Negation", "4.2", "Addition", "3", "Subtraction", "7", "Multiplication", "8", "Subtraction", "6" };
            List<String> expextedPostfixTokensList = new List<string> { "-2.5", "-4.2", "Division", "3", "Addition", "7", "8", "Multiplication", "Subtraction", "6", "Subtraction" };

            Queue<String> expectedPostfixTokens = new Queue<String>(); //the output is a queue

            //convert a List into a Queue
            foreach (String token in expextedPostfixTokensList)
            {
                expectedPostfixTokens.Enqueue(token);
            }

            //act - call the method under test
            Queue<String> actualPostixTokens = ConvertToPostfix(inputTokens);

            //assert - compare expected and actual outputs
            CollectionAssert.AreEqual(expectedPostfixTokens, actualPostixTokens);
        }

        [TestMethod()]
        public void GetOperatorPrecedenceTest()
        {
            //arrange inputs and expected outputs
            List<String> input = new List<string> { "Multiplication", "Division", "Subtraction", "Addition" };
            List<Int32> expectedOutput = new List<int> { 2, 2, 1, 1 };
            List<Int32> actualOutput = new List<int>();

            //act - call the method under test

            foreach (String element in input)
            {
                actualOutput.Add(GetOperatorPrecedence(element));
            }

            //assert - compare expected and actual outputs
            CollectionAssert.AreEqual(expectedOutput, actualOutput);
        }

        [TestMethod()]
        public void TwoOperandCalculatorTest()
        {
            //arrange - note: all the permutations will be calculated
            List<Double> firstOperands = new List<double> { 2, 2.25, -2, -2.25 };
            List<Double> secondOperands = new List<double> { 3, -3 };
            List<Double> expectedResults = new List<double>
            {
                2.0 + 3.0, 2.0 - 3.0, 2.0 * 3.0, 2.0 / 3.0,
                2.0 - 3.0, 2.0 + 3.0, 2.0 * -3.0, 2.0 / -3.0,
                  2.25 + 3.0, 2.25 - 3.0, 2.25 * 3.0, 2.25 / 3.0,
                2.25 - 3.0, 2.25 + 3.0, 2.25 * -3.0, 2.25 / -3.0,

                  -2.0 + 3.0, -2.0 - 3.0, -2.0 * 3.0, -2.0 / 3.0,
                -2.0 - 3.0, -2.0 + 3.0, -2.0 * -3.0, -2.0 / -3.0,
                  -2.25 + 3.0, -2.25 - 3.0, -2.25 * 3.0, -2.25 / 3.0,
                -2.25 - 3.0, -2.25 + 3.0, -2.25 * -3.0, -2.25 / -3.0
            };

            List<String> operators = new List<String> { "Addition", "Subtraction", "Multiplication", "Division" };

            List<Double> actualResults = new List<double>();

            // String unknownOperator = "Unknown Operator"; //to check Unknown operator - not implemented yet

            //Act - call the method under test, all the input permutations
            for (int i = 0; i < firstOperands.Count; i++)
            {
                for (int j = 0; j < secondOperands.Count; j++)
                {
                    for (int k = 0; k < operators.Count; k++)
                    {
                        actualResults.Add(TwoOperandCalculator(firstOperands[i], secondOperands[j], operators[k]));
                    }
                }
            }
            //assert - compare expected and actual outputs
            CollectionAssert.AreEqual(expectedResults, actualResults);


        }

        [TestMethod()]
        public void PostfixCalculatorTest()
        {
            //arrange input and expected resuls
            Double expectedResult = -2.5 / -4.2 + 3 - 7 * 8 - 6;

            List<String> inputTokens = new List<string>
            {
                "-2.5", "-4.2", "Division", "3", "Addition", "7", "8", "Multiplication", "Subtraction", "6", "Subtraction"
            };

            Double actualResult = 0;
            Queue<String> inputTokensQueue = new Queue<string>();

            //convert input List to a Queue
            foreach (String token in inputTokens)
            {
                inputTokensQueue.Enqueue(token);

            }

            //act - call the method under test
            actualResult = PostfixCalculator(inputTokensQueue);

            //assert - compare expected and actual outputs  

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void ReduceOperatorsTest()
        {
            //arrange inputs and expeted outputs
            List<String> inputStrings = new List<string> { "2+-3", "2--3" };
            List<String> expectedStrings = new List<string> { "2-3", "2+3" };

            List<String> actualStrings = new List<string>();


            //act - invoke the method under test
            foreach (String element in inputStrings)
            {
                actualStrings.Add(ReduceOperators(element));
            }

            //assert - compare expected and actual outputs
            CollectionAssert.AreEqual(expectedStrings, actualStrings);



        }
    }
}
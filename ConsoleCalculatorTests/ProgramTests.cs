using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConsoleCalculator;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCalculator.Tests
{
    [TestClass()]
    public class ProgramTests
    {
        [TestMethod()]
        public void CalculateTest()
        {
            //Test of the Calculate method

            //Arrange input 
            List<String> inputStrings = new List<string>
                {
            "2+2",
            "-2+2",
            "-2-2",
            "2*2",
            "-2*2",
            "-2*-2",
            "2/2",
            "-2/2",
            "-2/-2",//test simple expressions

            "2.0+2.0",
            "-2.0+2.0",
            "-2.0-2.0",
            "2.0*2.0",
            "-2.0*2.0",
            "-2.0*-2.0",
            "2.0/2.0",
            "-2.0/2.0",
            "-2.0/-2.0",//test simple expressions with in decimal format

            "3/5",//tests if this will result in a floating point division as integer division is not allowed

            "10-2*6/4", //expression from the specification
            "12.8*7+5-2.9/12+852+2-78.5*7-7/2+9.2-1.1+88.1",//more complicated expression

            //quite long expression to check if there are any performance issues with regular expressions, previous expression added 10 times 
            "12.8*7+5-2.9/12+852+2-78.5*7-7/2+9.2-1.1+88.1+12.8*7+5-2.9/12+852+2-78.5*7-7/2+9.2-1.1+88.1+12.8*7+5-2.9/12+852+2-78.5*7-7/2+9.2-1.1+88.1+12.8*7+5-2.9/12+852+2-78.5*7-7/2+9.2-1.1+88.1+12.8*7+5-2.9/12+852+2-78.5*7-7/2+9.2-1.1+88.1+12.8*7+5-2.9/12+852+2-78.5*7-7/2+9.2-1.1+88.1+12.8*7+5-2.9/12+852+2-78.5*7-7/2+9.2-1.1+88.1+12.8*7+5-2.9/12+852+2-78.5*7-7/2+9.2-1.1+88.1+12.8*7+5-2.9/12+852+2-78.5*7-7/2+9.2-1.1+88.1+12.8*7+5-2.9/12+852+2-78.5*7-7/2+9.2-1.1+88.1+12.8*7+5-2.9/12+852+2-78.5*7-7/2+9.2-1.1+88.1",
            };

            //Arrange expected values
            List<Double> expectedOutput = new List<double> {2+2,
            -2+2,
            -2-2,
            2*2,
            -2*2,
            -2*-2,
            2/2,
            -2/2,
            -2/-2,

             2.0+2.0,
            -2.0+2.0,
            -2.0-2.0,
            2.0*2.0,
            -2.0*2.0,
            -2.0*-2.0,
            2.0/2.0,
            -2.0/2.0,
            -2.0/-2.0,

            3.0/5.0,

            7,
            12.8*7+5-2.9/12+852+2-78.5*7-7.0/2.0+9.2-1.1+88.1,
            12.8*7+5-2.9/12+852+2-78.5*7-7.0/2.0+9.2-1.1+88.1+12.8*7+5-2.9/12+852+2-78.5*7-7.0/2.0+9.2-1.1+88.1+12.8*7+5-2.9/12+852+2-78.5*7-7.0/2.0+9.2-1.1+88.1+12.8*7+5-2.9/12+852+2-78.5*7-7.0/2.0+9.2-1.1+88.1+12.8*7+5-2.9/12+852+2-78.5*7-7.0/2.0+9.2-1.1+88.1+12.8*7+5-2.9/12+852+2-78.5*7-7.0/2.0+9.2-1.1+88.1+12.8*7+5-2.9/12+852+2-78.5*7-7.0/2.0+9.2-1.1+88.1+12.8*7+5-2.9/12+852+2-78.5*7-7.0/2.0+9.2-1.1+88.1+12.8*7+5-2.9/12+852+2-78.5*7-7.0/2.0+9.2-1.1+88.1+12.8*7+5-2.9/12+852+2-78.5*7-7.0/2.0+9.2-1.1+88.1+12.8*7+5-2.9/12+852+2-78.5*7-7.0/2.0+9.2-1.1+88.1
            };
            List<Double> actualOutput = new List<double>();

            //Act - call method under test
            foreach (String element in inputStrings)
            {
                actualOutput.Add(Program.Calculate(element));//Call the Calculate method and add to the output list 
            }


            //Assert - compare expected and actual results
            CollectionAssert.AreEqual(expectedOutput, actualOutput);//compare expected output and the actual output
        }
    }
}
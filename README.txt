Console Calculator project by Piotr Golacki January 2021

Console Calculator is a simple command line calculator.
The programme demonstrates the use of shunting yard algorithm and explores use of regular expressions to validate the input.
It is a learning project, NOT a production code. Some aspects such as use regular expressions may not be optimal solutions to the problem.
More details on the implementation and behaviour can be found in Documentation.txt

Four basic operators are supported i.e. +,-,* and / which correspond to addition, subtraction, multiplication and floating-point division.
The programme supports negative numbers. Parentheses are not supported.
Neither integer division nor modulo division are supported.

The input format is <number1><operator1><number2><operator2><number3>....<numberN-1><operatorN-1><numberN>
where number is a decimal number (positive or negative) in a format: <minus(optional)><digit(s)>.<digit(s)>
If only a number is typed in the number is returned as a result.
examples of valid inputs:

10-2*6/4
-10-2*6/4
10-2*-6/-4
10.5-2.1*-6.2/-4
2
2--2
2+-2

examples of invalid inputs:
2-*2
.2+.9
(2+1)*2
2-+2

Press Q and then Enter to Quit the programme.

The programme checks for the division by zero and invalid input and displays the appropriate message. 

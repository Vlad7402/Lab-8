using System;
using System.Collections.Generic;
using System.IO;

namespace Lab_8
{
    static class RPN
    {
        public static string GetExpression(string input)
        {
            string result = "  ";
            Stack<char> operations = new Stack<char>();
            int prefixisStart = 0;
            int prefixSkip = 0;
            int prefixisEnd = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == 'x')
                {
                    if (IsPrefix(i, input))
                    {
                        result += '-';
                    }
                    result += "x ";
                }
                if (Char.IsDigit(input[i]))
                {
                    if (IsPrefix(i, input))
                    {
                        result += '-';
                    }
                    while (!IsOperation(input[i]) || input[i] == '.')
                    {
                        result += input[i];
                        i++;
                        if (i == input.Length) break;
                    }
                    result += " ";
                    i--;
                }

                if (IsOperation(input[i]))
                {
                    if (input[i] == '(')
                    {
                        operations.Push(input[i]);
                        if (prefixisStart > 0)
                        {
                            result += "-1 ";
                            prefixisStart--;
                            prefixisEnd++;
                        }
                        else prefixSkip++;
                    }
                    else if (input[i] == ')')
                    {
                        string ejaktedOperation = operations.Pop().ToString();
                        while (ejaktedOperation != "(")
                        {
                            result += ejaktedOperation + ' ';
                            ejaktedOperation = operations.Pop().ToString();
                        }
                        if (prefixisEnd > 0 && prefixSkip == 0)
                        {
                            result += "* ";
                            prefixisEnd--;
                        }
                        else prefixSkip--;
                    }
                    else if (input[i] == '-')
                    {
                        if (input[i + 1] == '(') prefixisStart++;

                        else if (!IsPrefix(i + 1, input)) AddOperation(operations, ref result, i, input);
                    }
                    else
                    {
                        AddOperation(operations, ref result, i, input);
                    }
                }
            }
            while (operations.Count > 0)
                result += operations.Pop() + " ";
            return result;
        }
        public static double Counting(string input, double velOfX)
        {
            Stack<double> nums = new Stack<double>();
            for (int i = 0; i < input.Length; i++)
            {
                if (Char.IsWhiteSpace(input[i])) continue;

                if (input[i] == 'x') 
                {
                    if (IsPrefix(i, input)) nums.Push(velOfX * -1);
                    else nums.Push(velOfX); 

                }

                else if (Char.IsDigit(input[i]))
                {
                    string num = string.Empty;
                    if (IsPrefix(i, input)) num += '-';

                    while (!IsOperation(input[i]) || input[i] == '.')
                    {
                        num += input[i];
                        i++;
                        if (i == input.Length) break;
                    }
                    num += " ";
                    i--;
                    nums.Push(double.Parse(num));
                }
                else if (IsOperation(input[i]))
                {
                    double firstNum = nums.Pop();
                    double secNum = 0;
                    if (!IsOperationUnar(input[i])) secNum = nums.Pop();

                    nums.Push(GetOperationResult(firstNum, secNum, input[i]));
                }
            }
            return nums.Peek();
        }
        private static int GetPriority(char operation)
        {
            switch (operation)
            {
                case '+':
                case '-': return 0;
                case '*':
                case '/': return 1;
                case 's':
                case 'c':
                case 't':
                case 'l':
                case '!':
                case '^': return 2;
                default: return -1;
            }
        }
        private static bool IsOperation(char operation)
        {
            if (("+-/*^()sctl!".IndexOf(operation) != -1))
                return true;
            return false;
        }
        private static double GetOperationResult(double firstNum, double secNum, char operation)
        {
            double result = 0;
            switch (operation)
            {
                case '+': result = secNum + firstNum; break;
                case '-': result = secNum - firstNum; break;
                case '*': result = secNum * firstNum; break;
                case '/': if (IsDevisionAvaliable(firstNum)) result = secNum / firstNum; break;
                case 's': Math.Sin(firstNum); break;
                case 'c': Math.Cos(firstNum); break;
                case 't': Math.Tan(firstNum); break;
                case 'l': Math.Log(firstNum); break;
                case '!': if (IsFactorialAvaliable(firstNum)) GetFactorial(firstNum); break;
                case '^': result = Math.Pow(secNum, firstNum); break;
            }
            return result;
        }
        private static bool IsDevisionAvaliable(double val)
        {
            if (val != 0) return true;
            else
            {
                GetError("Ошибка: деление на 0");
                Environment.Exit(0);
                return false;
            }
        }
        private static double GetFactorial(double val)
        {
            double result = 1;
            for (int i = 1; i < val; i++)
            {
                result = result * i;
            }
            return result;
        }
        private static bool IsFactorialAvaliable(double val)
        {
            if (val > 150) return true;
            else
            {
                GetError("Ошибка: превышение максималного значения для факториала(150) или значение равно 0");
                return false;
            }
        }
        private static bool IsOperationUnar(char operation)
        {
            if (("sctl!".IndexOf(operation) != -1))
                return true;
            return false;
        }
        private static void AddOperation (Stack<char> operations, ref string result, int inputSymbol, string input) 
        {
            if (operations.Count > 0)
                if (GetPriority(input[inputSymbol]) <= GetPriority(operations.Peek()))
                    result += operations.Pop().ToString() + " ";
            operations.Push(char.Parse(input[inputSymbol].ToString()));
        }
        private static bool IsPrefix(int inputSymbol, string input)
        {
            return (input[inputSymbol - 1] == '-' && (input[inputSymbol - 2] == ' ' || input[inputSymbol - 2] == '('));
        }
        private static void GetError(string errorText)
        {
            File.WriteAllText("output.txt", errorText);
            Console.WriteLine(errorText);
            Console.WriteLine("Для продолжения нажмите любую клавишу...");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}
//Код данного алгаритма изначально не мой, тихо стыбзил и ушёл... Но он оказался настолько забагованным и недееспособным, что, как оказалось,
//с нуля его было бы проще написать. По итогу проведённой работы в 5-6 часов могу смело заявить, что моё ИМХО - этот код мой.
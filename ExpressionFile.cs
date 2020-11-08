using System;
using System.Collections.Generic;
using System.IO;

namespace Lab_8
{
    static class ExpressionFile
    {
        public static string[] GetExpressionsFromFile()
        {
            string input = File.ReadAllText("input.txt");
            input = input.ToLower();
            if (IsInafCuters(input)) return input.Split(new char[] { '|' });
            else GetError("Ошибка: в фаёле отсутствуют все необходимые компoненты.");
            return null;
        }
        private static void GetError(string errorText)
        {
            Console.Beep(500, 200);
            File.WriteAllText("output.txt", errorText);
            Console.WriteLine(errorText);
            Console.WriteLine("Для продолжения нажмите любую клавишу...");
            Console.ReadKey();
            Environment.Exit(0);
        }
        private static bool IsInafCuters(string input) 
        {
            int counter = 0;
            for (int i = 0; i < input.Length; i++) if (input[i] == '|') counter++;
            return counter == 3;
            //Через | подряд вводятся формула, X начальное, X конечное, шаг
        }
        public static string DeletWhitespaces(string input)
        {
            string result = string.Empty;
            string[] splited = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < splited.Length; i++) result += splited[i];
            return result;
        }
        public static bool DoesEndExist(double velStart, double velEnd, double step)
        {
            if (velStart < velEnd && Math.Sign(step) == -1) GetError("Ошибка: количество значений X превышает 1000");
            else if (velStart > velEnd && Math.Sign(step) == 1) GetError("Ошибка: количество значений X превышает 1000");
            else if (Math.Sign(step) == 0) GetError("Ошибка: количество значений X превышает 1000");
            for (double i = velStart; i < velEnd; i++)
            {
                velStart += step;
                if (i > 1000) GetError("Ошибка: количество значений X превышает 1000");
            }
            return true;
        }
        public static double DoubleParser(string input)
        {
            double result = 0;
            if (!double.TryParse(input, out result)) GetError("Ошибка: невозможно преобразовать значение X начальное, X конечное или шаг в нужный формат.");
            return result;
        }
        private static bool IsInUse(char symbol)
        {
            if (("+-/*^()sctl!0123456789.xy=".IndexOf(symbol) != -1))
                return true;
            return false;
        }
        public static void ExpressionCheck(string input)
        {
            int openBracket = 0;
            int closeBracket = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '(') openBracket++;
                if (input[i] == ')') closeBracket++;
                if (!IsInUse(input[i]))
                {
                    Console.Clear();
                    Console.WriteLine(input);
                    Console.SetCursorPosition(i, 1);
                    Console.WriteLine('^');
                    GetError("Ошибка: неизвестная операция в символе " + (i + 1));
                }
                if (i >= 1)
                    if (IsOperation(input[i]) && IsOperation(input[i - 1])) GetError("Ошибка: 2 бинарных оператора рядом");
            }
            if (openBracket != closeBracket) GetError("Ошибка: нарушение при постановке скобок");
        }
        private static bool IsOperation(char operation)
        {
            if (("+-/*^".IndexOf(operation) != -1))
                return true;
            return false;
        }
        public static double[] GetArgumentVels(double velStart, double velEnd, double step)
        {
            List<double> result = new List<double>();
            for (double i = velStart; i < velEnd; i++)
            {
                result.Add(velStart + step);
                velStart += step;
            }
            return result.ToArray();
        }
    }
}

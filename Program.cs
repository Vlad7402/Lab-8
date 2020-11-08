using System;
using System.Collections.Generic;

namespace Lab_8
{
    class Program
    {
        static void Main()
        {
            string[] input = ExpressionFile.GetExpressionsFromFile();
            double velStart = ExpressionFile.DoubleParser(ExpressionFile.DeletWhitespaces(input[1]));
            double velEnd = ExpressionFile.DoubleParser(ExpressionFile.DeletWhitespaces(input[2]));
            double step = ExpressionFile.DoubleParser(ExpressionFile.DeletWhitespaces(input[3]));
            ExpressionFile.DoesEndExist(velStart, velEnd, step);
            double[] velsOfX = ExpressionFile.GetArgumentVels(velStart, velEnd, step);
            string expression = ExpressionFile.DeletWhitespaces(input[0]);
            ExpressionFile.ExpressionCheck(expression);
            string[] toConvertInExpression = expression.Split(new char[] { 'y', '=' }, StringSplitOptions.RemoveEmptyEntries);
            expression = string.Empty;
            for (int i = 0; i < toConvertInExpression.Length; i++) expression += toConvertInExpression[i];
            string expressionInRPN = RPN.GetExpression("  " + expression);
            List<double> velsOfY = new List<double>();
            for (int i = 0; i < velsOfX.Length; i++) velsOfY.Add(RPN.Counting(expressionInRPN, velsOfX[i]));
            Table.GetTable(velsOfX, velsOfY.ToArray());
        }
    }
}

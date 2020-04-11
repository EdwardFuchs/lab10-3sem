using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        var num1 = new double[2, 2] { { 1, 2 }, { 3, 4 } };
        var num2 = new double[3, 3] { { 1, 2, 3 }, { 3, 2, 1 }, {2, 1, 3 } };
        var num3 = new double[3, 3] { { 4, 5, 6 }, { 6, 5, 4 }, { 4, 6, 5 } };
        try
        {
            Console.WriteLine("=============================================");
            Console.WriteLine("\tMatrix\t");
            Console.WriteLine("=============================================");
            var mat1 = new Matrix<double>(num2);
            var mat2 = new Matrix<double>(num3);
            var matVoid = new Matrix<double>();
            Console.WriteLine("Matrix 1:");
            Console.WriteLine(mat1);
            Console.WriteLine();
            Console.WriteLine("Matrix 2:");
            Console.WriteLine(mat2);
            Console.WriteLine();
            Console.WriteLine("Transposition Matrix 1:");
            Console.WriteLine(mat1.Trans());
            Console.WriteLine();
            Console.WriteLine("Reverse Matrix 1:");
            Console.WriteLine(mat1.Reverse());
            Console.WriteLine();
            Console.WriteLine("Determ Matrix 1:");
            Console.WriteLine(mat1.Determ());
            Console.WriteLine();
            Console.WriteLine("Determ Matrix 2:");
            Console.WriteLine(mat2.Determ());
            Console.WriteLine();
            Console.WriteLine("Summ:");
            Console.WriteLine(mat1 + mat2);
            Console.WriteLine();
            Console.WriteLine("Sub:");
            Console.WriteLine(mat1 - mat2);
            Console.WriteLine();
            Console.WriteLine("Mult:");
            Console.WriteLine(mat1 * mat2);
            Console.WriteLine();
            Console.WriteLine("Div:");
            Console.WriteLine(mat1 / mat2);
            Console.WriteLine();
            Console.WriteLine("Foreach:");
            foreach(var element in mat1)
                Console.Write($"{element} ");
            Console.WriteLine();
            Console.WriteLine("=============================================");
            Console.WriteLine("\tPolynom\t");
            Console.WriteLine("=============================================");
            var pol1 = new Polynom<Matrix<double>>();
            pol1.Add(mat1, 1);
            pol1.Add(mat2, 0);
            var pol2 = new Polynom<Matrix<double>>();
            pol2.Add(mat2, 1);
            pol2.Add(mat1, 0);
            var pol3 = new Polynom<double>();
            pol3.Add(new KeyValuePair<double, int>(1, 2));
            pol3.Add(new KeyValuePair<double, int>(7, 1));
            pol3.Add(new KeyValuePair<double, int>(2, 0));
            Console.WriteLine("Polynom 1:");
            Console.WriteLine(pol1);
            Console.WriteLine("Polynom 2:");
            Console.WriteLine(pol2);
            Console.WriteLine("Polynom 3:");
            Console.WriteLine(pol3);
            Console.WriteLine("Summ:");
            Console.WriteLine(pol1 + pol2);
            Console.WriteLine("Sub:");
            Console.WriteLine(pol1 - pol2);
            Console.WriteLine("Mult:");
            Console.WriteLine(pol1 * pol2);
            Console.WriteLine();
            var x = 3;
            Console.WriteLine($"Solve Polynom1 (x = {x}):");
            Console.WriteLine(pol1.Solve(x));
            Console.WriteLine("Composition pol3(pol3)");
            Console.WriteLine(pol3.Composition(pol3));
            Console.WriteLine();
            Console.WriteLine("Foreach:");
            foreach (var element in pol1)
                Console.Write($"{element} ");
            Console.WriteLine();
        }
        catch (MyException e)
        {
            Console.WriteLine($"Error: {e.Message}");
                
        }
            
    }
}
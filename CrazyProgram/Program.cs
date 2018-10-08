using System;
using System.Runtime.CompilerServices;
using Calculator;

[assembly: IgnoresAccessChecksTo("Calculator")]
namespace CrazyProgram
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var square = new Square(4); // internal type
            var calculator = new AreaCalculator(); // internal type
            var area = calculator.Calculate(square); // private method
            Console.WriteLine($"Square with a side of 4 has an area of {area}");
        }
    }
}

namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class IgnoresAccessChecksToAttribute : Attribute
    {
        public IgnoresAccessChecksToAttribute(string assemblyName)
        {
            AssemblyName = assemblyName;
        }

        public string AssemblyName { get; }
    }
}

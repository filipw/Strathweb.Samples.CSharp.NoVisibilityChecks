using System;

namespace Calculator
{
    class Square
    {
        public Square(double side)
        {
            Side = side;
        }

        public double Side { get; }
    }

    class AreaCalculator
    {
        private double Calculate(Square square) => square.Side * square.Side;
    }
}

using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    class ArchimedeanSpiral
    {
        /*
         * Realization of http://hijos.ru/2011/03/09/arximedova-spiral/ formulas
        */

        public readonly double SpiralRadius;
        public Point Center { get; }
        private readonly double angleStep;
        
        public double SpiralAngle { get; private set; } = 0;

        public ArchimedeanSpiral(Point center, double angleStep = 0.1, double spiralRadius = 1)
        {
            this.angleStep = angleStep;
            Center = center;
            SpiralRadius = spiralRadius;
        }

        public Tuple<double, double> GetPoint()
        {
            SpiralAngle += angleStep;
            
            var x = SpiralRadius * SpiralAngle * Math.Cos(SpiralAngle);
            var y = SpiralRadius * SpiralAngle * Math.Sin(SpiralAngle);

            return Tuple.Create(Center.X + x, Center.Y + y);
        }

        public Point BalancePoint(Tuple<double, double> point) =>
            new Point((int)Math.Floor(point.Item1), (int)Math.Ceiling(point.Item2));

        private const int RadiusDifference = -5;

        public bool CheckBalancedPointOnSpiral(Point point)
        {
            var spiral = new ArchimedeanSpiral(Center, angleStep, SpiralRadius);
            var balancedPoint = spiral.BalancePoint(spiral.GetPoint());
            while (point.GetDistance(Center) - balancedPoint.GetDistance(Center) > RadiusDifference)
            {
                balancedPoint = spiral.BalancePoint(spiral.GetPoint());
                if (balancedPoint == point) return true;
            }

            return false;
        }

    }
}

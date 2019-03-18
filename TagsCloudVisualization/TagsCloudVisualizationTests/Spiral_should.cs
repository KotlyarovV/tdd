using System;
using System.Drawing;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    class Spiral_should
    {
        private ArchimedeanSpiral spiral;
        private Point startPoint;

        [SetUp]
        public void SetUp()
        {
            startPoint = new Point(1, 1);
            spiral = new ArchimedeanSpiral(startPoint);
        }


        /*
         * test based on http://hijos.ru/2011/03/09/arximedova-spiral/
         */
        [TestCase(1)]
        [TestCase(50)]
        [TestCase(100)]
        public void CheckSpiralPoints_MustBeCorrectlyDefined(int numberOfPoints)
        {
            for (var i = 0; i < numberOfPoints; i++)
            {
                var spiralPoint = spiral.GetPoint();

                spiralPoint = Tuple.Create(spiralPoint.Item1 - spiral.Center.X, spiralPoint.Item2 - spiral.Center.Y);
                
                var sumXYSquares = Math.Pow(spiralPoint.Item1, 2) + Math.Pow(spiralPoint.Item2, 2);
                var radiusAndAngleSquare = Math.Pow(spiral.SpiralAngle * spiral.SpiralRadius, 2);

                Assert.True(Math.Abs(sumXYSquares - radiusAndAngleSquare) < 0.000001);
            }
        }
    }
}

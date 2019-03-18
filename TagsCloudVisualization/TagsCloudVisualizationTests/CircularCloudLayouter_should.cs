using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal.Filters;


namespace TagsCloudVisualization
{
    [TestFixture]
    class CircularCloudLayouter_should
    {
        private Point startPoint;
        private Size sizeMin;
        private CircularCloudLayouter circularCloud;

        private void InitializationOfRandomRectangles(int numberOfPoints)
        {
            Visualisator visualisator = new Visualisator(circularCloud);
            visualisator.GenerateRandomRectangles(numberOfPoints);
        }

        [SetUp]
        public void SetUp()
        {
            startPoint = new Point(1, 1);
            circularCloud = new CircularCloudLayouter(startPoint);
            sizeMin = new Size(1, 1);
        }

        [Test]
        public void GetCenterPiont_ReturnCenterPoint()
        {
            Assert.AreEqual(circularCloud.Spiral.Center, new Point(1, 1));
        }

        [Test]
        public void SetFirstMinRectangle_SetRectangleUpperCentre()
        {
            var rectangle = circularCloud.PutNextRectangle(sizeMin);
            rectangle.Should().Be(new Rectangle(1, 2, 1, 1));
        }

        [Test]
        public void SetTwoRectangle_SetSecondUpperFirst()
        {
            circularCloud.PutNextRectangle(sizeMin);
            var secondRectangle = circularCloud.PutNextRectangle(sizeMin);
            secondRectangle.Should().Be(new Rectangle(1, 3, 1, 1));
        }

        [Test]
        public void SetThreeRectangle_SetInStraightAngel()
        {
            circularCloud.PutNextRectangle(sizeMin);
            circularCloud.PutNextRectangle(sizeMin);

            var thirdRectangle = circularCloud.PutNextRectangle(sizeMin);
            thirdRectangle.Should().Be(new Rectangle(0, 3, 1, 1));
        }        

        [Test]
        public void TwoBigRectangles_IsNotInterspected()
        {
            var bigSize = new Size(100, 100);
            circularCloud.PutNextRectangle(bigSize);
            circularCloud.PutNextRectangle(bigSize);
            circularCloud.Rectangles.AnyIntersected().Should().BeFalse();
        }


        [TestCase(0, TestName = "zero rectangles")]
        [TestCase(1, TestName = "one rectangle")]
        [TestCase(10, TestName = "ten rectangles")]
        [TestCase(100, TestName = "hundried rectangles")]
        public void RectanglesOnSpiral_MustBeTrue(int numberOfPoints)
        {
            var spiral = new ArchimedeanSpiral(startPoint);
            InitializationOfRandomRectangles(200);
            circularCloud.Rectangles.All(rectangle => spiral.CheckBalancedPointOnSpiral(rectangle.Location))
                .Should()
                .BeTrue();
        }

        

        [TestCase(-1, 0, TestName = "negative number and zero in size")]
        [TestCase(-1, -10, TestName = "two negative numbers in size")]
        [TestCase(0, 0, TestName = "two zeros in size")]
        [TestCase(0, 5, TestName = "zero and positive number in size")]
        [TestCase(5, -10, TestName = "negative and positive number in size")]
        public void ZeroOrNegativeNumbersInSize_ThrowsArgumentException(int width, int height)
        {
            Assert.Throws<ArgumentException>(() => circularCloud.PutNextRectangle(new Size(width, height)));
        }

        [TestCase(100, TestName = "hundred of rectangles")]
        [TestCase(200, TestName = "two hundred of rectangles")]
        [TestCase(300, TestName = "three hundred of rectangles")]
        public void BigRandomCloud_IsCircle(int numberOfPoints)
        {
            InitializationOfRandomRectangles(numberOfPoints);

            var sortedByXRectangles = circularCloud.Rectangles.OrderBy(x => x.Location.X);
            var mostLeftPoint = sortedByXRectangles.First().Location;
            var mostRightPoint = sortedByXRectangles.Last().Location;

            var sortedByYRectangles = circularCloud.Rectangles.OrderBy(x => (x.Location.Y));
            var mostTopPoint = sortedByYRectangles.Last().Location;
            var mostBottomPoint = sortedByYRectangles.First().Location;
            
            var points = new [] {mostBottomPoint, mostLeftPoint, mostRightPoint, mostTopPoint};
            var radiuses = points.Select(point => circularCloud.Spiral.Center.GetDistance(point)).ToArray();

            for (int i = 0; i < radiuses.Length; i++)
                for (int j = 0; j < radiuses.Length; j++)
                {
                    Assert.True(radiuses[j] / radiuses[i] > 0.7);
                }
        }
         
    

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                var visualisator = new Visualisator(circularCloud);
                string testName = TestContext.CurrentContext.Test.Name;
                string filePath = Path.Combine(TestContext.CurrentContext.WorkDirectory, "failed_tests_pictures");

                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);

                visualisator.SaveBitmap(Path.Combine(filePath, testName + ".jpg"));
                Console.WriteLine(string.Format("Tag cloud visualization saved to file {0}.jpg", testName));
            }
        }
    }
}

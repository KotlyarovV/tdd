using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    class Visualisator
    {
        private CircularCloudLayouter circularCloud;
        private SolidBrush brush = new SolidBrush(Color.LimeGreen);

        public Visualisator(CircularCloudLayouter circularCloudLayouter)
        {
            circularCloud = circularCloudLayouter;
        }

        public void GenerateRandomRectangles(int numberOfSizes) =>
            (new List<Size>()).AddRandomSizes(numberOfSizes).ForEach(s => circularCloud.PutNextRectangle(s));

        public void SetRectanglesOnGraphics(Graphics graphics) =>
            circularCloud.Rectangles.ForEach(rectangle => graphics.FillRectangle(brush, rectangle)); 

        private Size GetBitmapSize()
        {
            var minX = circularCloud.Rectangles.Min(rectangle => rectangle.X);
            var maxX = circularCloud.Rectangles.Max(rectangle => rectangle.X + rectangle.Width);

            var minY = circularCloud.Rectangles.Min(rectangle => rectangle.Y - rectangle.Height);
            var maxY = circularCloud.Rectangles.Max(rectangle => rectangle.Y);
            
            return new Size(maxX - minX, maxY - minY);
        }

        public void SaveBitmap(string bitmapName)
        {
            var size = GetBitmapSize();
            var bitmap = new Bitmap(size.Width, size.Height);
            var graphics = Graphics.FromImage(bitmap);
            SetRectanglesOnGraphics(graphics);
            
            bitmap.Save(bitmapName);
        }
    }
}

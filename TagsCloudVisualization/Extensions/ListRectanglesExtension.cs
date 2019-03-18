using System.Collections.Generic;
using System.Drawing;
using System.Linq;


namespace TagsCloudVisualization
{
    static class ListRectanglesExtension
    {
        public static bool ContainPoint(this List<Rectangle> rectangles, Point point) =>
            rectangles.Any(r => r.Contains(point));

        public static bool IntersectRectangle(this List<Rectangle> rectangles, Rectangle rectangle) =>
            rectangles.Any(r => r.IntersectsWith(rectangle));

        public static bool AnyIntersected(this List<Rectangle> rectangles) =>
            rectangles.Any(x => rectangles.Any(y => x.IntersectsWith(y) && y != x));
    }
}

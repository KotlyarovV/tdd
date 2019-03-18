using System;
using System.Collections.Generic;
using System.Drawing;


namespace TagsCloudVisualization
{
    static class ListSizeExtension
    {
        private static readonly Random random = new Random();

        public static void AddRandomSize(this List<Size> sizeList, int minValue, int maxValue)
        {
            var x = random.Next(minValue, maxValue);
            var y = random.Next(minValue, maxValue);
            sizeList.Add(new Size(x, y));
        }

        public static List<Size> AddRandomSizes(this List<Size> sizes, int number, int minSize = 10, int maxSize = 60)
        {
            for (var i = 0; i < number; i++)
            {
                sizes.AddRandomSize(minSize, maxSize);
            }
            return sizes;
        }
    }
}

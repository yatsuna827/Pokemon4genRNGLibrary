using System;
using System.Collections.Generic;
using System.Text;

namespace Pokemon4genRNGLibrary
{

    // マップデータをimmutableに保持するだけのクラス.
    public class MapGrid
    {
        private readonly int height, width;
        private readonly bool[,] mapData;

        public bool this[int h, int w]
        {
            get => mapData[h + 4, w + 4];
        }

        // 9x9に切り出す.
        public bool[,] Clip(int center_h, int center_w)
        {
            var grid = new bool[9, 9];

            // (h,w) -> (4,4)になるように取ってくる.
            var h0 = center_h - 4;
            var w0 = center_w - 4;

            var init_h = h0 < 0 ? -h0 : 0;
            var init_w = w0 < 0 ? -w0 : 0;

            for (int h = init_h; h < 9 && h + h0 < height; h++)
            {
                for (int w = init_w; w < 9 && w + w0 < width; w++)
                {
                    grid[h, w] = mapData[h + h0, w + w0];
                }
            }

            return grid;
        }

        public MapGrid(string[] mat)
        {
            (height, width) = (mat.Length, mat[0].Length);
            mapData = new bool[height + 8, width + 8];

            for (int i = 0; i < height; i++)
            {
                for (int k = 0; k < width; k++)
                {
                    mapData[i + 4, k + 4] = mat[i][k] == '*';
                }
            }
        }

    }

    // MapGridに重ねて相対座標を取りやすくするクラス.
    public class GridClipper
    {
        private readonly MapGrid mapGrid;
        public (int h, int w) Point { get; set; }

        public bool this[int h, int w] { get => mapGrid[h + Point.h, w + Point.w]; }
        public bool this[(int h, int w) p] { get => mapGrid[p.h + Point.h, p.w + Point.w]; }
        public bool[,] Clip() => mapGrid.Clip(Point.h, Point.w);

        public GridClipper(MapGrid mapGrid, int init_h = 0, int init_w = 0)
            => (this.mapGrid, this.Point) = (mapGrid, (init_h, init_w));

    }

}

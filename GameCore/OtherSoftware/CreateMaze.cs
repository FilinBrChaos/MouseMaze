using System;
using System.Drawing;

namespace GameParts.GameCore.OtherSoftware
{
    public class CreateMaze
    {
        public static bool[,] ConvertMaze(string path, double step = 14)        //step in px
        {
            //this method create bool[,] by checking every step: if color[index] on image is equal to barrier color [index] = false
            //input maze must have borders at least 1px
            //method take barrier color from the first pixel of image

            Bitmap bitmap = new Bitmap(path);
            bool[,] maze = new bool[(bitmap.Height / (int)step) * 2 + 1, (bitmap.Width / (int)step) * 2 + 1];
            Color border = bitmap.GetPixel(0, 0);
            step /= 2;

            for (int i = 0; i < maze.GetLength(0); i++)
            {
                for (int j = 0; j < maze.GetLength(1); j++)
                {
                    int row = j == 0 ? (int)Math.Round(j * step, 0, MidpointRounding.AwayFromZero) : (int)Math.Round(j * step, 0, MidpointRounding.AwayFromZero) - 1;
                    int column = i == 0 ? (int)Math.Round(i * step, 0, MidpointRounding.AwayFromZero) : (int)Math.Round(i * step, 0, MidpointRounding.AwayFromZero) - 1;

                    if (bitmap.GetPixel(row, column) == border) maze[i, j] = false;
                    else maze[i, j] = true;
                }
            }
            return maze;
        }
    }
}

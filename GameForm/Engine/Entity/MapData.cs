using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IcerDesign.Game.SLG.Engine.Enum;

namespace IcerDesign.Game.SLG.Engine.Entity
{
    internal class MapData
    {
        public byte[,] Data { get; set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public MapData()
        {
            Width = 11;
            Height = 11;
            Data = new byte[Width, Height];
            InitMap();
        }

        public void InitMap()
        {
            for (int i = 0; i < Width; ++i)
            {
                for (int j = 0; j < Height; ++j)
                {
                    Data[i, j] = (int)TerrainType.Beach;
                }
            }

        }

        public bool IsTopTheSame(int x, int y)
        {
            if (y - 1 >= 0)
            {
                if (Data[x, y] == Data[x, y - 1])
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool IsBottomTheSame(int x, int y)
        {
            if (y + 1 < Height)
            {
                if (Data[x, y] == Data[x, y + 1])
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool IsLeftTheSame(int x, int y)
        {
            if (x - 1 >= 0)
            {
                if (Data[x, y] == Data[x - 1, y])
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool IsRightTheSame(int x, int y)
        {
            if (x + 1 < Width)
            {
                if (Data[x, y] == Data[x + 1, y])
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using IcerDesign.Game.SLG.Engine.Enum;

namespace IcerDesign.Game.SLG.Texture
{
    internal class MapTexture : IDisposable
    {
        private readonly string FileName = @"..\..\Image\MP{0}\MP{0}_{1:00}_0.bmp";
        private readonly int TotalMapNumber = 13;

        private List<Bitmap[]>[] texMap;

        public MapTexture(int mapId)
        {
            texMap = new List<Bitmap[]>[TotalMapNumber];
            Bitmap texMapTemp;

            for (int i = 0; i < TotalMapNumber; i++)
            {
                texMapTemp = new Bitmap(string.Format(FileName, mapId, i));
                texMap[i] = new List<Bitmap[]>();
                Bitmap[] bmpTemp = new Bitmap[16];
                for (int x = 0; x < 4; x++)
                    for (int y = 0; y < 4; y++)
                        bmpTemp[x + y * 4] = GetBlockTexture(texMapTemp, x, y);
                texMap[i].Add(bmpTemp);
            }
            Console.WriteLine("地图读取完成！");
        }

        private Bitmap GetBlockTexture(Bitmap sourceTexture, int x, int y)
        {
            return GetBlockTexture(sourceTexture, x, y, 48, 48);
        }

        private Bitmap GetBlockTexture(Bitmap sourceTexture, int x, int y, int width, int height)
        {
            Bitmap tmpBMP = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(tmpBMP);
            g.DrawImage(sourceTexture, new Rectangle(0, 0, width, height), new Rectangle(x * width, y * height, width, height), GraphicsUnit.Pixel);
            g.Flush();
            g.Dispose();
            return tmpBMP;
        }

        internal Image GetMapImage(int mapType, bool isTopSame, bool isRightSame, bool isBottomSame, bool isLeftSame)
        {
            int x = (isTopSame ? 1 : 0) + (isRightSame ? 2 : 0);
            int y = (isBottomSame ? 1 : 0) + (isLeftSame ? 2 : 0);

            return texMap[mapType][0][x + y * 4];
        }

        internal Image GetMapImage(TerrainType terrainType, bool isTopSame, bool isRightSame, bool isBottomSame, bool isLeftSame)
        {
            return GetMapImage((int)terrainType, isTopSame, isRightSame, isBottomSame, isLeftSame);
        }

        internal Image GetMapImage(TerrainType terrainType)
        {
            return GetMapImage((int)terrainType, false, false, false, false);
        }

        #region IDisposable Members

        public void Dispose()
        {
            texMap = null;
            GC.Collect();
        }

        #endregion
    }
}

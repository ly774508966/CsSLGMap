using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IcerDesign.Game.SLG.Texture;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using IcerDesign.Game.SLG.Engine.Entity;
using System.Drawing.Imaging;
using IcerDesign.Game.SLG.Engine.Enum;

namespace IcerDesign.Game.SLG.Engine
{
    internal class GameEngine : IDisposable
    {
        private EngineConfig _config = new EngineConfig();
        private readonly Control _displayer;
        private int gameFrameAnimation = 0;
        private MapTexture _texMap = new MapTexture(0);
        private MapData _mapData = new MapData();
        private Thread _thMainLoop;
        private Point _positionOfMouse;//= new Point(0, 0);

        public GameEngine(Control ctrl)
        {
            _displayer = ctrl;
            _displayer.MouseMove += new MouseEventHandler(Displayer_MouseMove);
        }

        public void Start()
        {
            addMotiveText("欢迎进入冰河魔法师的游戏");
            _thMainLoop = new Thread(gameLoop);
            _thMainLoop.Start();
        }

        public void Stop()
        {
            _config.gameEnd = true;
            _thMainLoop.Abort();
            _thMainLoop.Join();
            _thMainLoop = null;
            _texMap.Dispose();
        }

        private void gameLoop()
        {
            int nextTime = 0;// Time to next update.
            int nextSecond = Environment.TickCount + 1000;// Time to next second.
            int fpsCount = 0;// Current FPS.
            int ticks = Environment.TickCount;// Current time.

            while (!_config.gameEnd) // Go until game ends.
            {
                //while (this. == true)
                //{
                //    System.Windows.Forms.Application.DoEvents();
                //}

                ticks = Environment.TickCount;// Update time.

                if (ticks > nextTime) // Check if game needs update.
                {
                    // Update game.
                    update();
                    gameFrameAnimation = (gameFrameAnimation + 1) % 10000;


                    nextTime = (1000 / _config.gameSpeed) + ticks;// Get next update time.

                    if (ticks > nextSecond) // Check second update.
                    {
                        // Increase elapsed time.
                        //_elapsedTime++;


                        _config.gameFPS = fpsCount;// Ready to display fps.
                        Console.WriteLine("Gaming FPS:" + fpsCount.ToString());

                        fpsCount = 0;// Reset fps.
                        nextSecond = 1000 + ticks;// Get next second.
                    }

                    fpsCount++;// Update fps.

                }
            }
        }

        private void update()
        {
            //int x, y, tx, ty;
            int ta;
            //int i, j;
            float fx, fy;
            Font fnt = new Font("宋体", 10);
            Bitmap bmp = new Bitmap(_config.gameWidth, _config.gameHeight);
            Graphics g = Graphics.FromImage(bmp);
            LinearGradientBrush brush;// = new LinearGradientBrush(new PointF(0.0f, 0.0f), new PointF(60.0f, 60.0f), Color.White, Color.Black);

            g.Clear(Color.White);

            Point pos = GetTilePosition(_positionOfMouse.X - 200, _positionOfMouse.Y);
            g.DrawString(string.Format("X: {0}, Y: {1}", _positionOfMouse.X, _positionOfMouse.Y), fnt, new SolidBrush(Color.Blue), 0, 0);
            g.DrawString(string.Format("X: {0}, Y: {1}", pos.X, pos.Y), fnt, new SolidBrush(Color.Blue), 0, 20);

            // draw map
            for (int i = 1; i < _mapData.Width + _mapData.Height; i++)
            {
                int startNumber = i <= _mapData.Height ? 0 : i - _mapData.Height;
                int endNumber = startNumber > 0 ? _mapData.Width : i;
                for (int x = startNumber; x < endNumber; x++)
                {
                    int y = i - x - 1;
                    ImageAttributes imgAttrib = new ImageAttributes();
                    imgAttrib.SetColorKey(Color.FromArgb(253, 0, 255), Color.FromArgb(253, 0, 255));
                    Image blockImage = _texMap.GetMapImage(_mapData.Data[x, y], _mapData.IsTopTheSame(x, y), _mapData.IsRightTheSame(x, y),
                        _mapData.IsBottomTheSame(x, y), _mapData.IsLeftTheSame(x, y));
                    g.DrawImage(blockImage, new Rectangle(200 + (x - y) * 24, (x + y) * 12, 48, 48), 0, 0, 48, 48, GraphicsUnit.Pixel, imgAttrib);
                }
            }


            // draw object move with mouse
            if (pos.X >= 0 && pos.X < _mapData.Width && pos.Y >= 0 && pos.Y < _mapData.Height)
            {
                Image blockImage = _texMap.GetMapImage(TerrainType.Beach);
                ImageAttributes imgAttrib = new ImageAttributes();
                imgAttrib.SetColorKey(Color.FromArgb(253, 0, 255), Color.FromArgb(253, 0, 255));
                g.DrawImage(blockImage, new Rectangle(200 + (pos.X - pos.Y) * 24, (pos.X + pos.Y) * 12, 48, 48), 0, 0, 48, 48, GraphicsUnit.Pixel, imgAttrib);

            }

            // Draw Motive Text
            if (_config.castText != "")
            {
                fx = (-50 + 10 * _config.castPosition.X) * _config.castPosition.X;
                fy = fx + _config.castPosition.Y;
                ta = -(int)fx * 2;
                ta = ta < 0 ? 0 : (ta > 255 ? 255 : ta);
                fx = 10 + (fx < 0 ? (-fx / 5) : 0);
                fnt = new Font("楷体_GB2312", fx, FontStyle.Bold);
                fx = (_config.gameWidth - fx * _config.castText.Length) / 2;
                drawGlowText(g, _config.castText, fnt, Color.FromArgb(ta, 255, 255, 255), Color.FromArgb(ta, 0, 0, 255), (int)fx, (int)fy);
                _config.castPosition.X += 0.10f;
                if (_config.castPosition.X >= 6f) _config.castText = "";
            }

            //			fnt = new Font("宋体",10);
            //			g.DrawString("FPS:" + gameMode.gameFPS, fnt, new SolidBrush(Color.Black), 400, 0);
            //g.FillRectangle(new SolidBrush(Color.FromArgb(128,128,128,128)) , 0,0,picDay.Width ,picDay.Height);
            //try
            //{
            _displayer.CreateGraphics().DrawImage(bmp, 0, 0);
            //}
            //catch
            //{
            //}

        }

        private Point GetTilePosition(int x, int y)
        {
            int tx = Convert.ToInt32(Math.Floor((double)y / 24)) + Convert.ToInt32(Math.Floor((double)x / 48));
            int ty = Convert.ToInt32(Math.Floor((double)y / 24)) - Convert.ToInt32(Math.Floor((double)x / 48));
            int cx = ((x % 48) + 48) % 48;
            int cy = ((y % 24) + 24) % 24;
            cx -= 24;
            cy -= 12;
            int offsetX = Math.Abs(cx);
            int offsetY = Math.Abs(cy);
            if (offsetX + offsetY * 2 > 24)
            {
                if (cx > 0)
                {
                    if (cy > 0)
                    {
                        tx++;
                    }
                    else
                    {
                        ty--;
                    }
                }
                else
                {
                    if (cy > 0)
                    {
                        ty++;
                    }
                    else
                    {
                        tx--;
                    }
                }
            }
            return new Point(tx - 1, ty - 1);
        }

        private void drawGlowText(Graphics g, string txt, Font fnt, Color glowColor, Color mainColor, int x, int y)
        {
            g.DrawString(txt, fnt, new SolidBrush(glowColor), x - 1, y + 1);
            g.DrawString(txt, fnt, new SolidBrush(glowColor), x - 1, y - 1);
            g.DrawString(txt, fnt, new SolidBrush(glowColor), x + 1, y + 1);
            g.DrawString(txt, fnt, new SolidBrush(glowColor), x + 1, y - 1);
            g.DrawString(txt, fnt, new SolidBrush(glowColor), x, y + 1);
            g.DrawString(txt, fnt, new SolidBrush(glowColor), x, y - 1);
            g.DrawString(txt, fnt, new SolidBrush(glowColor), x + 1, y);
            g.DrawString(txt, fnt, new SolidBrush(glowColor), x - 1, y);
            g.DrawString(txt, fnt, new SolidBrush(mainColor), x, y);

        }

        private void addMotiveText(string txt)
        {
            _config.castText = txt;
            _config.castPosition.X = 0;
            _config.castPosition.Y = _config.gameHeight / 2;
        }

        private void Displayer_MouseMove(object sender, MouseEventArgs e)
        {
            _positionOfMouse.X = e.X;
            _positionOfMouse.Y = e.Y;
            Point pos = GetTilePosition(_positionOfMouse.X - 200, _positionOfMouse.Y);
            if (e.Button == MouseButtons.Left)
            {
                if (pos.X >= 0 && pos.X < _mapData.Width && pos.Y >= 0 && pos.Y < _mapData.Height)
                {
                    _mapData.Data[pos.X, pos.Y] = (int)TerrainType.Pit;
                }
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Stop();
        }

        #endregion
    }
}

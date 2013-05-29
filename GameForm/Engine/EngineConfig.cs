using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;

namespace IcerDesign.Game.SLG.Engine
{
    internal class EngineConfig
    {
        public bool gameEnd;
        public bool gamePause;
        public int gameSpeed;
        public int gameFPS;
        public int gameMouse;
        public int delay;
        public string state;
        public PointF castPosition;
        public string castText;
        public int gameHeight;
        public int gameWidth;
        public Thread gameThread;

        public EngineConfig()
        {
            gameEnd = false;
            gamePause = false;
            gameSpeed = 50;
            gameMouse = 0;
            gameHeight = 500;
            gameWidth = 500;
            delay = 0;
            castPosition = new PointF(0, 0);
            castText = "";
        }
    }
}

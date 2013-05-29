using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IcerDesign.Game.SLG.Engine;

namespace IcerDesign.Game.SLG
{
    public partial class frmGameMain : Form
    {
        private GameEngine _engine;

        public frmGameMain()
        {
            InitializeComponent();
        }

        private void frmGameMain_Load(object sender, EventArgs e)
        {
            this.Top = Screen.PrimaryScreen.WorkingArea.Height - this.Height;
            this.Left = Screen.PrimaryScreen.WorkingArea.Width - this.Width;
            _engine = new GameEngine(picMain);
            _engine.Start();
        }

        private void frmGameMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            _engine.Dispose();
        }
    }
}

using System;
using System.Drawing;

namespace Pocket1945
{
    public class GameFont : IDrawable
    {
        #region Fields

        private Brush _brush = new SolidBrush(Color.Red);
        private Font _font = new Font("Tahoma", 10, FontStyle.Bold);
        private string _text = string.Empty;
        private int _x = 10;
        private int _y = 10;

        #endregion

        #region Properties

        public Font Font
        {
            get { return _font; }
            set { _font = value; }
        }

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public int X
        {
            get { return _x; }
            set { _x = value; }
        }

        public int Y
        {
            get { return _y; }
            set { _y = value; }
        }

        #endregion

        #region Constructor

        public GameFont()
        {
        }

        public GameFont(Color color)
        {
            _brush = new SolidBrush(color);
        }

        public GameFont(Color color, string text)
        {
            _brush = new SolidBrush(color);
            _text = text;
        }

        public GameFont(Color color, string text, Font font)
        {
            _brush = new SolidBrush(color);
            _font = font;
            _text = text;
        }

        public GameFont(Color color, string text, Font font, int x, int y)
        {
            _brush = new SolidBrush(color);
            _font = font;
            _text = text;
            _x = x;
            _y = y;
        }

        #endregion

        public void Draw(Graphics g)
        {
            g.DrawString(_text, _font, _brush, _x, _y);
        }
    }
}
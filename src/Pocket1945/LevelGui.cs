using System;
using System.Drawing;

namespace Pocket1945
{
    /// <summary>
    /// This class is the level graphical user interface. The class
    /// gives the user information about current score, current level,
    /// number of lives etc.
    /// </summary>
    public class LevelGui : IDrawable
    {
        private Font font;
        private Brush brush;
        private string text;

        public LevelGui()
        {
            brush = new SolidBrush(Color.Yellow);
            font = new Font(FontFamily.GenericSansSerif, 8, FontStyle.Bold);
        }

        public void Draw(Graphics g)
        {
            text = string.Format("Score: {0}\r\nHealth: {1}%\r\nLives: {2}",
                GameForm.Player.Score,
                GameForm.Player.Health,
                GameForm.Player.Lives);
            g.DrawString(text, font, brush, 7, 7);

            text = string.Format("Level: {0} - Progress: {1}%", GameForm.LevelCount + 1,
                GameForm.CurrentLevel.GetProgress());
            g.DrawString(text, font, brush, 7, GameForm.GameArea.Height - 16);
        }
    }
}
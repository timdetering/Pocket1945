using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Pocket1945
{
    /// <summary>
    /// This class draws the background of the game.
    /// </summary>
    public class Background : IDrawable
    {
        /// <summary>
        /// The current Y position of the map. 0 is at the end of the map.
        /// </summary>
        public int Y;

        /// <summary>
        /// The total height of the map. Used to messure progress.
        /// </summary>
        public int Height;

        /// <summary>
        /// The speed the map is scrolling at.
        /// </summary>
        public double Speed;

        private double y;
        private Rectangle focusRectangle;
        private Bitmap background;
        private ImageAttributes imgAttribs;

        /// <summary>
        /// Instanciates a new background.
        /// </summary>
        /// <param name="map">The map to draw for the level.</param>
        /// <param name="backgroundElements">The backgroundelements to draw for the level.</param>
        public Background(string[] map, BackgroundElement[] backgroundElements)
        {
            int spriteWidth = 32;
            y = (map.Length * spriteWidth) - GameForm.GameArea.Height;
            focusRectangle = new Rectangle(0, map.Length * spriteWidth, GameForm.GameArea.Width,
                GameForm.GameArea.Height);
            Height = focusRectangle.Y - GameForm.GameArea.Height;
            background = new Bitmap(GameForm.GameArea.Width, map.Length * spriteWidth);
            Graphics g = Graphics.FromImage(background);

            imgAttribs = new ImageAttributes();
            imgAttribs.SetColorKey(Color.FromArgb(0, 66, 173), Color.FromArgb(0, 66, 173));

            //Loop trough all map strings.
            for (int i = 0; i < map.Length; ++i)
            {
                //Create a char array of all map elements.
                char[] mapElements = map[i].ToCharArray();

                //Loop trough all chars in the map string.
                for (int j = 0; j < mapElements.Length; ++j)
                {
                    //Drawnig the map element to the map bitmap.
                    int index = SpriteList.Instance.GetTileIndex(mapElements[j]);
                    int n = SpriteList.Instance.Tiles.Length;
                    g.DrawImage(SpriteList.Instance.Tiles[index], j * spriteWidth, i * spriteWidth);
                }
            }

            //Loop trough all background elements.
            for (int i = 0; i < backgroundElements.Length; ++i)
            {
                BackgroundElement e = backgroundElements[i];

                //Draw the map element to the map bitmap.
                if (e.Width > 32)
                    g.DrawImage(SpriteList.Instance.BigBackgroundElements[e.SpriteIndex],
                        new Rectangle(e.X, e.Y, e.Width, e.Height), 0, 0, e.Width, e.Height, GraphicsUnit.Pixel,
                        imgAttribs);
            }

            //Dispose the graphics element used to draw the background.
            g.Dispose();
        }

        /// <summary>
        /// Implemention of IDrawable interface making the background able to draw itself.
        /// </summary>
        /// <param name="g">The graphics object used to draw the background.</param>
        public void Draw(Graphics g)
        {
            //Scrolling the background and increasing the y co-ordinate.
            if (y > 0)
            {
                y -= Speed;
                focusRectangle.Y = (int)y;
                Y = focusRectangle.Y;
            }

            //Drawing the active part of the map to the back buffer bitmap.
            g.DrawImage(background, GameForm.GameArea, focusRectangle, GraphicsUnit.Pixel);
        }
    }
}
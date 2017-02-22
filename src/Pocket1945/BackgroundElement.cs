using System;

namespace Pocket1945
{
    /// <summary>
    /// This structure represents the simples type of 
    /// sprite/graphical element. It is a sprite element that get's drawn 
    /// to the map. The reason there background elements isn't added to the ASCII map
    /// is the fact that wee need to use color keys to get transparency etc.
    /// 
    /// A typical background element can be a island, a house or anything that's not "moving", 
    /// "shooting" or "colliding".
    /// </summary>
    public struct BackgroundElement
    {
        /// <summary>
        /// X position of the background element.
        /// </summary>
        public int X;

        /// <summary>
        /// Y position of the background element.
        /// </summary>
        public int Y;

        /// <summary>
        /// Height of the background element.
        /// </summary>
        public int Height;

        /// <summary>
        /// Width of the background element.
        /// </summary>
        public int Width;

        /// <summary>
        /// Sprite index of the background element.
        /// </summary>
        public int SpriteIndex;

        /// <summary>
        /// Instanciating a new background element.
        /// </summary>
        /// <param name="x">The x position of the background element.</param>
        /// <param name="y">The y position of the background element.</param>
        /// <param name="height">The height of the background element.</param>
        /// <param name="width">The width of the background element.</param>
        /// <param name="spriteIndex">The spriteindex of the background element.</param>
        public BackgroundElement(int x, int y, int height, int width, int spriteIndex)
        {
            X = x;
            Y = y;
            Height = height;
            Width = width;
            SpriteIndex = spriteIndex;
        }
    }
}
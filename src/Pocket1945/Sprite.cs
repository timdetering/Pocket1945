using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Pocket1945
{
	/// <summary>
	/// This class is a base sprite class. All game objects like 
	/// enemy planes, background elements etc. inherit this class.
	/// The class implements IDrawable since all sprites must be able to 
	/// be drawn on the screen.	
	/// </summary>
	public abstract class Sprite : IDrawable
	{							
		protected int x;
		protected int y;
		protected Size spriteSize;		
		protected Point collitionPoint;
		protected Rectangle collitionRectangle;
		protected ImageAttributes imgAttribs;

		/// <summary>
		/// Abstract method all child classes must override. This 
		/// method returns the index of the sprite from the SpritList 
		/// bitmap array.
		/// </summary>
		/// <returns>The index to use to access the SpriteList bitmap array.</returns>
		public abstract int GetSpriteIndex();
				
		/// <summary>
		/// Method returning true if the sprite is currently off the screen.
		/// The method is virtual so that child classess can make it's own implementation
		/// of the method.
		/// </summary>
		/// <returns>True if the sprite is off the screen.</returns>
		public virtual bool IsOffScreen()
		{
			return(x > GameForm.GameArea.Width || x + spriteSize.Width < 0 || y > GameForm.GameArea.Height || y + spriteSize.Height < 0);
		}

		/// <summary>
		/// Method returning the collition bounds of the sprite.
		/// </summary>
		/// <returns>A rectangle representing the collition area of the sprite.</returns>
		public virtual Rectangle GetCollitionRectangle()
		{			
			collitionRectangle.X = x + collitionPoint.X;
			collitionRectangle.Y = y + collitionPoint.Y;
			return collitionRectangle;
		}

		/// <summary>
		/// Implementation of IDrawable. This method draws the sprite.
		/// </summary>
		/// <param name="g">The graphics object the sprite is using to draw itself.</param>
		public abstract void Draw(Graphics g);

		/// <summary>
		/// Instanciates a new Sprite class.
		/// </summary>
		public Sprite()
		{
			x = 0;
			y = 0;	
			spriteSize = new Size(0, 0);
			collitionPoint = new Point(0, 0);
			collitionRectangle = new Rectangle(x, y, spriteSize.Width, spriteSize.Height);
			
			imgAttribs = new ImageAttributes();			
			imgAttribs.SetColorKey(Color.FromArgb(0, 66, 173), Color.FromArgb(0, 66, 173));
		}
	}
}
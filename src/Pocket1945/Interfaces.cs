using System;
using System.Drawing;

namespace Pocket1945
{
	/// <summary>
	/// This interface is implemented by all game classes
	/// that can be drawn to the screen. The interface has only 
	/// one method passing in a graphics object which the class should 
	/// use to draw itself.
	/// </summary>
	public interface IDrawable
	{
		/// <summary>
		/// Method drawing the game object to the screen.
		/// </summary>
		/// <param name="g">The graphics object used to draw the sprite.</param>
		void Draw(Graphics g);
	}

	/// <summary>
	/// This interface is implementet by all game classes that
	/// are armed. The interface contains methods to check if the item 
	/// can fire a bullet, a method to fire a new bullet, and a method to 
	/// act on a bullet hit.
	/// </summary>
	public interface IArmed
	{
		/// <summary>
		/// Method checking if the game object can fire.
		/// </summary>
		/// <returns>True if the object can fire a new bullet.</returns>
		bool CanFire();

		/// <summary>
		/// Method executing a new bullet hit.
		/// </summary>
		/// <param name="b">The bullet hitting the game object.</param>
		void Hit(Bullet b);

		/// <summary>
		/// Method fiering a new bullet.
		/// </summary>
		/// <returns>Returns a new bullet.</returns>
		Bullet Fire();
	}

	/// <summary>
	/// This interface is implemented by all game objects that you can collide 
	/// into. Examples of collidable objects are the player, the enemies and bonuses.
	/// The method contains one method returning a rectangle defining the collition area.
	/// </summary>
	public interface ICollidable
	{
		/// <summary>
		/// Method returning the collition area of the game object.
		/// </summary>
		/// <returns>A rectangle definging the collition area of the game object.</returns>
		Rectangle GetCollitionRectangle();
	}
}
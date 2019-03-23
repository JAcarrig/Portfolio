using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SlowCanterToTheRight
{
    // The Platform class is simply a hitbox that is used to determine whether or not an enemy is in contact with one.
    // It is evaluated similarly to the yBound in the Player class Update method
    class Platform : GameObject
    {
        // Fields
        protected Rectangle platformCollisionbox;
        protected int collisionHeight;

        // Properties
        public Rectangle PlatformCollision
        {
            get { return platformCollisionbox; }
        }

        // Constructor
        public Platform(Rectangle position, Texture2D sprite) : base(position, sprite)
        {
            // The collision height is the depth for which the player is allowed to connect with the platform with
            collisionHeight = 25;

            // The platformCollisionbox is equal to the width of the platform and the set collision height
            platformCollisionbox = new Rectangle(posX, posY, Width, collisionHeight);

            // The Platform class really only exists so that the 
            // player can tell if it is standing on top of one
            // and act accordingly based upon the platform's position
        }
    }
}

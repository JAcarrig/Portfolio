using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SlowCanterToTheRight
{
    // The MoneyBomb class is a container of data for the boss's projectiles
    // They store a position and will inflict damage upon player contact
    class MoneyBomb : GameObject
    {
        //Fields
        private int damage;

        //Constructor
        public MoneyBomb(Rectangle position, Texture2D sprite)
            : base(position, sprite)
        {
            damage = 1;
        }

        // Methods

        // Do Damage
        // When the bombs collide with the player, they exploed and impart damage to the player.
        // They set their collided value to true so that they can spawn an explosion particle and
        // be removed from the game
        public bool DoDamage(Player nacho)
        {
            bool collided = false;

            if (position.Intersects(nacho.Position) == true && nacho.IsHurt == false)
            {
                nacho.Health -= damage;
                nacho.IsHurt = true;
                collided = true;
            }

            return collided;
        }
    }
}

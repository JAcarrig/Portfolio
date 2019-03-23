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

    class Character : GameObject
    {
        //Fields

        // Health
        protected int health;
        public int Health { get { return health; } set { health = value; } }

        // xSpeed
        protected int xSpeed;
        public int XSpeed { get { return xSpeed; } set { xSpeed = value; } }

        // xSpeed
        protected int ySpeed;
        public int YSpeed { get { return ySpeed; } set { ySpeed = value; } }

        // Damage
        protected int damage;
        public int Damage { get { return damage; } set { damage = value; } }


        //Constructor
        public Character(int health, int xSpeed, int ySpeed, int damage, Rectangle position, Texture2D sprite) : base(position, sprite)
        {
            this.health = health;
            this.xSpeed = xSpeed;
            this.ySpeed = ySpeed;
            this.damage = damage;
        }
    }
}

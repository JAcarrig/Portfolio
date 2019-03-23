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
    class Bomb : GameObject
    {
        // Fields
        private Rectangle Brect; // positional rectangle
        private Texture2D Bsprite; // bomb sprite

        private bool isExploding; // determines if the bomb is exploding now
        private Particle blastParticle;
        private Rectangle explosionHitbox; // explosion radius- enemies inside this box will be damaged by the bomb

        private int xSpeed;
        private int ySpeed;
        private float rot; 

        // Properties
        public bool IsExploding
        {
            get { return isExploding; }
            set { isExploding = value; }
        }

        public Rectangle ExplosionHitbox
        {
            get { return explosionHitbox; }
        }

        public int YSpeed
        {
            get { return ySpeed; }
        }

        // Constructor
        public Bomb(Texture2D sprite, Rectangle Bpos, int xSpeed) : base(Bpos, sprite)
        {
            Bsprite = sprite;
            Brect = Bpos;

            isExploding = false;

            this.xSpeed = xSpeed;
            ySpeed = -30;
            rot = 0f;
        }

        /// <summary>
        /// Handles the bomb's explosions.
        /// </summary>
        public void UpdateBomb()
        {
            if(isExploding == false) // calculate non-exploding bombs movement
            {
                posX += xSpeed;
                posY += ySpeed;

                ySpeed += 1;

                if (posY >= 800) // trigger explosion when bomb hits ground
                {
                    isExploding = true;
                }

                explosionHitbox = new Rectangle(posX - sprite.Width, posY - sprite.Height, Width * 3, Height * 3); // creates bomb explosion hitbox
            }
        }

        /// <summary>
        /// The standard draw method.
        /// </summary>
        /// <param name="sb"></param>
        public void DrawBomb(SpriteBatch sb)
        {
            sb.Draw(sprite, new Vector2(this.posX, this.posY),
                    new Rectangle(0, 0, this.Width, this.Height),
                    Color.White, rot, new Vector2(0, 0), 1,
                    SpriteEffects.FlipHorizontally, 1);
        }
    }
}

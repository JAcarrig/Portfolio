using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace SlowCanterToTheRight
{
    // Initiates the states used for both Enemy objects
    public enum EnemyState
    {
        spawn = 0,
        facingLeft = 1,
        facingRight = 2,
        scout = 3,
        pursue = 4,
        dying = 5,
        hitLeft = 6,
        hitRight = 7,
        dead = 8,
        atkRight = 9,
        atkLeft = 10
    }

    // The enemy class represents a ground enemy object. It controls the ground enemy's individual fields,
    // its state updates, its animation, and drawing it to the screen. It inherits from the Charater class 
    // meaning it has X and Y speeds, health, and a damage value
    class Enemy : Character
    {
        // Direction
        protected EnemyState enemyState;
        public EnemyState EnemyState { get { return enemyState; } set { enemyState = value; } }

        protected EnemyState prevEnemyState;
        public EnemyState PrevEnemyState { get { return prevEnemyState; } set { prevEnemyState = value; } }

        private SoundEffectInstance Will;
        private bool played;

        protected float scl;

        private int initialXSpeed;
        private int topSpeed;

        // Values for drawing the enemy from the sprite sheet
        private int groundEnemyFrameX;
        private double groundEnemyTimeCounter;
        private double groundEnemyFPS;                      // Instantiate these in the constructor
        private double groundEnemyTimePerFrame;             // ---

        // Constants for Sprite Sheet
        private const int GroundEnemyFrameCountX = 7; // Determines how many frames the animation is ( the number of sprites it runs through before going back to the first one)
        private const int GroundEnemyRectWidth = 200; // Width of a single frame (you get this from counting the width between each sprite)
        private const int GroundEnemyRectHeight = 275; // Height of a single frame (You get this from counting the height of the sprite + a little bit of extra room)

        // Hitbox Property
        public Rectangle EnemyHitbox
        {
            get { return position; }
        }

        // Bounds
        // Represent the walls of the map
        protected int xBound;
        protected int yBound;

        public Enemy(int health, int xSpeed, int ySpeed, int damage, Rectangle position, Texture2D sprite, SoundEffectInstance will) 
            : base(health, xSpeed, ySpeed, damage, position, sprite)
        {
            // Ground enemies begin in the spawn state
            enemyState = EnemyState.spawn;

            // Sets the map bounds
            xBound = 3200;
            yBound = 800;

            Will = will;
            played = false;

            this.xSpeed = xSpeed;
            this.ySpeed = -40;
            scl = 0;

            // Initial speed passed in. Top speed is double the initial
            initialXSpeed = xSpeed;
            topSpeed = initialXSpeed * 2;

            // Controls the FPS of the animation
            groundEnemyFPS = 15;
            groundEnemyTimePerFrame = 1.0 / groundEnemyFPS;
        }

        //Methods
        // Updates the enemy based upon its and the player's position
        public virtual void EnemyUpdate(GameTime gameTime, Player player, Random rng)
        {
            switch (enemyState)
            {
                // In the spawn state, the ground enemies shoot from the ground and become active when they reach the ground
                case EnemyState.spawn:
                    position.Y += ySpeed;

                    // Downward acceleration
                    ySpeed += 2;

                    // The scale of the enemy increases to 1 as it spawns
                    if (scl < 1)
                    {
                        scl += .03f;
                    }

                    // Makes sure the enemy spawns facing the right direction with the correct based upon its spawn location
                    if (position.Y >= yBound - sprite.Height && ySpeed >= 0)
                    {
                        if(position.X < 1600)
                        {
                            xSpeed = Math.Abs(xSpeed);
                            ySpeed = 0;
                            enemyState = EnemyState.facingRight;
                        }
                        else
                        {
                            xSpeed *= -1;
                            ySpeed = 0;
                            enemyState = EnemyState.facingLeft;
                        }
                    }

                    break;

                case EnemyState.facingLeft:
                    // Translates the enemy by its speed in both dimensions
                    posX += xSpeed;
                    posY += ySpeed;

                    // When the enemy has reached the ground. Its ySpeed becomes 0
                    if (posY < yBound - Height)
                    {
                        ySpeed++;
                    }
                    else
                    {
                        posY = yBound - Height;
                    }

                    // Keep track of previous state to properly calculate knockback when hit
                    prevEnemyState = enemyState;

                    // Checks the walls to turn around
                    if (posX <= 0)
                    {
                        posX = 0;

                        xSpeed = Math.Abs(xSpeed);
                        enemyState = EnemyState.facingRight;
                    }

                    if(posX >= xBound - Width)
                    {
                        posX = xBound - Width;
                        xSpeed = -initialXSpeed;
                    }

                    // When the ground enemy is close enough to the player, it increases its speed up to a maximum
                    if ((player.posX + player.Width / 2) - (posX + Width / 2) > -700 &&
                        (player.posX + player.Width / 2) - (posX + Width / 2) <= 0)
                    {
                        if(rng.Next(2) == 0 && posY >= yBound - Height)
                        {
                            ySpeed = -10;
                        }

                        if (xSpeed > -topSpeed)
                        {
                            xSpeed--;
                        }
                    }
                    else if (xSpeed < -initialXSpeed)
                    {
                        xSpeed++;
                    }

                    break;

                // The facingRight state is similar to the facing left state, with value reversed and alter to account
                // for the different orientation.
                // Comments for the facingLeft state apply here
                case EnemyState.facingRight:
                    posX += xSpeed;
                    posY += ySpeed;

                    if (posY < yBound - Height)
                    {
                        ySpeed++;
                    }
                    else
                    {
                        posY = yBound - Height;
                    }

                    prevEnemyState = enemyState;

                    if (posX >= xBound - Width)
                    {
                        posX = xBound - Width;

                        xSpeed = -initialXSpeed;
                        enemyState = EnemyState.facingLeft;
                    }

                    if ((player.posX + player.Width / 2) - (posX + Width / 2) < 700 &&
                        (player.posX + player.Width / 2) - (posX + Width / 2) >= 0)
                    {
                        if (rng.Next(2) == 0 && posY >= yBound - Height)
                        {
                            ySpeed = -10;
                        }

                        if (xSpeed < topSpeed)
                        {
                            xSpeed++;
                        }
                    }
                    else if(xSpeed > initialXSpeed)
                    {
                        xSpeed--;
                    }

                    break;

                case EnemyState.dying:
                    // When the enemy is dying, it flys back based upon its current speed
                    position.X += xSpeed;
                    position.Y += ySpeed;

                    // Death sound
                    if(played == false)
                    {
                        Will.Play();
                        played = true;
                    }

                    // Y acceleration
                    ySpeed += 2;

                    // When the enemy is no longer visible, it enters the dead state and is ultimately
                    // removed from the enemy list
                    if(position.Y >= yBound)
                    {
                        enemyState = EnemyState.dead;
                    }

                    break;

                    // The hitLeft state knocks the enemy back based upon the values imparted
                    // by the player during the fatal attack. When the enemy's Y position reaches the floor,
                    // it enters a facing state so that it can be hit again
                case EnemyState.hitLeft:
                    position.X += xSpeed;
                    position.Y += ySpeed;

                    ySpeed += 2;

                    if (position.X <= 0)
                    {
                        posX = 0;
                    }

                    if (position.Y >= yBound - Height)
                    {
                        xSpeed = 5;
                        enemyState = EnemyState.facingRight;
                    }

                    break;

                    // The hitRight state works very similarly to the hitLeft state, with reversed values
                    // based upon direction
                case EnemyState.hitRight:
                    position.X += xSpeed;
                    position.Y += ySpeed;

                    ySpeed += 2;

                    if (position.X >= 3200 - Width)
                    {
                        posX = 3200 - Width;
                    }

                    if (position.Y >= yBound - Height)
                    {
                        xSpeed = 5;
                        enemyState = EnemyState.facingLeft;
                    }

                    break;
            }

            // Animates the enemy
            GroundEnemyAnimation(gameTime);
        }

        // Changes the positon of the sprite sheet source rectangle based upon the gameTime
        public void GroundEnemyAnimation(GameTime gameTime)
        {
            groundEnemyTimeCounter += gameTime.ElapsedGameTime.TotalSeconds;
            if (groundEnemyTimeCounter >= groundEnemyTimePerFrame)
            {
                groundEnemyFrameX++;

                groundEnemyTimeCounter -= groundEnemyTimePerFrame;

                if (groundEnemyFrameX > GroundEnemyFrameCountX - 1)
                {
                    groundEnemyFrameX = 0;
                }
            }
        }

        // Draws the Enemies based upon their current animation frame and direction
        public override void Draw(SpriteBatch sb)
        {

            switch (enemyState)
            {
                case EnemyState.spawn:

                    sb.Draw(
                        sprite,
                        new Vector2(this.posX, this.posY),
                        new Rectangle(
                            groundEnemyFrameX * GroundEnemyRectWidth,
                            0,
                            GroundEnemyRectWidth,
                            GroundEnemyRectHeight),
                        Color.White,
                        0,
                        Vector2.Zero,
                        scl,
                        SpriteEffects.None,
                        0);

                    break;

                case EnemyState.facingLeft:

                    sb.Draw(
                        sprite,
                        new Vector2(this.posX, this.posY),
                        new Rectangle(
                            groundEnemyFrameX * GroundEnemyRectWidth,
                            0,
                            GroundEnemyRectWidth,
                            GroundEnemyRectHeight),
                        Color.White,
                        0,
                        Vector2.Zero,
                        scl,
                        SpriteEffects.None,
                        0);

                    break;

                case EnemyState.facingRight:

                    sb.Draw(
                        sprite,
                        new Vector2(this.posX, this.posY),
                        new Rectangle(
                            groundEnemyFrameX * GroundEnemyRectWidth,
                            0,
                            GroundEnemyRectWidth,
                            GroundEnemyRectHeight),
                        Color.White,
                        0,
                        Vector2.Zero,
                        scl,
                        SpriteEffects.FlipHorizontally,
                        0);

                    break;

                case EnemyState.dying:

                    sb.Draw(
                        sprite,
                        new Vector2(this.posX, this.posY),
                        new Rectangle(
                            groundEnemyFrameX * GroundEnemyRectWidth,
                            0,
                            GroundEnemyRectWidth,
                            GroundEnemyRectHeight),
                        Color.White,
                        0,
                        Vector2.Zero,
                        scl,
                        SpriteEffects.FlipHorizontally,
                        0);

                    break;

                case EnemyState.dead:

                    sb.Draw(
                        sprite,
                        new Vector2(this.posX, this.posY),
                        new Rectangle(
                            groundEnemyFrameX * GroundEnemyRectWidth,
                            0,
                            GroundEnemyRectWidth,
                            GroundEnemyRectHeight),
                        Color.White,
                        0,
                        Vector2.Zero,
                        scl,
                        SpriteEffects.FlipHorizontally,
                        0);

                    break;

                case EnemyState.hitLeft:

                    sb.Draw(
                        sprite,
                        new Vector2(this.posX, this.posY),
                        new Rectangle(
                            groundEnemyFrameX * GroundEnemyRectWidth,
                            0,
                            GroundEnemyRectWidth,
                            GroundEnemyRectHeight),
                        Color.White,
                        0,
                        Vector2.Zero,
                        scl,
                        SpriteEffects.FlipHorizontally,
                        0);

                    break;

                case EnemyState.hitRight:

                    sb.Draw(
                        sprite,
                        new Vector2(this.posX, this.posY),
                        new Rectangle(
                            groundEnemyFrameX * GroundEnemyRectWidth,
                            0,
                            GroundEnemyRectWidth,
                            GroundEnemyRectHeight),
                        Color.White,
                        0,
                        Vector2.Zero,
                        scl,
                        SpriteEffects.None,
                        0);

                    break;
            }
        }
    }
}

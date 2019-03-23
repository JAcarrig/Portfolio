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
    class FlyingEnemy : Enemy
    {
        // Fields
        private bool direction;
        private int initialY;
        private int initialXSpeed;
        private bool played;
        private Texture2D smoke;

        private float rot;

        // Values for drawing the enemy from the sprite sheet
        private int flyingEnemyFrameX;
        private int flyingEnemyFrameY;
        private double flyingEnemyTimeCounter;
        private double flyingEnemyFPS;                      // Instantiate these in the constructor
        private double flyingEnemyTimePerFrame;             // ---

        // Constants for Sprite Sheet
        private const int FlyingEnemyFrameCountY = 7; // Determines how many frames the animation is ( the number of sprites it runs through before going back to the first one)
        private const int FlyingEnemyFrameRectWidth = 350; // Width of a single frame (you get this from counting the width between each sprite)
        private const int FlyingEnemyFrameRectHeight = 240; // Height of a single frame (You get this from counting the height of the sprite + a little bit of extra room)

        private SoundEffectInstance Flysould; //sound played when enemy attacks
        private SoundEffectInstance DieS; // sound played when enemy dies

        private List<Rectangle> CloudList;

        // Constructor
        public FlyingEnemy(int health, int xSpeed, int ySpeed, int damage, Rectangle position, Texture2D sprite, SoundEffectInstance flysould, SoundEffectInstance Die, Texture2D Smoke)
            : base(health, xSpeed, ySpeed, damage, position, sprite, Die)
        {
            enemyState = EnemyState.spawn;

            xBound = 3200;
            yBound = 800;

            smoke = Smoke;

            this.xSpeed = 3;
            this.ySpeed = 5;
            scl = 0;
            rot = 0;

            CloudList = new List<Rectangle>();

            played = false;
            Flysould = flysould;
            DieS = Die;

            initialY = posY;
            initialXSpeed = xSpeed;

            flyingEnemyFPS = 10;
            flyingEnemyTimePerFrame = 1.0 / flyingEnemyFPS;

            // True is moving right, false is moving left
            if (posX < 0)
            {
                direction = true;
            }
            else
            {
                direction = false;
            }
        }

        // Methods
        /// <summary>
        /// Handles the current enemy state and switches
        /// as necessary. 
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="player"></param>
        /// <param name="rng"></param>
        public override void EnemyUpdate(GameTime gameTime, Player player, Random rng)
        {
            //Handles switching between the different states
            switch (enemyState)
            {
                //The spawn state
                case EnemyState.spawn:
                    if (scl < 1)
                    {
                        scl += .03f;
                    }
                    else
                    {
                        enemyState = EnemyState.scout;
                    }

                    break;

                //The scout state
                case EnemyState.scout:

                    if (direction)
                    {
                        xSpeed = initialXSpeed;
                    }
                    else
                    {
                        xSpeed = -initialXSpeed;
                    }

                    posX += xSpeed;

                    if (Math.Abs((player.posX + player.Width / 2) - (posX + Width / 2)) < 300)
                    {
                        Flysould.Play();

                        ySpeed = 20;
                        xSpeed *= 3;

                        enemyState = EnemyState.pursue;
                    }

                    if (position.X <= 0)
                    {
                        position.X = 0;
                        xSpeed = 5;
                        direction = true;
                    }
                    else if (position.X >= xBound - this.Width)
                    {
                        position.X = xBound - this.Width;
                        xSpeed = -5;
                        direction = false;
                    }

                    break;

                //The pursue state
                case EnemyState.pursue:
                    posX += xSpeed;
                    posY += ySpeed;

                    ySpeed -= 1;

                    if (position.X <= 0)
                    {
                        position.X = 0;
                        xSpeed = 5;
                        direction = true;
                    }
                    else if (position.X >= xBound - this.Width)
                    {
                        position.X = xBound - this.Width;
                        xSpeed = -5;
                        direction = false;
                    }

                    if (posY <= initialY)
                    {
                        posY = initialY;
                        xSpeed /= 3;
                        enemyState = EnemyState.scout;
                    }

                    break;

                //The hit left state
                case EnemyState.hitLeft:
                    posX += xSpeed;

                    xSpeed += 2;

                    if (position.X <= 0)
                    {
                        posX = 0;
                    }

                    if (xSpeed >= 0)
                    {
                        enemyState = EnemyState.scout;
                    }

                    break;

                //The hit right state
                case EnemyState.hitRight:
                    posX += xSpeed;

                    xSpeed -= 2;

                    if (position.X >= 3200 - Width)
                    {
                        posX = 3200 - Width;
                    }

                    if (xSpeed <= 0)
                    {
                        enemyState = EnemyState.scout;
                    }

                    break;

                //The dying state
                case EnemyState.dying:
                    position.X += xSpeed;
                    position.Y += ySpeed;

                    Random cspawn = new Random();
                    int spawn = cspawn.Next(1, 10);
                    if (spawn <= 2)
                    { 

                    CloudList.Add(new Rectangle(position.X, position.Y, 100, 100));
                    }
                    UpdateCloud();

                    if (played == false)
                    { 
                        DieS.Play();
                        played = true;
                    }

                    rot += .1f;

                    ySpeed += 1;

                    if (position.Y >= yBound + Height)
                    {
                        enemyState = EnemyState.dead;
                    }

                    break;
            }

            //Updates the flying enemy's animation
            FlyingEnemyAnimation(gameTime);
        }

        /// <summary>
        /// Handles the animations for the flying enemy.
        /// </summary>
        /// <param name="gameTime"></param>
        public void FlyingEnemyAnimation(GameTime gameTime)
        {
            flyingEnemyTimeCounter += gameTime.ElapsedGameTime.TotalSeconds;
            if (flyingEnemyTimeCounter >= flyingEnemyTimePerFrame)
            {
                flyingEnemyFrameY++;

                flyingEnemyTimeCounter -= flyingEnemyTimePerFrame;

                if (flyingEnemyFrameY > FlyingEnemyFrameCountY - 1)
                {
                    flyingEnemyFrameY = 0;
                }
            }
        }

        //Draw
        /// <summary>
        /// Draws the enemy based on which state it is in and
        /// which animation frame it is.
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(SpriteBatch sb)
        {
            //Determines which state the flying enemy is in
            //and draws accordingly.
            switch (enemyState)
            {
                //The spawn state
                case EnemyState.spawn:

                    sb.Draw(
                        sprite,
                        new Vector2(this.posX, this.posY),
                        new Rectangle(
                            0,
                            flyingEnemyFrameY * FlyingEnemyFrameRectHeight,
                            FlyingEnemyFrameRectWidth,
                            FlyingEnemyFrameRectHeight),
                        Color.White,
                        rot,
                        Vector2.Zero,
                        scl,
                        SpriteEffects.None,
                        0);

                    break;
                
                    //The scout state
                case EnemyState.scout:
                    if (direction)
                    {
                        sb.Draw(
                        sprite,
                        new Vector2(this.posX, this.posY),
                        new Rectangle(
                            0,
                            flyingEnemyFrameY * FlyingEnemyFrameRectHeight,
                            FlyingEnemyFrameRectWidth,
                            FlyingEnemyFrameRectHeight),
                        Color.White,
                        rot,
                        Vector2.Zero,
                        scl,
                        SpriteEffects.FlipHorizontally,
                        0);
                    }
                    else
                    {
                        sb.Draw(
                        sprite,
                        new Vector2(this.posX, this.posY),
                        new Rectangle(
                            0,
                            flyingEnemyFrameY * FlyingEnemyFrameRectHeight,
                            FlyingEnemyFrameRectWidth,
                            FlyingEnemyFrameRectHeight),
                        Color.White,
                        rot,
                        Vector2.Zero,
                        scl,
                        SpriteEffects.None,
                        0);
                    }

                    break;

                //The pursue state
                case EnemyState.pursue:
                    if (direction)
                    {
                        sb.Draw(
                        sprite,
                        new Vector2(this.posX, this.posY),
                        new Rectangle(
                            0,
                            flyingEnemyFrameY * FlyingEnemyFrameRectHeight,
                            FlyingEnemyFrameRectWidth,
                            FlyingEnemyFrameRectHeight),
                        Color.White,
                        rot,
                        Vector2.Zero,
                        scl,
                        SpriteEffects.FlipHorizontally,
                        0);
                    }
                    else
                    {
                        sb.Draw(
                        sprite,
                        new Vector2(this.posX, this.posY),
                        new Rectangle(
                            0,
                            flyingEnemyFrameY * FlyingEnemyFrameRectHeight,
                            FlyingEnemyFrameRectWidth,
                            FlyingEnemyFrameRectHeight),
                        Color.White,
                        rot,
                        Vector2.Zero,
                        scl,
                        SpriteEffects.None,
                        0);
                    }

                    break;
                
                //The hit left state
                case EnemyState.hitLeft:
                    if (direction)
                    {
                        sb.Draw(
                        sprite,
                        new Vector2(this.posX, this.posY),
                        new Rectangle(
                            0,
                            flyingEnemyFrameY * FlyingEnemyFrameRectHeight,
                            FlyingEnemyFrameRectWidth,
                            FlyingEnemyFrameRectHeight),
                        Color.White,
                        rot,
                        Vector2.Zero,
                        scl,
                        SpriteEffects.FlipHorizontally,
                        0);
                    }
                    else
                    {
                        sb.Draw(
                        sprite,
                        new Vector2(this.posX, this.posY),
                        new Rectangle(
                            0,
                            flyingEnemyFrameY * FlyingEnemyFrameRectHeight,
                            FlyingEnemyFrameRectWidth,
                            FlyingEnemyFrameRectHeight),
                        Color.White,
                        rot,
                        Vector2.Zero,
                        scl,
                        SpriteEffects.None,
                        0);
                    }

                    break;

                //The hit right state
                case EnemyState.hitRight:
                    if (direction)
                    {
                        sb.Draw(
                        sprite,
                        new Vector2(this.posX, this.posY),
                        new Rectangle(
                            0,
                            flyingEnemyFrameY * FlyingEnemyFrameRectHeight,
                            FlyingEnemyFrameRectWidth,
                            FlyingEnemyFrameRectHeight),
                        Color.White,
                        rot,
                        Vector2.Zero,
                        scl,
                        SpriteEffects.FlipHorizontally,
                        0);
                    }
                    else
                    {
                        sb.Draw(
                        sprite,
                        new Vector2(this.posX, this.posY),
                        new Rectangle(
                            0,
                            flyingEnemyFrameY * FlyingEnemyFrameRectHeight,
                            FlyingEnemyFrameRectWidth,
                            FlyingEnemyFrameRectHeight),
                        Color.White,
                        rot,
                        Vector2.Zero,
                        scl,
                        SpriteEffects.None,
                        0);
                    }

                    break;

                //The dying state
                case EnemyState.dying:
                    if (direction)
                    {
                        sb.Draw(
                        sprite,
                        new Vector2(this.posX, this.posY),
                        new Rectangle(
                            0,
                            flyingEnemyFrameY * FlyingEnemyFrameRectHeight,
                            FlyingEnemyFrameRectWidth,
                            FlyingEnemyFrameRectHeight),
                        Color.White,
                        rot,
                        Vector2.Zero,
                        scl,
                        SpriteEffects.FlipHorizontally,
                        0);

                        drawcloud(sb);
                    }
                    else
                    {
                        sb.Draw(
                        sprite,
                        new Vector2(this.posX, this.posY),
                        new Rectangle(
                            0,
                            flyingEnemyFrameY * FlyingEnemyFrameRectHeight,
                            FlyingEnemyFrameRectWidth,
                            FlyingEnemyFrameRectHeight),
                        Color.White,
                        rot,
                        Vector2.Zero,
                        scl,
                        SpriteEffects.None,
                        0);

                        drawcloud(sb);
                    }

                    break;
            }
            
        }

        /// <summary>
        /// Updates the clouds of the flying enemy's propellors. 
        /// </summary>
        void UpdateCloud()
        {
            for(int i = 0; i < CloudList.Count; i++)
            {
                //CloudList[i] = new Rectangle(CloudList[i].X, CloudList[i].Y, CloudList[i].Width + 20, CloudList[i].Height + 20);
                Rectangle current = CloudList[i];
                CloudList.Remove(current);
                current.Width += 5;
                current.Height += 5;
                current.X -= 3;
                CloudList.Add(current);

                if(CloudList[i].Width >= 2000)
                {
                    //CloudList.Remove(CloudList[i]);
                }
            }
        }

        /// <summary>
        /// Draws the flying enemy's cloud.
        /// </summary>
        /// <param name="sb"></param>
        void drawcloud(SpriteBatch sb)
        {
            for(int i = 0; i < CloudList.Count; i++)
            {
                float opval = .5F;
                float r = 255;

                sb.Draw(smoke, CloudList[i], new Color(r, 255, 255, opval));
            }
        }
    }
}

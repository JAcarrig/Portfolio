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
    class Coin : GameObject
    {
        //Fields
        private Rectangle CoinPos;
        private int xSpeed;
        private int ySpeed;

        //coin animation
        private int frame; // determines if the coin has completed one full animation cycle (determined by SpinFrameCount)
        private double timeCounter; // keeps track of elapsed time of a particular frame
        private double fps; // number of animation frames displayed per second
        private double timePerFrame; // amount of time each frame is displayed
        private Texture2D coinSpriteSheet; // image containing all animation sprites

        //constants for rectange
        private const int SpinFrameCount = 5; // Determines how many frames the animation is ( the number of sprites it runs through before going back to the first one)
        private const int CoinRectOffsetY = 4; // How far down the spritesheet (in pixels) the sprite is from the top
        private const int CoinRectHeight = 24; // Height of a single frame (You get this from counting the height of the sprite + a little bit of extra room)
        private const int CoinRectWidth = 20; // Width of a single frame (you get this from counting the width between each sprite)

        // Properties
        public int XSpeed
        {
            get { return xSpeed; }
            set { xSpeed = value; }
        }

        public int YSpeed
        {
            get { return ySpeed; }
            set { ySpeed = value; }
        }

        // Constructor
        public Coin(Rectangle coinPos, int xSpeed, int ySpeed, Texture2D coinSpriteSheet) : base(coinPos, coinSpriteSheet)
        {
            CoinPos = coinPos;
            this.xSpeed = xSpeed;
            this.ySpeed = ySpeed;
            this.coinSpriteSheet = coinSpriteSheet;
            fps = 10.0;
            timePerFrame = 1.0 / fps;
        }

        /// <summary>
        /// causes coins to fall and keeps them within bounds of the map
        /// </summary>
        public void Coinfall()
        {
            posX += xSpeed;
            posY += ySpeed;

            if(xSpeed < 0)
            {
                xSpeed += 1;
            }
            else if(xSpeed > 0)
            {
                xSpeed -= 1;
            }
            ySpeed += 2;

            if (position.Y >= 800 - position.Height)
            {
                position.Y = 800 - position.Height;
            }

            if (position.X <= 0)
            {
                position.X = 0;
                xSpeed = 0;
            }
            else if (position.X >= 3200 - Width)
            {
                position.X = 3200 - Width;
                xSpeed = 0;
            }
        }


        /// <summary>
        /// Draws the coin object.
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(coinSpriteSheet, new Rectangle(posX, posY, 20, 0), Color.White);
        }

        /// <summary>
        /// Keeps track of the coin's animation.
        /// </summary>
        /// <param name="gameTime"></param>
        public void CoinAnimation(GameTime gameTime)
        {
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;
            if (timeCounter >= timePerFrame)
            {
                frame += 1;

                if (frame > SpinFrameCount)     // Check the bounds
                    frame = 1;

                timeCounter -= timePerFrame;   
            }
        }

        /// <summary>
        /// Draws the fully animated coin.
        /// </summary>
        /// <param name="flipSprite"></param>
        /// <param name="spriteBatch"></param>
        public void DrawCoinSpinning(SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                coinSpriteSheet,
                new Vector2(posX, posY),
                new Rectangle(
                    frame * CoinRectWidth,
                    CoinRectOffsetY,
                    CoinRectWidth,
                    CoinRectHeight),
                Color.White,
                0,
                Vector2.Zero,
                1.2f,
                flipSprite,
                0);
        }

        

        /// <summary>
        /// Randomizes coin fall.
        /// </summary>
        /// <param name="pinit"></param>
        /// <returns></returns>
        public static Rectangle coinrand(Rectangle pinit)
        {
            Random cran = new Random();

            pinit.X += cran.Next(20, 60);
            pinit.Y += cran.Next(0, 3);

            return pinit;
        }
    }
}

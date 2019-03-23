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
    // Handles the animation for each individual particle
    class Particle
    {
        // Fields
        private Vector2 particlePosition;
        private bool direction;
        private bool isActive;
        private float scale;

        // Particle Effects Animation
        private int hitParticleFrameX;
        private int hitParticleFrameY;
        private double hitParticleTimeCounter;
        private double hitParticleFPS;                      // Instantiate these in the constructor
        private double hitParticleTimePerFrame;             // ---

        // Constants for Sprite Sheet
        private const int HitParticleFrameCountX = 10; // Determines how many frames the animation is ( the number of sprites it runs through before going back to the first one)
        private const int HitParticleFrameCountY = 5;
        private const int HitParticleRectOffsetY = 0; // How far down the spritesheet (in pixels) the sprite is from the top
        private const int HitParticleRectHeight = 96; // Height of a single frame (You get this from counting the height of the sprite + a little bit of extra room)
        private const int HitParticleRectWidth = 96; // Width of a single frame (you get this from counting the width between each sprite)

        // Properties
        public bool Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        // Determines whether a particle should be deleted or not
        public bool IsActive
        {
            get { return isActive; }
        }

        // Constructor
        public Particle(int xPos, int yPos, bool direction, float scale)
        {
            // Determines the particle position
            particlePosition = new Vector2(xPos, yPos);
            this.direction = direction;

            // The size of the particle is passed in
            this.scale = scale;

            // While active is true the particle will be drawn and animated
            isActive = true;

            hitParticleFPS = 75;
            hitParticleTimePerFrame = 1.0 / hitParticleFPS;
        }

        // Methods
        // Animates an individual partile based upon the gameTime
        public void ParticleAnimation(GameTime gameTime)
        {
            hitParticleTimeCounter += gameTime.ElapsedGameTime.TotalSeconds;
            if (hitParticleTimeCounter >= hitParticleTimePerFrame)
            {
                hitParticleFrameX++;

                hitParticleTimeCounter -= hitParticleTimePerFrame;

                if (hitParticleFrameX > HitParticleFrameCountX - 1)
                {
                    hitParticleFrameX = 0;
                    hitParticleFrameY++;

                    // When the animation completes the particle is set to inactive and 
                    // will ultimately be removed from the list
                    if(hitParticleFrameY == HitParticleFrameCountY - 1)
                    {
                        isActive = false;
                    }
                }
            }
        }

        // Draws the individual particle based upon direction and scale
        public void DrawParticle(SpriteEffects flipSprite, SpriteBatch spriteBatch, Texture2D particleSpriteSheet)
        {
            spriteBatch.Draw(
                particleSpriteSheet,
                particlePosition,
                new Rectangle(
                    hitParticleFrameX * HitParticleRectWidth,
                    HitParticleRectOffsetY + hitParticleFrameY * HitParticleRectHeight,
                    HitParticleRectWidth,
                    HitParticleRectHeight),
                Color.White,
                0,
                Vector2.Zero,
                scale,
                flipSprite,
                0);
        }
    }
}

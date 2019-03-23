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
    // The ParticleManager class acts as a container for all of the spawned particles,
    // having the ability to update and draw all existing particles from one object
    class ParticleManager
    {
        // Fields
        private List<Particle> particleList;
        Texture2D particleSpriteSheet;
        GameTime gameTime;

        // Properties
        public List<Particle> ParticleList
        {
            get { return particleList; }
        }

        public Texture2D ParticleSpriteSheet
        {
            get { return particleSpriteSheet; }
        }

        // Constructor
        public ParticleManager(Texture2D spriteSheet, GameTime gameTime)
        {
            // A list to store all spawned particles
            particleList = new List<Particle>();

            particleSpriteSheet = spriteSheet;

            this.gameTime = gameTime;
        }

        // Methods
        // Updates and animates all existing particles
        public void UpdateParticles()
        {
            foreach(Particle p in particleList)
            {
                p.ParticleAnimation(gameTime);
            }

            // Checks if all the particles are active. If they are are no longer active, they
            // are removed from the manager
            for(int i = 0; i < particleList.Count; i++)
            {
                if(particleList[i].IsActive == false)
                {
                    particleList.RemoveAt(i);
                    i--;
                }
            }
        }

        // Draws all spawned particles based upon direction
        public void DrawParticles(SpriteBatch sb)
        {
            foreach (Particle p in particleList)
            {
                if(p.Direction)
                {
                    p.DrawParticle(SpriteEffects.FlipHorizontally, sb, particleSpriteSheet);
                }
                else
                {
                    p.DrawParticle(SpriteEffects.None, sb, particleSpriteSheet);
                }
            }
        }
    }
}

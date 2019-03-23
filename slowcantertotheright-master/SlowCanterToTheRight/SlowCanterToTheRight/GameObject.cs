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
    abstract class GameObject
    {
        //Fields and Properties

        //position Rectangle
        protected Rectangle position;
        public int posX { get { return position.X; } set { position.X = value; } } 
        public int posY { get { return position.Y; } set { position.Y = value; } } 
        public int Width { get { return position.Width; } set { position.Width = value; } }
        public int Height { get { return position.Height; } set { position.Height = value; } }
        public Rectangle Position { get { return position; } set { position = value; } }
        //Sprite
        protected Texture2D sprite;
        public Texture2D Sprite { get { return sprite; } set { sprite = value; } }
        
        //Constructor
        public GameObject(Rectangle position, Texture2D sprite)
        {
            this.position = position;
            this.sprite = sprite;
        }

        /// <summary>
        /// The standard draw method.
        /// </summary>
        /// <param name="sb"></param>
        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, new Rectangle(posX, posY, Width, Height), Color.White);
        }
    }
}

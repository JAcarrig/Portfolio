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
    class Camera
    {
        // Fields
        // Position of the camera
        private Vector2 position;

        // The translation matrix based upon player position
        private Matrix veiwMatrix;

        private int screenWidth;
        private int screenHeight;

        // Fields for the min and max of the shake pixels
        private int shakeStartAngle;
        private float shakeRadius;

        // Properties
        public Matrix ViewMatrix
        {
            get { return veiwMatrix; }
        }

        public int ScreenWidth
        {
            get { return screenWidth; }
        }

        public int ScreenHeight
        {
            get { return screenHeight; }
        }

        // Can get and set the camera shake radius for different impacts
        public float ShakeRadius
        {
            get { return shakeRadius; }
            set { shakeRadius = value; }
        }

        // Constructor
        public Camera(int xBound, int yBound)
        {
            screenWidth = xBound;
            screenHeight = yBound;

            // Initial shake angle is 0. This changes as the camera rumbles
            shakeStartAngle = 0;

            // Sets the default shake radius
            shakeRadius = 10;
        }

        // UpdateCamera Method
        // Updates the camera matrix based upon the player's current position
        public Matrix UpdateCamera(Vector2 playerPosition)
        {
            // Sets the deafult camera view to be the player in the center if the screen
            position.X = playerPosition.X - (ScreenWidth / 2);

            // When the player is at either end of the map, the camera will be locked
            if(position.X < 0)
            {
                position.X = 0;
            }
            else if(position.X > ScreenWidth * 2 - ScreenWidth)
            {
                position.X = ScreenWidth * 2 - ScreenWidth;
            }

            // If the player is above one fifth of the screenheight from the top,
            // the camera will track the y-axis, otherwise it will be locked that the player's position
            position.Y = playerPosition.Y - (ScreenHeight / 5);

            if (position.Y > 0)
            {
                position.Y = 0;
            }

            // Returns the altered view matrix
            veiwMatrix = Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0));
            return veiwMatrix;
        }

        // Casues the camera to rumble
        // Returns a "rumbled" camera view
        public Matrix ShakeCamera(Vector2 playerPosition, Random rng)
        {
            // Sets the deafult camera view to be the player in the center if the screen
            position.X = playerPosition.X - (ScreenWidth / 2);

            // When the player is at either end of the map, the camera will be locked
            if (position.X < 0)
            {
                position.X = 0;
            }
            else if (position.X > ScreenWidth * 2 - ScreenWidth)
            {
                position.X = ScreenWidth * 2 - ScreenWidth;
            }

            // If the player is above one fifth of the screenheight from the top,
            // the camera will track the y-axis, otherwise it will be locked that the player's position
            position.Y = playerPosition.Y - (ScreenHeight / 5);

            if (position.Y > 0)
            {
                position.Y = 0;
            }

            // Changes the camera slightly by a small angle within the shakeradius and moves the position matrix by the offset
            // in both the X and Y directions
            Vector2 offset = new Vector2(0, 0);
            offset = new Vector2((float)(Math.Sin(shakeStartAngle) * shakeRadius), (float)(Math.Cos(shakeStartAngle) * shakeRadius));
            shakeRadius -= 0.25f;
            shakeStartAngle += (150 + rng.Next(60));

            // Returns the altered view matrix
            veiwMatrix = Matrix.CreateTranslation(-position.X + offset.X, -position.Y + offset.Y, 0);
            return veiwMatrix;
        }
    }
}

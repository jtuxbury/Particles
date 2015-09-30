using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Particles
{
    
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        Texture2D partTex;                  // A variable to hold the image/sprite of the particle
        
        // When we start creating particles, we'll reuse these variables over and over again.
        Vector2 partPos;                    // The position of the particle we're creating
        Vector2 partVel;                    // The velocity of the particle we're creating
        Particle tempParticle;              // A temporary particle that we use over and over again
        
        List<Particle> partiList;           // The list of all the alive particles
        Random generator;                   // A random number generator
        
        SpriteFont font;                    // To be able to draw text to the screen (for debugging)
        Vector2 textPos;                    // The position of the text
        
        public int screenWidth, screenHeight;// I use this to determine when to "bounce"

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this); 
            graphics.IsFullScreen = true;       // This is the only line I changed in the constructor
            Content.RootDirectory = "Content";
        }

        // This method is called exactly one, and is used to load images/audio/video
        protected override void LoadContent()
        {
            // This line of code is here by default
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // Set the text to be 10 pixels over, 10 pixels down
            textPos = new Vector2(10, 10);  
            // Load the font from the Content folder
            font = Content.Load<SpriteFont>("myFont");
            // Load the particle image and put it into the "partTex" variable
            partTex = Content.Load<Texture2D>("fire_particle");
            // Bring the list of particles to life, but note that it is *empty* (i.e. no particles yet)
            partiList = new List<Particle>();
            // Bring the random number generator to life
            generator = new Random();
            // I added this so that you can see the mouse
            this.IsMouseVisible = true;
            // This is funky code, but it determines how big the screen is
            screenWidth = graphics.GraphicsDevice.Viewport.Width;
            screenHeight = graphics.GraphicsDevice.Viewport.Height;
           
        }
        
        // This method is called 60 times per second and is used to update positions, determine collisions,
        // get user input, and in this case, add new particles to the partiList if the user holds the left
        // mouse button. It also removes any dead particles that are taking up memory!
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit - I added the "Escape" key part, so if the user presses it, we stop
            if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)||
                (Keyboard.GetState().IsKeyDown(Keys.Escape)))
                this.Exit();
            
            // Grab the current mouse position and store it into "mouseX" and "mouseY"
            int mouseX = Mouse.GetState().X;
            int mouseY = Mouse.GetState().Y;

            // If the user presses the left mouse button, then we're going to generate 10 particles - 
            // each going in a random direction!  We'll add each one to the partiList.
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                for (int i = 0; i < 10; i++)
                {
                    // Calculate a random direction (rotation) from 0 to 2 PI (it's in radians)
                    double rot = generator.NextDouble() * Math.PI*2.0;
                    // The particle's position will originate from the mouse position
                    partPos = new Vector2(mouseX, mouseY);
                    // It's velocity is based on using Sin and Cos of the rotation (from above).
                    // If you have a question about this, please let me know!
                    partVel = new Vector2((float)(Math.Cos(rot)*6.0), (float)(Math.Sin(rot) * 4.0 - 2.0));
                    // Bring the temp particle to life
                    tempParticle = new Particle(this, partTex, partPos, partVel);
                    // and add it to the partiList
                    partiList.Add(tempParticle);
                }
            }

            // Call the Update() method for each particle in the partiList
            foreach (Particle p in partiList)
            {
                p.Update();
            }

            // This is funky, but it's the way that we remove dead particles from the list.
            // You CANNOT use a foreach loop to do this (because it breaks the Iterator)
            for (int i = partiList.Count - 1; i >= 0; i--)
            {
                Particle temp = partiList[i];
                if (!temp.isAlive)
                {
                    partiList.Remove(temp);
                }
            }
            base.Update(gameTime);
        }

        // The Draw() method is also called 60 times per second.  This is where we tell all the
        // particles in the partiList to draw themselves
        protected override void Draw(GameTime gameTime)
        {
            // Clear the screen with the color Black
            GraphicsDevice.Clear(Color.Black);
            // Start a sprite batch (it's more efficient to batch up all the 2D stuff when drawing)
           
            spriteBatch.Begin(SpriteSortMode.BackToFront,BlendState.Additive);
            // Draw some text to the screen
            spriteBatch.DrawString(font, "R:" + partiList.Count, textPos, Color.White);
            // Tell all of the particles to draw themselves.  Here, we pass the spriteBatch variable
            // off to each particle - letting them use it for drawing
            foreach (Particle p in partiList)
            {
                p.Draw(spriteBatch);
            }
            // End of drawing 2D stuff
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

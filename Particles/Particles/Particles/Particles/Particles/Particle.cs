using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Particles
{
    // Here's a basic particle class.  We're going to be creating thousands of instances
    // of this class to get the effects we want...
    public class Particle
    {
        Texture2D tex;                      // The texture/image of the particle
        Rectangle rect;                     // We're not using this, but it's good for collision detection
        Vector2 pos, vel;                   // The position, velocity
        Vector2 midpoint;                   // The midpoint is used for rotation and represents the point 
                                            // that we'll be rotating about (which is the middle of the texture)
        Vector2 scale;                      // The scale is what stretches the sprite (in 2 directions)
        public double rot;                  // The rotation of the sprite
        Game1 parent;                       // A reference to the main game (running game)
        public bool isAlive;                // Whether or not this particle is alive
        int age;                            // The current age, which gets decremented in Update()
        Color color;                        // The current color of the particle

        public Particle(Game1 g, Texture2D t, Vector2 p, Vector2 v)
        {
            // I always start the age off at 255. It's great because you can use it for color as well!
            age = 255;          
            tex = t;
            pos = p;
            parent = g;
            vel = v;
            rect = new Rectangle((int)p.X, (int)p.Y, t.Width/4, t.Height/4);
            midpoint = new Vector2(rect.Width/2, rect.Height / 2);
            rot = 0;
            scale = new Vector2(0.1f, 0.1f);
            isAlive = true;
            color = new Color(age, age, age, age);
        }

        // This update is called for each particle in Game1.cs (look at the Update method there)
        public void Update()
        {
            // Age the particle
            age-=2;
            // 1) Change its color based on age.  If you change the color, it will have to be type-cast to a byte
            color.G = color.A = (byte)age;
            color.B = (byte)(age / 2);
            // Check and see if the particle is alive.  If not, flip the isAlive boolean
            if (age <= 0)
            {
                isAlive = false;
            }
            // Make the particle bounce.  Think about this code...
            if (pos.Y+vel.Y >= parent.screenHeight)
            {
                vel.Y = -vel.Y/2;
            }
            // This is how gravity pulls the particle down the screen (positive is down)
            vel.Y += 0.1f;
            // 2) Calculate the rotation of the particle based on its velocity

            rot = Math.Atan2(vel.Y, vel.X);
            // PLEASE UNDERSTAND THIS CODE: Update the particles position based on it's velocity
            pos += vel;

            // 3) Change the scale of the particle based on the velocity of the particle, but only the X value.
            // The Y value stays constant.  Remember, you can use the vel.Length() to help you..
            scale = new Vector2(vel.Length()/10,.1f);
            
        }
        public void Draw(SpriteBatch sb)
        {
           
            sb.Draw(tex, pos,null,color,(float)rot,midpoint,scale,SpriteEffects.None,1.0f);

            // 4) Out of lovingkindness, I give you the code below.  Only use it when those variables are valid!
            //sb.Draw(tex, pos, null, color, (float)rot, midpoint, scale, SpriteEffects.None, 1.0f);
            
        }
    }
}
    
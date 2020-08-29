using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace MysteryOfAtonClient
{
    public class ResolutionHandler
    {
        public Rectangle windowBounds;
        public Rectangle renderRectangle;
        public ResolutionHandler(Rectangle windowBounds)
        {
            this.windowBounds = windowBounds;
            renderRectangle = FixWindowRatios();
        }

        /// <summary>
        /// Adjusts the rendered screen depending on window size
        /// </summary>
        /// <returns></returns>
        public Rectangle FixWindowRatios()
        {
            float outputAspect = windowBounds.Width / (float)windowBounds.Height;
            float preferredAspect = Client.width / (float)Client.height;

            Rectangle dst;

            if (outputAspect <= preferredAspect)
            {
                // output is taller than it is wider, bars on top/bottom
                int presentHeight = (int)((windowBounds.Width / preferredAspect) + 0.5f);
                int barHeight = (windowBounds.Height - presentHeight) / 2;

                dst = new Rectangle(0, barHeight, windowBounds.Width, presentHeight);
            }
            else
            {
                // output is wider than it is tall, bars left/right
                int presentWidth = (int)((windowBounds.Height * preferredAspect) + 0.5f);
                int barWidth = (windowBounds.Width - presentWidth) / 2;

                dst = new Rectangle(barWidth, 0, presentWidth, windowBounds.Height);
            }
            
            return dst;
        }

        /// <summary>
        /// Maps the game window mouse position to the rendered images mouse position
        /// </summary>
        /// <returns></returns>
        public TMouseState transformedMouseState()
        {
            var mouse = Mouse.GetState();

            float yAspect = Client.height / (float)(windowBounds.Height-renderRectangle.Y*2);
            float xAspect = Client.width / (float)(windowBounds.Width-renderRectangle.X*2);

            int mouseXPos = Convert.ToInt32((mouse.X-renderRectangle.X) * xAspect);
            int mouseYPos = Convert.ToInt32((mouse.Y-renderRectangle.Y) * yAspect);

            return new TMouseState(mouse, new Point(mouseXPos, mouseYPos));
        }
        
    }
}

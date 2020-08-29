using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MysteryOfAtonClient.Textboxes
{
    class Textbox
    {
        private Rectangle _destinationRect;

        protected Rectangle _sourceRect;
        protected Texture2D _boxTexture;

        public Vector2 textLocation;
        public Rectangle destinationRect { get { return _destinationRect; } }
        public StringBuilder displayText;
        public SpriteFont _spriteFont;
       
        public Textbox(Texture2D texture, SpriteFont spriteFont)
        {
            _boxTexture = texture;
            _spriteFont = spriteFont;
            displayText = new StringBuilder("", 50);
            _sourceRect = _boxTexture.Bounds;
        }

        /// <summary>
        /// If there is a long text that should fit in a box,
        /// then this function structures it to fit
        /// </summary>
        public void StructureText()
        {
            var words = displayText.ToString().Split(' ', '\n');

            string structuredText = "";


            for(var i = 0; i < words.Length; i++)
            {
                if (_spriteFont.MeasureString(words[i]).X > _destinationRect.Width)
                {
                    throw new Exception("A word is longer than the destinationRect. Word: "+words[i]);
                }

                if(_spriteFont.MeasureString(structuredText+" "+words[i]).X < _destinationRect.Width)
                {
                    structuredText += words[i];
                }
                else
                {
                    structuredText += "\n" + words[i];
                }
            }

        }
        /// <summary>
        /// Calculates a fitting text position and sets the X and Y coordinates.
        /// Test position can be set manually by the textLocation variable;
        /// </summary>
        /// <returns></returns>
        public Vector2 CalculateTextPosition()
        {
                textLocation = new Vector2(_destinationRect.Left + _destinationRect.Width * (float)0.1, _destinationRect.Top + _destinationRect.Height * (float)0.1);
            return textLocation;
        }

        /// <summary>
        /// Destination rectangle defines where the image will appear.
        /// </summary>
        /// <param name="dRect"></param>
        public void SetDestinationRectangle(Rectangle dRect)
        {
            _destinationRect = dRect;
            if(textLocation == Vector2.Zero)
            {
                CalculateTextPosition();
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_boxTexture, _destinationRect, _sourceRect, Color.White);
            spriteBatch.DrawString(_spriteFont, displayText, textLocation, Color.Black);
        }

    }
}

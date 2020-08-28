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
        protected Rectangle _sourceRect;
        public Rectangle destinationRect { get { return _destinationRect; } }
        private Rectangle _destinationRect;
        public Vector2 textLocation;
        private ContentManager _content;
        protected Texture2D _boxTexture;

        public StringBuilder displayText;
        public SpriteFont _spriteFont;
       
        public Textbox(ContentManager content)
        {
            _content = content;

            displayText = new StringBuilder("", 50);
        }

        
        /// <summary>
        /// Loads textures and spritefonts and sets destination rectangle
        /// and text position to default values
        /// </summary>
        /// <param name="textureName">Name of the texture-file to use as background</param>
        /// <param name="fontName">Name of the spritefont-file</param>
        protected void LoadTextboxProperties(string textureName, string fontName)
        {
            _boxTexture = _content.Load<Texture2D>(textureName);
            _spriteFont = _content.Load<SpriteFont>(fontName);

            _sourceRect = _boxTexture.Bounds;

            _destinationRect = _destinationRect.IsEmpty ? _sourceRect : _destinationRect;

            if (textLocation.Length() == 0)
            {
                CalculateTextPosition();
            }

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
            if(textLocation.Length() == 0)
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

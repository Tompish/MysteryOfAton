using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace MysteryOfAtonClient.Textboxes
{
    class InputTextbox : Textbox
    {
        private bool _isActive;
        private readonly GameWindow _window;

        public bool isActive { get { return _isActive; } }

        public InputTextbox(ContentManager content, GameWindow window) : base(content)
        {
            _window = window;
            _isActive = false;
        }

        /// <summary>
        /// Initialization is mandatory, or else spritefonts and textures wont load.
        /// </summary>
        /// <param name="TextureFileName"></param>
        /// <param name="FontFileName"></param>
        public void InitializeTextbox(string TextureFileName, string FontFileName)
        {
            base.LoadTextboxProperties(TextureFileName, FontFileName);
        }

        /// <summary>
        /// Checks if user is clicking on textbox.
        /// If yes, then all text inputs will be added to a string
        /// </summary>
        /// <param name="mouse"></param>
        public virtual void CheckTextboxState(MouseState mouse)
        {
            if(base.destinationRect.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed && !_isActive)
            {
                _isActive = true;
                beginTypeEvent();
            }
            else if(!base.destinationRect.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed && _isActive)
            {
                _isActive = false;
                closeTypeEvent();
            }
        }

        /// <summary>
        /// Takes input characters and adds them to a string
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected virtual void OnKeyEvent(object sender, TextInputEventArgs args)
        {
            var charPressed = args.Character;
            var keyPressed = args.Key;

            //Handle special characters
            switch (keyPressed)
            {
                case Microsoft.Xna.Framework.Input.Keys.Back:
                    if (displayText.Length > 0)
                        displayText = displayText.Substring(0, displayText.Length - 1);
                    return;

                case Microsoft.Xna.Framework.Input.Keys.Escape:
                case Microsoft.Xna.Framework.Input.Keys.Tab:
                    return;
            }

            //If normal character and there is room, add character
            if (_spriteFont.MeasureString(displayText).X + (textLocation.X - destinationRect.Left) * 2 < destinationRect.Width)
            {
                displayText += charPressed;
                int pointerPosX = Convert.ToInt32(_spriteFont.MeasureString(displayText).X + textLocation.X);
                int pointerPosY = destinationRect.Y + destinationRect.Height/2;
            }
        }
        protected void closeTypeEvent()
        {
            _window.TextInput -= OnKeyEvent;
        }
        protected void beginTypeEvent()
        {
            _window.TextInput += OnKeyEvent;
        }

    }
}
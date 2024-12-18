using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MysteryOfAtonClient.UI
{
    class TextInputElement: TextElement
    {
        private readonly GameWindow _window;
        private ResolutionHandler _rHandler;
        public TextInputElement(GameWindow window, Texture2D texture, string? text, SpriteFont spriteFont, ResolutionHandler rhandler): base(texture, text, spriteFont)
        {
            _window = window;
            _rHandler = rhandler;
        }

        public void InputHandler()
        {
            var mouseState = _rHandler.transformedMouseState();

            if(base._destinationRect.Contains(mouseState.TMousePosition) && mouseState.OriginalMouseState.LeftButton == ButtonState.Pressed && !_focused)
            {
                _focused = true;
                beginTypeEvent();
            }
            else if(!base._destinationRect.Contains(mouseState.TMousePosition) && mouseState.OriginalMouseState.LeftButton == ButtonState.Pressed && _focused)
            {
                _focused = false;
                closeTypeEvent();
            }
        }

        protected virtual void OnKeyEvent(object sender, TextInputEventArgs args)
        {
            var charPressed = args.Character;
            var keyPressed = args.Key;

            //Handle special characters
            switch (keyPressed)
            {
                case Microsoft.Xna.Framework.Input.Keys.Back:
                    if (displayText.Length > 0)
                        displayText.Length--;
                    return;

                case Microsoft.Xna.Framework.Input.Keys.Escape:
                case Microsoft.Xna.Framework.Input.Keys.Tab:
                    return;
            }

            //If normal character and there is room, add character
            if (_spriteFont.MeasureString(displayText).X + (padding*2 - _destinationRect.Left) * 2 < _destinationRect.Width)
            {
                displayText.Append(charPressed);
                int pointerPosX = Convert.ToInt32(_spriteFont.MeasureString(displayText).X + padding);
                int pointerPosY = _destinationRect.Y + _destinationRect.Height/2;
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

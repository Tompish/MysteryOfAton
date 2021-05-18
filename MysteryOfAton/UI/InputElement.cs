using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace MysteryOfAtonClient.UI
{
    class InputElement : BaseElement
    {
        private ResolutionHandler _rHandler;
        private MouseState _prevMouseState;
        public InputElement(ResolutionHandler rHandler) : base() {
            _rHandler = rHandler;
        }

        public virtual void MouseInput()
        {
            var mState = _rHandler.transformedMouseState();

            if (_destinationRect.Contains(mState.TMousePosition))
            {
                if (mState.OriginalMouseState.LeftButton == ButtonState.Pressed && _prevMouseState.LeftButton == ButtonState.Released)
                {
                    _focused = true;
                }
            }
            else
            {
                if (mState.OriginalMouseState.LeftButton == ButtonState.Pressed)
                    _focused = false;
            }
        }

    }
}

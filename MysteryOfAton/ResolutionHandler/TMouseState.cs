using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace MysteryOfAtonClient
{
    public struct TMouseState
    {
        public TMouseState(MouseState OriginalMouseState, Point TransformedMousePosition)
        {
            this.OriginalMouseState = OriginalMouseState;
            this.TMousePosition = TransformedMousePosition;
        }
        public MouseState OriginalMouseState;
        public Point TMousePosition;
    }
}

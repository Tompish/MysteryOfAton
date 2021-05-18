using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace MysteryOfAtonClient.UI
{
    abstract class BaseElement
    {
        private BaseElement _parent = null;
        protected bool _isVisible = true;
        protected bool _isDragable;
        protected Texture2D _texture ;
        protected bool _focused;
        protected bool _hasActiveTexture { get { return _hasActiveTexture; } set { _hasActiveTexture = value; SetSourceRectangle(); } }
        protected Vector2 position;
        protected Vector2 size;
        public Rectangle? internalRect = null;
        protected Rectangle _destinationRect => new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);

        public List<BaseElement> children = new List<BaseElement>();
        public Rectangle _sourceRect { get; private set; }

        public BaseElement() {}
        public BaseElement(Texture2D texture)
        {
            _texture = texture;
        }
        public BaseElement(BaseElement parent)
        {
            _parent = parent;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if(_texture != null && _isVisible) { spriteBatch.Draw(_texture, position, Color.White); }

            foreach(BaseElement element in children)
            {
                if(element._isVisible) { element.Draw(spriteBatch); }
            }
        }

        protected virtual void SetSourceRectangle()
        {
            if(_texture == null)
                throw new NullReferenceException("Cannot set source rectangle when there is no texture");

            _sourceRect = new Rectangle(0, 0, _texture.Width / 2, _texture.Height); 
        }

        protected void AddChild(BaseElement element)
        {
            element._parent = this;
            children.Add(element);
        }

        protected BaseElement GetSibling()
        {
            if (_parent == null)
                throw new ArgumentNullException("Cannot get siblings when element has no parent");

            var iterator = _parent.children.GetEnumerator();

            while (iterator.Current != null)
            {
                if (iterator.Current == this && iterator.MoveNext())
                {
                    return iterator.Current;
                }
            }
            return null;
        }

        public virtual void DragElement()
        {
            if(!_isDragable) { return; }

            position = Mouse.GetState().Position.ToVector2();
        }
    }
}

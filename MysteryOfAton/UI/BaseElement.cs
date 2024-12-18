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
        //if UI item is an animation
        protected bool _hasActiveTexture { get { return _hasActiveTexture; } set { _hasActiveTexture = value; SetSourceRectangle(); } }
        public Rectangle? internalRect = null;
        protected Rectangle _destinationRect => new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
        //Set vertical or horizontal stacking. Both cannot be true;
        protected bool _hStack { get { return _hStack; } set { _hStack = value; _vStack = !value; organizeChildren(); } }
        protected bool _vStack { get { return _vStack;  } set { _vStack = value; _hStack = !value; organizeChildren(); } }

        public List<BaseElement> children = new List<BaseElement>();
        public Rectangle _sourceRect { get; private set; }
        public Vector2 position;
        public Vector2 size;

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

        protected void organizeChildren() {
            var childIterator = this.children.GetEnumerator();

            var previousChild = childIterator.Current;
            previousChild.position = this.position;

            var elementCoordExtremeties = this.position;
            elementCoordExtremeties.X += childIterator.Current._destinationRect.Width;
            elementCoordExtremeties.Y += childIterator.Current._destinationRect.Height;


            childIterator.MoveNext();


            while(childIterator.Current != null){

                //If it is dragable, it might be necessary to drag it outside parent element
                if (!childIterator.Current._isDragable)
                {
                    //If items are to be stacked horizontally
                    if (childIterator.Current._hStack)
                    {
                        childIterator.Current.position.X = previousChild.position.X + previousChild._destinationRect.Width;
                        childIterator.Current.position.Y = childInsideParentBoundsX(previousChild)? 
			                previousChild.position.Y: previousChild.position.Y+previousChild._destinationRect.Height;
                    }
                    //If items are to be stacked vertically
                    else if (childIterator.Current._vStack) {
                        childIterator.Current.position.Y = previousChild.position.Y + previousChild._destinationRect.Height;
                        childIterator.Current.position.X = childInsideParentBoundsY(previousChild) ?
                            previousChild.position.X : elementCoordExtremeties.X;
		            }

                    elementCoordExtremeties.X = childIterator.Current.position.X < elementCoordExtremeties.X ?
                        elementCoordExtremeties.X : childIterator.Current.position.X;

                    elementCoordExtremeties.Y = childIterator.Current.position.Y < elementCoordExtremeties.Y ?
                        elementCoordExtremeties.Y : childIterator.Current.position.Y;
                }

                previousChild = childIterator.Current;
	        }
	    }

        private bool childInsideParentBoundsX(BaseElement child) {

            return this.position.X + this._destinationRect.Width < child.position.X + child._destinationRect.Width;
	    }

        private bool childInsideParentBoundsY(BaseElement child) {

            return this.position.Y + this._destinationRect.Height < child.position.Y + child._destinationRect.Height;
	    }
    }
}

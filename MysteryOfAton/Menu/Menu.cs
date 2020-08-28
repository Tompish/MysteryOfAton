using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MysteryOfAtonClient.Textboxes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MysteryOfAtonClient.Menu
{
    class Menu
    {
        private ContentManager _content;
        private Texture2D _menuBG;
        private SpriteFont _menuFont;
        private bool _isActive;
        private MenuItem[] _menuItem;
        public LoginTextbox loginTextbox;
        private GameWindow _window;

        public bool isActive { get { return _isActive; } }
        public Menu(ContentManager content, GameWindow window)
        {
            _content = content;
            _window = window;
            loginTextbox = new LoginTextbox(_content, window);
        }

        public void LoadMenu()
        {
            if (_isActive) return;
            _menuBG = _content.Load<Texture2D>("MenuBackground");
            _menuFont = _content.Load<SpriteFont>("MenuFont");
            LoadMenuItems();
            loginTextbox.InitializeLoginbox();
            _isActive = true;
        }

        private void LoadMenuItems()
        {
            _menuItem = new MenuItem[2];

            var quitDimensions = _menuFont.MeasureString("Quit");
            var connectDimensions = _menuFont.MeasureString("Connect");

            var quitRect = new Rectangle((int)(_window.ClientBounds.Width - quitDimensions.X) / 2
                , (_window.ClientBounds.Height / 2 + 30)
                , (int)quitDimensions.X + 10
                , (int)quitDimensions.Y + 10);

            var connectRect = new Rectangle((int)(_window.ClientBounds.Width - connectDimensions.X) / 2
                , (_window.ClientBounds.Height / 2)
                , (int)connectDimensions.X + 10
                , (int)connectDimensions.Y + 10);

            _menuItem[0] = new MenuItem() 
            { text = "Connect", activeArea = connectRect, color = Color.White, menuChoice = MenuChoice.connect };

            _menuItem[1] = new MenuItem()
            { text = "Quit", activeArea = quitRect, color = Color.White, menuChoice = MenuChoice.quit };

        }

        public void CloseMenu()
        {
            _menuBG.Dispose();
            _menuFont.Texture.Dispose();
            //_loginTextbox.DeactivateTextbox();

            _isActive = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_menuBG, new Rectangle(0, 0, 800, 480), Color.White);
            loginTextbox.Draw(spriteBatch);
            

            for(var i = 0; i<_menuItem.Length; i++)
            {
                spriteBatch.DrawString(_menuFont, _menuItem[i].text, _menuItem[i].activeArea.Location.ToVector2(), _menuItem[i].color);
            }
        }

        public MenuChoice Update(MouseState mouse)
        {
            for (var i = 0; i < _menuItem.Length; i++)
            {
                if (_menuItem[i].activeArea.Contains(mouse.Position))
                {
                    Mouse.SetCursor(MouseCursor.Hand);
                    _menuItem[i].color = Color.Yellow;

                    if(mouse.LeftButton == ButtonState.Pressed)
                    {
                        return _menuItem[i].menuChoice;
                    }
                    break;
                }
                else
                {
                    Mouse.SetCursor(MouseCursor.Arrow);
                    _menuItem[i].color = Color.White;
                }
                
            }

            loginTextbox.Update(mouse);

            return MenuChoice.idle;
            
        }
    }

    struct MenuItem
    {
        public string text;
        public Rectangle activeArea;
        public Color color { get; set; }
        public MenuChoice menuChoice { get; set; }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MysteryOfAtonClient.Menu;
using System.Diagnostics;

namespace MysteryOfAtonClient
{
    public class Client : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Menu.Menu _menu;
        private Networking _networking;
        private RenderTarget2D _renderTarget;

        public ResolutionHandler rHandler;
        public const int width = 1920;
        public const int height = 1080;

        public Client()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _networking = new Networking();
            _menu = new Menu.Menu(Content, Window);
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = width-400;
            _graphics.PreferredBackBufferHeight = height-400;
            _graphics.ApplyChanges();
            _renderTarget = new RenderTarget2D(_graphics.GraphicsDevice
                , width
                , height
                , false
                , SurfaceFormat.Color
                , DepthFormat.None
                , GraphicsDevice.PresentationParameters.MultiSampleCount
                , RenderTargetUsage.DiscardContents);

            rHandler = new ResolutionHandler(Window.ClientBounds);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            _menu.LoadMenu();

        }

        protected override void Update(GameTime gameTime)
        {
            var keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Escape))
                _menu.LoadMenu();

            if(_menu.isActive)
            {
                switch(_menu.Update(rHandler.transformedMouseState()))
                {
                    case MenuChoice.connect:
                    {
                        _networking.initiateClientNetwork(_menu.loginTextbox.userName.ToString(), _menu.loginTextbox.userPassword.ToString());
                        break;
                    }
                    case MenuChoice.quit:
                        {
                            Exit();
                            break;
                        }
                }
            }

            if (_networking.isConnected) { _networking.checkWithServer(); }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(_renderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //Draw everything on the rendertarget
            _spriteBatch.Begin();

            _menu.Draw(_spriteBatch);
            

            _spriteBatch.End();

            //Clear the rendertarget so everything eventually gets drawn on the screen.
            GraphicsDevice.SetRenderTarget(null);
            
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            
            _spriteBatch.Draw(_renderTarget, rHandler.renderRectangle, Color.White);

            _spriteBatch.End();
            
            // TODO: Add your drawing code here

            base.Draw(gameTime);

        }
    }
}

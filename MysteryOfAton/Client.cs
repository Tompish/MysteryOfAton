using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MysteryOfAtonClient.Menu;

namespace MysteryOfAtonClient
{
    public class Client : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Menu.Menu _menu;
        private Networking _networking;
        private RenderTarget2D _renderTarget;

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
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();
            _renderTarget = new RenderTarget2D(_graphics.GraphicsDevice
                , _graphics.PreferredBackBufferWidth
                , _graphics.PreferredBackBufferHeight
                , false
                , SurfaceFormat.Color
                , DepthFormat.None
                , GraphicsDevice.PresentationParameters.MultiSampleCount
                , RenderTargetUsage.DiscardContents);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            _menu.LoadMenu();

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            var mouse = Mouse.GetState(this.Window);
            var keyboard = Keyboard.GetState();
            if (/*GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||*/ keyboard.IsKeyDown(Keys.Escape))
                _menu.LoadMenu();
                //Exit();

            if(_menu.isActive)
            {
                switch(_menu.Update(mouse))
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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _spriteBatch.Draw(_renderTarget, Vector2.Zero, null, Color.White);

            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);

            
        }
    }
}

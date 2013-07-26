using System;
using System.IO;
using System.Reflection;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Input;
using WaveEngine.Common.Math;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;

namespace SaveMe
{
    public class App : WaveEngine.Adapter.Application
    {
        SaveMeProject.Game _game;
        SpriteBatch _spriteBatch;
        Texture2D _splashScreen;
        bool _splashState = true;
        TimeSpan _time;
        Vector2 _position;
        Color _backgroundSplashColor;

        public App()
        {
            Width = 800;
            Height = 600;
            FullScreen = false;
            WindowTitle = "SaveMe";
        }

        public override void Initialize()
        {
            _game = new SaveMeProject.Game();
            _game.Initialize(Adapter);

            #region WAVE SOFTWARE LICENSE AGREEMENT
            _backgroundSplashColor = new Color(32, 32, 32, 255);
            _spriteBatch = new SpriteBatch(WaveServices.GraphicsDevice);

            var resourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            string name = string.Empty;

            foreach (string item in resourceNames)
            {
                if (item.Contains("SplashScreen.wpk"))
                {
                    name = item;
                    break;
                }
            }
           

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidProgramException("License terms not agreed.");
            }

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name))
            {
                _splashScreen = WaveServices.Assets.Global.LoadAsset<Texture2D>(name, stream);
            }

            _position = new Vector2();
            _position.X = (Width / 2.0f) - (_splashScreen.Width / 2.0f);
            _position.Y = (Height / 2.0f) - (_splashScreen.Height / 2.0f);
            #endregion
        }

        public override void Update(TimeSpan elapsedTime)
        {
            if (_game != null && !_game.HasExited)
            {
                if (WaveServices.Input.KeyboardState.F10 == ButtonState.Pressed)
                {
                    FullScreen = !FullScreen;
                }

                if (_splashState)
                {
                    #region WAVE SOFTWARE LICENSE AGREEMENT
                    _time += elapsedTime;
                    if (_time > TimeSpan.FromSeconds(2))
                    {
                        _splashState = false;
                    }
                    #endregion
                }
                else
                {
                    if (WaveServices.Input.KeyboardState.Escape == ButtonState.Pressed)
                    {
                        Exit();
                        _game.Unload();
                    }
                    else
                    {
                        _game.UpdateFrame(elapsedTime);
                    }
                }
            }
        }

        public override void Draw(TimeSpan elapsedTime)
        {
            if (_game == null || _game.HasExited) return;
            if (_splashState)
            {
                #region WAVE SOFTWARE LICENSE AGREEMENT
                WaveServices.GraphicsDevice.RenderTargets.SetRenderTarget(null);
                WaveServices.GraphicsDevice.Clear(ref _backgroundSplashColor, ClearFlags.Target, 1);
                _spriteBatch.Begin();
                _spriteBatch.Draw(_splashScreen, _position, Color.White);
                _spriteBatch.End();
                #endregion
            }
            else
            {
                _game.DrawFrame(elapsedTime);
            }
        }
    }
}


#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
#endregion

namespace SaveMeProject
{
    public class Game : WaveEngine.Framework.Game
    {
        public override void Initialize(IAdapter adapter)
        {
            base.Initialize(adapter);

            ScreenLayers screenLayers = WaveServices.ScreenLayers;
            screenLayers.AddScene<MyScene>();
            screenLayers.Apply();
        }
    }
}

#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics2D;
using WaveEngine.Framework.Services;
#endregion

namespace SaveMeProject
{
    public class MyScene : Scene
    {
        private const string GroundTexture = "Content/GroundSprite.wpk";

        protected override void CreateScene()
        {
            RenderManager.BackgroundColor = Color.CornflowerBlue;
            EntityManager.Add(CreateGround("floor1", 0,
                WaveServices.Platform.ScreenHeight, 0));
            EntityManager.Add(CreateGround("floor2", WaveServices.Platform.ScreenWidth / 2,
                WaveServices.Platform.ScreenHeight, 0));
            EntityManager.Add(CreateGround("floor3", WaveServices.Platform.ScreenWidth,
                WaveServices.Platform.ScreenHeight, 0));
        }


        /// <summary>
        /// This method creates the floor, with collisions, especifying the coordinates.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        private static Entity CreateGround(string name, float x, float y, float angle)
        {
            Entity sprite = new Entity(name)
                .AddComponent(new Transform2D() { X = x, Y = y })
                .AddComponent(new RectangleCollider())
                .AddComponent(new Sprite(GroundTexture))
                .AddComponent(new RigidBody2D() { IsKinematic = true })
                .AddComponent(new SpriteRenderer(DefaultLayers.Opaque));

            sprite.FindComponent<RigidBody2D>().Rotation = angle;

            return sprite;
        }

    }
}

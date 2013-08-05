#region Using Statements
using System;
using SaveMeProject.Helpers;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Animation;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics2D;
using WaveEngine.Framework.Physics3D;
using WaveEngine.Framework.Services;
using WaveEngine.Materials;

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

            //EntityManager.Add(CreateSaver("people", WaveServices.Platform.ScreenWidth / 3,
            //WaveServices.Platform.ScreenHeight - 30));
            EntityManager.Add(CreateRug("rug", WaveServices.Platform.ScreenWidth / 3, WaveServices.Platform.ScreenHeight - 100));
            //EntityManager.Add(CreateVictim("viticm", WaveServices.Platform.ScreenWidth / 3, 0, 1f ));

            var instances = 0;
             //Timer to create crates
            WaveServices.TimerFactory.CreateTimer("FallingCratesTimer", TimeSpan.FromSeconds(3f), () =>
            {
                var box = CreateVictim("Crate" + instances++, WaveServices.Platform.ScreenWidth/2, -10, 0.5f);
                EntityManager.Add(box);

                if (instances > 9)
                {
                    EntityManager.Remove("Crate" + (instances - 10));
                    //WaveServices.TimerFactory.RemoveTimer("FallingCratesTimer");
                }
            });


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


        private static Entity CreateVictim(string name, float x = 0, float y = 0, float mass = 5f)
        {
            var victim = new Entity(name)
                .AddComponent(new Transform2D() {X = x, Y = y})
                //.AddComponent(new Transform3D(){ Position = new Vector3(x,y,0)})
                .AddComponent(new RectangleCollider())
                .AddComponent(new Sprite("Content/Crate.wpk"))
                //.AddComponent(Model.CreateSphere())
                .AddComponent(new RigidBody2D
                                  {
                                      IsKinematic = false,
                                      Mass = mass,
                                      Restitution = 1f,
                                      CollisionCategories = Physic2DCategory.All
                                  })
                //.AddComponent(new MaterialsMap(new BasicMaterial(Color.Black)))
                .AddComponent(new SpriteRenderer(DefaultLayers.Alpha));
                //.AddComponent(new ModelRenderer());

            return victim;
        }

        private static Entity CreateRug(string name, float x = 0, float y = 0)
        {
            var rug = new Entity(name)
                .AddComponent(new Transform2D() { X = x, Y = y, XScale = 10, YScale = 1 })
                .AddComponent(new RectangleCollider())
                .AddComponent(new Sprite("Content/Wood.wpk"))
                .AddComponent(new RigidBody2D()
                {
                    IsKinematic = true, 
                    CollisionCategories = Physic2DCategory.All,
                    Mass = 1f
                })
                .AddComponent(new SpriteRenderer(DefaultLayers.Alpha))
                .AddComponent(new SaverBehavior());

            return rug;
        }

    }
}

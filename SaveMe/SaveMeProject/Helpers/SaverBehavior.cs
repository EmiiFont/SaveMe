using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Input;
using WaveEngine.Components.Animation;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;

namespace SaveMeProject.Helpers
{
    public class SaverBehavior : Behavior
    {

        private const int SPEED = 5;
        private const int RIGHT = 1;
        private const int LEFT = -1;
        private const int NONE = 0;
        private const int BORDER_OFFSET = 25;

        private Animation2D _reverseSaver;
        private Animation2D _saver;
        private bool _reverse;

        [RequiredComponent]
        public Animation2D anim2D;
        [RequiredComponent]
        public Transform2D trans2D;

        private int direction;
        private AnimState currentState, lastState;
        private enum AnimState { Idle, Right, Left };

        public SaverBehavior(bool reverse)
            : base("SaverBehavior")
        {
            _reverse = reverse;
            direction = NONE;

            _saver = Animation2D.Create<SpriteXMLReader>("Content/Awesomenauts.xml").
                Add("Running", new SpriteSheetAnimationSequence
                                   {
                                       First = 1,
                                       Length = 70,
                                       FramesPerSecond = 70,
                                   }).
                Add("Idle", new SpriteSheetAnimationSequence
                                {
                                    First = 1,
                                    Length = 70,
                                    FramesPerSecond = 70,
                                });

            _reverseSaver = Animation2D.Create<SpriteXMLReaderRevese>("Content/Awesomenauts.xml").
                Add("Running", new SpriteSheetAnimationSequence
                                   {
                                       First = 1,
                                       Length = 70,
                                       FramesPerSecond = 70,
                                   }).
                Add("Idle", new SpriteSheetAnimationSequence
                                {
                                    First = 1,
                                    Length = 70,
                                    FramesPerSecond = 70,
                                });


            trans2D = null;
            anim2D = null;
            currentState = AnimState.Idle;
        }

        protected override void Initialize()
        {
            _saver = Animation2D.Create<SpriteXMLReader>("Content/Awesomenauts.xml").
                Add("Running", new SpriteSheetAnimationSequence
                {
                    First = 1,
                    Length = 70,
                    FramesPerSecond = 70,
                }).
                Add("Idle", new SpriteSheetAnimationSequence
                {
                    First = 37,
                    Length = 40,
                    FramesPerSecond = 60,
                });

            _reverseSaver = Animation2D.Create<SpriteXMLReaderRevese>("Content/Awesomenauts.xml").
                Add("Running", new SpriteSheetAnimationSequence
                                   {
                                       First = 1,
                                       Length = 70,
                                       FramesPerSecond = 70,
                                   }).
                Add("Idle", new SpriteSheetAnimationSequence
                                {
                                    First = 37,
                                    Length = 40,
                                    FramesPerSecond = 60,
                                });

            trans2D.Effect = _reverse ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            anim2D = _reverse ? _saver : _reverseSaver;

            base.Initialize();
        }
        protected override void Update(TimeSpan gameTime)
        {
            currentState = AnimState.Idle;

            // touch panel
            var touches = WaveServices.Input.TouchPanelState;
            if (touches.Count > 0)
            {
                var firstTouch = touches[0];
                if (firstTouch.Position.X > WaveServices.Platform.ScreenWidth / 2)
                {
                    currentState = AnimState.Right;
                }
                else
                {
                    currentState = AnimState.Left;
                }
            }

            // Keyboard
            var keyboard = WaveServices.Input.KeyboardState;
            if (keyboard.Right == ButtonState.Pressed)
            {
                currentState = AnimState.Right;
            }
            else if (keyboard.Left == ButtonState.Pressed)
            {
                currentState = AnimState.Left;
            }

            // Set current animation if that one is diferent
            if (currentState != lastState)
            {
                switch (currentState)
                {
                    case AnimState.Idle:
                        anim2D.CurrentAnimation = "Idle";
                        anim2D.Play(true);
                        direction = NONE;
                        break;
                    case AnimState.Right:
                        anim2D = _reverse ? _reverseSaver : _saver;
                        anim2D.CurrentAnimation = "Running";
                        anim2D.Play(true);
                        direction = RIGHT;
                        break;
                    case AnimState.Left:
                        anim2D = _reverse ? _saver : _reverseSaver;
                        anim2D.CurrentAnimation = "Running";
                        anim2D.Play(true);
                        direction = LEFT;
                        break;
                }
            }

            lastState = currentState;

            // Move sprite
            trans2D.X += direction * SPEED * (gameTime.Milliseconds / 10);

            // Check borders
            if (trans2D.X < BORDER_OFFSET)
            {
                trans2D.X = BORDER_OFFSET;
            }
            else if (trans2D.X > WaveServices.Platform.ScreenWidth - BORDER_OFFSET)
            {
                trans2D.X = WaveServices.Platform.ScreenWidth - BORDER_OFFSET;
            }
        }
    }
}

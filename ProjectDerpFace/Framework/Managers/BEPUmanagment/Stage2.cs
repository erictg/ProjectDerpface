using BEPUphysics;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;
using BEPUphysics.Character;
using BEPUphysics.Entities;
using BEPUphysics.Entities.Prefabs;
using ProjectDerpface.Framework.BEPUextensions.BEPUsample;
using BEPUphysics.CollisionRuleManagement;
using System;
using ProjectDerpface.Framework.Cameras;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Ray = BEPUutilities.Ray;
using Vector2 = BEPUutilities.Vector2;
using Vector3 = BEPUutilities.Vector3;
namespace ProjectDerpface.Framework.Managers.BEPUmanagment
{
    /// <summary>
    /// Superclass of the normal demos that let the user
    /// shoot spheres, grab things, create vehicles, 
    /// and walk around as characters.
    /// </summary>
    public abstract class Stage2 : Stage1
    {
        protected FreeCameraControlScheme freeCameraControlScheme;
        protected CharacterControllerInput character;

        protected Texture2D whitePixel;

        public Stage2(Game1 game)
            : base(game)
        {
            freeCameraControlScheme = new FreeCameraControlScheme(10, game.Camera, game);

            //Creates the player character (C).
            character = new CharacterControllerInput(Space, game.Camera, game);

            //Creates the drivable vehicle (V).
            var wheelModel = game.Content.Load<Model>("carWheel");
            var wheelTexture = game.Content.Load<Texture2D>("wheel");
            whitePixel = game.Content.Load<Texture2D>("whitePixel");
            Space.ForceUpdater.Gravity = new Vector3(0, -9.81f, 0f); //If left unset, the default value is (0,0,0).



            //IMPORTANT PERFORMANCE NOTE:
            //  BEPUphysics uses an iterative system to solve constraints.  You can tell it to do more or less iterations.
            //  Less iterations is faster; more iterations makes the result more accurate.
            //
            //  The amount of iterations needed for a simulation varies.  The "Wall" and "Pyramid" simulations are each fairly
            //  solver intensive, but as few as 4 iterations can be used with acceptable results.
            //  The "Jenga" simulation usually needs a few more iterations for stability; 7-9 is a good minimum.
            //
            //  The Dogbot demo shows how accuracy can smoothly increase with more iterations.
            //  With very few iterations (1-3), it has slightly jaggier movement, as if the parts used to construct it were a little cheap.
            //  As you give it a few more iterations, the motors and constraints get more and more robust.
            //  
            //  Many simulations can work perfectly fine with very few iterations, 
            //  and using a low number of iterations can substantially improve performance.
            //
            //  To change the number of iterations used, uncomment and change the following line (10 iterations is the default):

            //Space.Solver.IterationLimit = 10;

            rayCastFilter = RayCastFilter;
        }

        //The raycast filter limits the results retrieved from the Space.RayCast while grabbing.
        Func<BroadPhaseEntry, bool> rayCastFilter;
        bool RayCastFilter(BroadPhaseEntry entry)
        {
            return entry != character.CharacterController.Body.CollisionInformation && entry.CollisionRules.Personal <= CollisionRule.Normal;
        }

        public override void Update(float dt)
        {
            

            base.Update(dt); //Base.update updates the space, which needs to be done before the camera is updated.

            character.Update(dt, Game.PreviousKeyboardInput, Game.KeyboardInput);
            
            //If neither are active, just use the default camera movement style.
            if (!character.IsActive)
                freeCameraControlScheme.Update(dt);
        }

        public override void CleanUp()
        {
            //Wouldn't want the character or vehicle to own the camera after we switch.
            character.Deactivate();
            base.CleanUp();
        }

        public override void DrawUI()
        {
#if !WINDOWS
            if (Game.GamePadInput.IsButtonDown(Buttons.RightShoulder))
#else
            if (Game.MouseInput.RightButton == ButtonState.Pressed)
#endif
                Game.UIDrawer.Draw(whitePixel, new Microsoft.Xna.Framework.Rectangle(Game.Graphics.PreferredBackBufferWidth / 2, Game.Graphics.PreferredBackBufferHeight / 2, 3, 3), Microsoft.Xna.Framework.Color.LightBlue);
        }
    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectDerpface.Framework.SettingsObjects;
using ProjectDerpface.Framework.Managers;
using ProjectDerpface.Framework;
using ProjectDerpface.Framework.Cameras;
using System;
using System.IO;
using System.Collections;
using ProjectDerpface.Framework.BEPUextensions;
using ProjectDerpface.Framework.BEPUextensions.BEPUsample;
using BEPUphysicsDrawer.Font;
using BEPUphysicsDrawer.Lines;
using BEPUphysicsDrawer.Models;
using ConversionHelper;
/*
 * 
 * THINGS I NEED TO DO
 * 1) get an entity to walk
 * 2) get an entity to go down an incline
 * 3) 
 * 
 * 
 */


namespace ProjectDerpface
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        public Camera Camera;

        private InGameManager game;

        //Rendering Variables
        public GraphicsDeviceManager Graphics;

        //Rendering tools
        public ModelDrawer ModelDrawer;
        public LineDrawer ConstraintDrawer;
        public ContactDrawer ContactDrawer;
        public BoundingBoxDrawer BoundingBoxDrawer;
        public SimulationIslandDrawer SimulationIslandDrawer;
        public BasicEffect LineDrawer;
        public SpriteBatch UIDrawer;
        public TextDrawer DataTextDrawer;
        public TextDrawer TinyTextDrawer;

        //FPS calculation variables
        private double FPSlastTime;
        private double FPStotalSinceLast;
        private double FPStoDisplay;
        private double averagePhysicsTime;
        private int FPStotalFramesSinceLast;

        //Input
        public KeyboardState KeyboardInput;
        public KeyboardState PreviousKeyboardInput;
        public GamePadState GamePadInput;
        public GamePadState PreviousGamePadInput;
#if WINDOWS
        public MouseState MouseInput;
        public MouseState PreviousMouseInput;
#endif

        //Display Booleans        
        public bool displayEntities = true;
        public bool displayUI = true;
        public bool displayActiveEntityCount = true;
        public bool displayConstraints = true;
        private bool displayMenu;
        private bool displayContacts;
        private bool displayBoundingBoxes;
        private bool displaySimulationIslands;

       
        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            game = new InGameManager(this);
            Graphics.PreferredBackBufferWidth = 1280;
            Graphics.PreferredBackBufferHeight = 720;
            Camera = new Camera(BEPUutilities.Vector3.Zero, 0, 0, BEPUutilities.Matrix.CreatePerspectiveFieldOfViewRH(MathHelper.PiOver4, Graphics.PreferredBackBufferWidth / (float)Graphics.PreferredBackBufferHeight, .1f, 10000));
            checkForFolders();
            Exiting += DemosGameExiting;
        }

        private void DemosGameExiting(object sender, EventArgs e)
        {
            game.CleanUp();
        }


        protected override void Initialize()
        {
            ModelDrawer = new InstancedModelDrawer(this);

            ConstraintDrawer = new LineDrawer(this);


            ContactDrawer = new ContactDrawer(this);
            BoundingBoxDrawer = new BoundingBoxDrawer(this);
            SimulationIslandDrawer = new SimulationIslandDrawer(this);

            base.Initialize();
        }


        /// <summary>
        /// Manages the switch to a new physics engine simulation.
        /// </summary>
        /// <param name="sim">Index of the simulation to switch to.</param>
        

        protected override void LoadContent()
        {
            
            IsFixedTimeStep = false;

            LineDrawer = new BasicEffect(GraphicsDevice);

            UIDrawer = new SpriteBatch(GraphicsDevice);


#if WINDOWS
            Mouse.SetPosition(200, 200); //This helps the camera stay on track even if the mouse is offset during startup.
#endif

            

        }


        /// <summary>
        /// Determines whether or not the key was pressed this frame.
        /// </summary>
        /// <param name="key">Key to check.</param>
        /// <returns>Whether or not the key was pressed.</returns>
        public bool WasKeyPressed(Keys key)
        {
            return KeyboardInput.IsKeyDown(key) && PreviousKeyboardInput.IsKeyUp(key);
        }

        /// <summary>
        /// Determines whether or not the button was pressed this frame.
        /// </summary>
        /// <param name="button">Button to check.</param>
        /// <returns>Whether or not the button was pressed.</returns>
        public bool WasButtonPressed(Buttons button)
        {
            return GamePadInput.IsButtonDown(button) && PreviousGamePadInput.IsButtonUp(button);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            PreviousKeyboardInput = KeyboardInput;
            KeyboardInput = Keyboard.GetState();
            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
#if WINDOWS
            PreviousMouseInput = MouseInput;
            MouseInput = Mouse.GetState();

            //Keep the mouse within the screen
            if (!IsMouseVisible)
                Mouse.SetPosition(200, 200);
#endif
            PreviousGamePadInput = GamePadInput;
            for (int i = 0; i < 4; i++)
            {
                GamePadInput = GamePad.GetState((PlayerIndex)i);
                if (GamePadInput.IsConnected)
                    break;
            }

            // Allows the default game to exit on Xbox 360 and Windows
            if (KeyboardInput.IsKeyDown(Keys.Escape) || GamePadInput.Buttons.Back == ButtonState.Pressed)
                Exit();

            //Toggle mouse control.  The camera will look to the IsMouseVisible to determine if it should turn.
            if (WasKeyPressed(Keys.Tab))
                IsMouseVisible = !IsMouseVisible;

     

            #region UI Toggles

#if !WINDOWS
            if (WasButtonPressed(Buttons.Start))
            {
                displayMenu = !displayMenu;
            }
#else
            if (WasKeyPressed(Keys.F1))
            {
                displayMenu = !displayMenu;
            }
#endif
            if (WasKeyPressed(Keys.I))
            {
                if (KeyboardInput.IsKeyDown(Keys.RightShift) || KeyboardInput.IsKeyDown(Keys.LeftShift))
                    displayActiveEntityCount = !displayActiveEntityCount;
                else
                    displayUI = !displayUI;
            }
            if (WasKeyPressed(Keys.J))
            {
                displayConstraints = !displayConstraints;
            }
            if (WasKeyPressed(Keys.K))
            {
                displayContacts = !displayContacts;
            }
            if (WasKeyPressed(Keys.U))
            {
                displayBoundingBoxes = !displayBoundingBoxes;
            }
            if (WasKeyPressed(Keys.Y))
            {
                displayEntities = !displayEntities;
            }
            if (WasKeyPressed(Keys.H))
            {
                displaySimulationIslands = !displaySimulationIslands;
            }
            if (WasKeyPressed(Keys.G))
            {
                ModelDrawer.IsWireframe = !ModelDrawer.IsWireframe;
            }

            #endregion

            #region Simulation Switcharoo

#if !WINDOWS

            int switchTo = -2;
            if (WasButtonPressed(Buttons.DPadLeft))
                switchTo = currentSimulationIndex - 1;
            else if (WasButtonPressed(Buttons.DPadRight))
                switchTo = currentSimulationIndex + 1;
            if (switchTo != -2)
            {
                if (switchTo < 1)
                    switchTo += demoTypes.Length;
                else if (switchTo > demoTypes.Length)
                    switchTo = 1;
                SwitchSimulation(switchTo);
            }
#else

           


#endif

            #endregion

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(.41f, .41f, .45f, 1));

            var viewMatrix = Camera.ViewMatrix;
            var projectionMatrix = Camera.ProjectionMatrix;
            if (displayEntities)
                ModelDrawer.Draw(viewMatrix, projectionMatrix);

            if (displayConstraints)
                ConstraintDrawer.Draw(viewMatrix, projectionMatrix);

            LineDrawer.LightingEnabled = false;
            LineDrawer.VertexColorEnabled = true;
            LineDrawer.World = Matrix.Identity;
            LineDrawer.View = MathConverter.Convert(viewMatrix);
            LineDrawer.Projection = MathConverter.Convert(projectionMatrix);

            if (displayContacts)
                ContactDrawer.Draw(LineDrawer, game.Space);

            if (displayBoundingBoxes)
                BoundingBoxDrawer.Draw(LineDrawer, game.Space);

            if (displaySimulationIslands)
                SimulationIslandDrawer.Draw(LineDrawer, game.Space);


            //This doesn't actually draw the elements in the demo (that's the modeldrawer's job),
            //but some demos can specify their own extra stuff to draw.
            game.Draw();

            base.Draw(gameTime);

            #region UI Drawing

            UIDrawer.Begin();
            int bottom = GraphicsDevice.Viewport.Bounds.Height;
            int right = GraphicsDevice.Viewport.Bounds.Width;
            if (displayUI)
            {
                FPStotalSinceLast += gameTime.ElapsedGameTime.TotalSeconds;
                FPStotalFramesSinceLast++;
                if (gameTime.TotalGameTime.TotalSeconds - FPSlastTime > .25 && gameTime.ElapsedGameTime.TotalSeconds > 0)
                {
                    double avg = FPStotalSinceLast / FPStotalFramesSinceLast;
                    FPSlastTime = gameTime.TotalGameTime.TotalSeconds;
                    FPStoDisplay = Math.Round(1 / avg, 1);
                    averagePhysicsTime = Math.Round(1000 * game.PhysicsTime, 1);
                    FPStotalSinceLast = 0;
                    FPStotalFramesSinceLast = 0;
                }

                DataTextDrawer.Draw("FPS: ", FPStoDisplay, new Vector2(50, bottom - 150));
                DataTextDrawer.Draw("Physics Time (ms): ", averagePhysicsTime, new Vector2(50, bottom - 133));
                DataTextDrawer.Draw("Collision Pairs: ", game.Space.NarrowPhase.Pairs.Count, new Vector2(50, bottom - 116));
                if (displayActiveEntityCount)
                {
                    int countActive = 0;
                    for (int i = 0; i < game.Space.Entities.Count; i++)
                    {
                        if (game.Space.Entities[i].ActivityInformation.IsActive)
                            countActive++;
                    }
                    DataTextDrawer.Draw("Active Objects: ", countActive, new Vector2(50, bottom - 99));
                }
#if !WINDOWS
                DataTextDrawer.Draw("Press Start for Controls", new Vector2(50, bottom - 82));
#else
                DataTextDrawer.Draw("Press F1 for Controls", new Vector2(50, bottom - 82));
#endif

                TinyTextDrawer.Draw("Current Simulation: ", 1, new Vector2(right - 200, bottom - 100));
                TinyTextDrawer.Draw(game.Name, new Vector2(right - 180, bottom - 86));

                game.DrawUI();
            }
            UIDrawer.End();

            #endregion
        }
    

        private void checkForFolders()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string mainPath = path + @"\Data\Saves\";
            string settingsPath = path + @"\Data\Saves\Settings\";
            string gameTypePath = path + @"\Data\Saves\GameTypes\";
            string userDataPath = path + @"\Data\Saves\UserData\";
            string audioPath = path + @"\Data\Saves\Settings\audio.settings";


            if (!File.Exists(mainPath))
            {
                Directory.CreateDirectory(mainPath);
                Directory.CreateDirectory(settingsPath);
                Directory.CreateDirectory(gameTypePath);
                Directory.CreateDirectory(userDataPath);
            }
            else
            {
                if (!File.Exists(settingsPath))
                {
                    Directory.CreateDirectory(settingsPath);
                }
                if (!File.Exists(gameTypePath))
                {
                    Directory.CreateDirectory(gameTypePath);
                }
                if (!File.Exists(userDataPath))
                {
                    Directory.CreateDirectory(userDataPath);
                }
            }

            if (!File.Exists(audioPath))
            {
                XmlControls.serializeObject<AudioSettings>(XmlControls.SETTINGS, "audio", new AudioSettings());
            }
        }

    }


}

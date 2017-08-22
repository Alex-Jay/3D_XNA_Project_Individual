using System;
using GDLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GDApp
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Main : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private CameraManager cameraManager;
        private MouseManager mouseManager;
        private KeyboardManager keyboardManager;
        private BasicEffect modelEffect;

        public ObjectManager objectManager { get; private set; }

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            #region Set the screen resolution
            //obviously this will affect the viewport for the camera and does use the same aspect ratio as the camera i.e. 4/3
            int resolutionWidth = 1024, resolutionHeight = 768;
            InitializeGraphics(resolutionWidth, resolutionHeight);
            #endregion

            #region Initialize the effect (shader) for models
            InitializeEffects();
            #endregion

            #region Add the Managers
            bool isMouseVisible = true;
            InitializeManagers(resolutionWidth, resolutionHeight, isMouseVisible);
            #endregion

            #region Add Camera(s)
            InitializeCameras(resolutionWidth, resolutionHeight);
            #endregion

            #region Add ModelObject(s)
            AddDecoratorModelObjects();
            #endregion

            base.Initialize();
        }

        private void AddDecoratorModelObjects()
        {
            //load the texture
            Texture2D texture = Content.Load<Texture2D>("Assets/Textures/Props/Crates/crate1");

            //load the model file i.e. the vertices of the model from the 3DS Max file
            Model boxModel = Content.Load<Model>("Assets/Models/box2");

            //use one of our static defaults to position the object at the origin
            Transform3D transform = Transform3D.Zero;
            //initialise the boxObject
            ModelObject boxObject = new ModelObject("box1", ActorType.Decorator, transform, this.modelEffect, Color.White, 1, texture, boxModel);
            //add to the objectManager so that it will be drawn and updated
            this.objectManager.Add(boxObject);

            //a clone variable that we can reuse
            ModelObject clone = null;

            //add a clone of the box model object to test the clone
            clone = (ModelObject)boxObject.Clone();
            clone.Transform3D.Translation = new Vector3(5, 0, 0);
            //scale it to make it look different
            clone.Transform3D.Scale = new Vector3(1, 4, 1);
            //change its color
            clone.Color = Color.Red;
            this.objectManager.Add(clone);

            //add more clones here...


        }

        private void InitializeCameras(int resolutionWidth, int resolutionHeight)
        {
            Transform3D transform = null;

            //add the camera
            transform = new Transform3D(new Vector3(0, 0, 10), -Vector3.UnitZ, Vector3.UnitY);

            //set the camera to occupy the entire viewport
            Viewport viewPort = new Viewport(0, 0, resolutionWidth, resolutionHeight);
            //initialise the camera
            Camera3D firstPersonCamera = new Camera3D("first person camera 1", ActorType.Camera, transform, ProjectionParameters.StandardMediumFourThree, viewPort, 0, StatusType.Drawn | StatusType.Update);
            //attach a FirstPersonCameraController
            firstPersonCamera.AttachController(new FirstPersonController("firstPersonCameraController1", ControllerType.FirstPerson,
                AppData.CameraMoveKeys, AppData.CameraMoveSpeed, AppData.CameraStrafeSpeed, AppData.CameraRotationSpeed,
                this.mouseManager, this.keyboardManager, this.cameraManager));


            this.cameraManager.Add(firstPersonCamera);
        }

        private void InitializeManagers(int resolutionWidth, int resolutionHeight, bool isMouseVisible)
        {
            this.cameraManager = new CameraManager(this, 1);
            Components.Add(this.cameraManager);

            this.objectManager = new ObjectManager(this, cameraManager, 10);
            Components.Add(this.objectManager);

            //add mouse and keyboard managers
            this.mouseManager = new MouseManager(this, isMouseVisible, /*screen centre*/ new Vector2(resolutionWidth / 2.0f, resolutionHeight / 2.0f));
            Components.Add(this.mouseManager);

            this.keyboardManager = new KeyboardManager(this);
            Components.Add(this.keyboardManager);

        }

        private void InitializeEffects()
        {
            this.modelEffect = new BasicEffect(graphics.GraphicsDevice);
            //enable the use of a texture on a model
            modelEffect.TextureEnabled = true;
            //setup the effect to have a single default light source which will be used to calculate N.L and N.H lighting
            modelEffect.EnableDefaultLighting();
        }

        private void InitializeGraphics(int resolutionWidth, int resolutionHeight)
        {
            graphics.PreferredBackBufferWidth = resolutionWidth;
            graphics.PreferredBackBufferHeight = resolutionHeight;
            //if we forget to apply the changes then our resolution wont be set!
            graphics.ApplyChanges();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}

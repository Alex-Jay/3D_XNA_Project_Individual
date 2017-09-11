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
        #region Variables
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //added property setters in short-hand form for speed
        public ObjectManager objectManager { get; private set; }
        public CameraManager cameraManager { get; private set; }
        public MouseManager mouseManager { get; private set; }
        public KeyboardManager keyboardManager { get; private set; }
        public ScreenManager screenManager { get; private set; }

        private BasicEffect modelEffect;
        private ContentDictionary<Model> modelDictionary;
        private ContentDictionary<Texture2D> textureDictionary;
        private ContentDictionary<SpriteFont> fontDictionary;


        //temp var - remove later
        private ModelObject drivableBoxObject;

#if DEBUG
        private DebugDrawer debugDrawer;
#endif
        #endregion

        #region Properties
        #endregion

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
            //set resolution - see ScreenUtility statics
            Integer2 screenResolution = ScreenUtility.HD720;

            #region Initialize the effect (shader) for models
            InitializeEffects();
            #endregion

            #region Add the Managers
            bool isMouseVisible = true;
            InitializeManagers(screenResolution, isMouseVisible);
            #endregion

            #region Load Content Dictionary
            LoadContentDictionaries();
            #endregion

            #region Add ModelObject(s)
            int worldScale = 100;
            AddWorldDecoratorObjects(worldScale);
            AddControllableModelObjects();
            AddDecoratorModelObjects();
            #endregion

            //we need to move this method because of a dependency with the drivable model object instanciated in AddControllableModelObjects() and the RailController
            #region Add Camera(s)
            InitializeCameras(screenResolution);
            #endregion

            base.Initialize();
        }

        private void LoadContentDictionaries()
        {
            //models
            this.modelDictionary = new ContentDictionary<Model>("model dictionary", this.Content);
            this.modelDictionary.Load("Assets/Models/plane1", "plane1");
            this.modelDictionary.Load("Assets/Models/box2", "box2");

            //textures
            this.textureDictionary = new ContentDictionary<Texture2D>("texture dictionary", this.Content);
            this.textureDictionary.Load("Assets/Textures/Props/Crates/crate1"); //demo use of the shorter form of Load() that generates key from asset name
            this.textureDictionary.Load("Assets/Debug/Textures/checkerboard");    
            this.textureDictionary.Load("Assets/Textures/Foliage/Ground/grass1");
            this.textureDictionary.Load("Assets/Textures/Skybox/back");
            this.textureDictionary.Load("Assets/Textures/Skybox/left");
            this.textureDictionary.Load("Assets/Textures/Skybox/right");
            this.textureDictionary.Load("Assets/Textures/Skybox/sky");
            this.textureDictionary.Load("Assets/Textures/Skybox/front");
            this.textureDictionary.Load("Assets /Textures/Foliage/Trees/tree2");

            //fonts
            this.fontDictionary = new ContentDictionary<SpriteFont>("font dictionary", this.Content);
            this.fontDictionary.Load("Assets/Debug/Fonts/debug");
        }

        private void AddControllableModelObjects()
        {
            #region Add 1st drivable crate

            //place the drivable model to the left of the existing models and specify that forward movement is along the -ve z-axis
            Transform3D transform = new Transform3D(new Vector3(0, 0, 5), -Vector3.UnitZ, Vector3.UnitY);
            
            //initialise the drivable model object - we've made this variable a field to allow it to be visible to the rail camera controller - see InitializeCameras()
            this.drivableBoxObject = new ModelObject("drivable box1", ActorType.Player, transform, this.modelEffect, Color.LightYellow, 1,
                this.textureDictionary["crate1"],
                this.modelDictionary["box2"]);

            //attach a DriveController
            drivableBoxObject.AttachController(new DriveController("driveController1", ControllerType.Drive,
                AppData.PlayerMoveKeys, AppData.PlayerMoveSpeed, AppData.PlayerStrafeSpeed, AppData.PlayerRotationSpeed, this.mouseManager, this.keyboardManager));

            //add to the objectManager so that it will be drawn and updated
            this.objectManager.Add(drivableBoxObject);
            #endregion
        }

        private void AddWorldDecoratorObjects(int worldScale)
        {
            //first we will create a prototype plane and then simply clone it for each of the decorator elements (e.g. ground, sky_top etc). 
            Transform3D transform = new Transform3D(new Vector3(0, -5, 0), new Vector3(worldScale, 1, worldScale));

            ModelObject planePrototypeModelObject = new ModelObject("plane1", ActorType.Decorator, transform, this.modelEffect, Color.White, 1,
                this.textureDictionary["grass1"], 
                this.modelDictionary["plane1"]);

            //will be re-used for all planes
            ModelObject clonePlane = null;

            #region Grass & Skybox
            //add the grass
            clonePlane = (ModelObject)planePrototypeModelObject.Clone();
            clonePlane.Texture = this.textureDictionary["grass1"];
            this.objectManager.Add(clonePlane);

            //add the back skybox plane
            clonePlane = (ModelObject)planePrototypeModelObject.Clone();
            clonePlane.Texture = this.textureDictionary["back"];
            //rotate the default plane 90 degrees around the X-axis (use the thumb and curled fingers of your right hand to determine +ve or -ve rotation value)
            clonePlane.Transform3D.Rotation = new Vector3(90, 0, 0);
            /*
             * Move the plane back to meet with the back edge of the grass (by based on the original 3DS Max model scale)
             * Note:
             * - the interaction between 3DS Max and XNA units which result in the scale factor used below (i.e. 1 x 2.54 x worldScale)/2
             * - that I move the plane down a little on the Y-axiz, purely for aesthetic purposes
             */
            clonePlane.Transform3D.Translation = new Vector3(0, -5, (-2.54f * worldScale)/2.0f);
            this.objectManager.Add(clonePlane);

            //As an exercise the student should add the remaining 4 skybox planes here by repeating the clone, texture assignment, rotation, and translation steps above...
            //add the left skybox plane
            clonePlane = (ModelObject)planePrototypeModelObject.Clone();
            clonePlane.Texture = this.textureDictionary["left"];
            clonePlane.Transform3D.Rotation = new Vector3(90, 90, 0);
            clonePlane.Transform3D.Translation = new Vector3((-2.54f * worldScale) / 2.0f, -5, 0);
            this.objectManager.Add(clonePlane);

            //add the right skybox plane
            clonePlane = (ModelObject)planePrototypeModelObject.Clone();
            clonePlane.Texture = this.textureDictionary["right"];
            clonePlane.Transform3D.Rotation = new Vector3(90, -90, 0);
            clonePlane.Transform3D.Translation = new Vector3((2.54f * worldScale) / 2.0f, -5, 0);
            this.objectManager.Add(clonePlane);

            //add the top skybox plane
            clonePlane = (ModelObject)planePrototypeModelObject.Clone();
            clonePlane.Texture = this.textureDictionary["sky"];
            //notice the combination of rotations to correctly align the sky texture with the sides
            clonePlane.Transform3D.Rotation = new Vector3(180, -90, 0);
            clonePlane.Transform3D.Translation = new Vector3(0, ((2.54f * worldScale) / 2.0f) - 5, 0);
            this.objectManager.Add(clonePlane);

            //add the front skybox plane
            clonePlane = (ModelObject)planePrototypeModelObject.Clone();
            clonePlane.Texture = this.textureDictionary["front"];
            clonePlane.Transform3D.Rotation = new Vector3(-90, 0, 180);
            clonePlane.Transform3D.Translation = new Vector3(0 , -5, (2.54f * worldScale) / 2.0f);
            this.objectManager.Add(clonePlane);
            #endregion

            #region Add Trees
            clonePlane = (ModelObject)planePrototypeModelObject.Clone();
            clonePlane.Texture = this.textureDictionary["tree2"];
            clonePlane.Transform3D.Rotation = new Vector3(90, 0, 0);
            /*
             * ISRoT - Scale operations are applied before rotation in XNA so to make the tree tall (i.e. 10) we actually scale 
             * along the Z-axis (remember the original plane is flat on the XZ axis) and then flip the plane to stand upright.
             */
            clonePlane.Transform3D.Scale = new Vector3(5, 1, 10);
            //y-displacement is (10(XNA) x 2.54f(3DS Max))/2 - 5(ground level) = 7.7f
            clonePlane.Transform3D.Translation = new Vector3(0, ((clonePlane.Transform3D.Scale.Z* 2.54f)/2 - 5), -20);
            this.objectManager.Add(clonePlane);
            #endregion


        }

        private void AddDecoratorModelObjects()
        {
            //use one of our static defaults to position the object at the origin
            Transform3D transform = Transform3D.Zero;

            //loading model, texture

            //initialise the boxObject
            ModelObject boxObject = new ModelObject("some box 1", ActorType.Decorator, transform, this.modelEffect, Color.White, 0.5f,
                this.textureDictionary["crate1"], this.modelDictionary["box2"]);
            //add to the objectManager so that it will be drawn and updated
            //this.objectManager.Add(boxObject);

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

        private void InitializeCameras(Integer2 screenResolution)
        {
            Transform3D transform = null;
            Camera3D camera = null;

            int smallViewPortHeight = 144; //6 small cameras along the left hand side of the main camera view i.e. total height / 5 = 720 / 5 = 144
            int smallViewPortWidth = 5 * smallViewPortHeight/3; //we should try to maintain same ProjectionParameters aspect ratio for small cameras as the large

            #region Initialise the first person camera
            transform = new Transform3D(new Vector3(0, 0, 10), -Vector3.UnitZ, Vector3.UnitY);
            //set the camera to occupy the the full width but only half the height of the full viewport
            Viewport viewPort = ScreenUtility.Pad(new Viewport(0, 0, screenResolution.X, (int)(screenResolution.Y)), smallViewPortWidth, 0, 0, 0);

            camera = new Camera3D("first person camera 1", ActorType.Camera, transform, ProjectionParameters.StandardMediumFiveThree, viewPort, 1, StatusType.Update);
            //attach a FirstPersonCameraController
            camera.AttachController(new FirstPersonCameraController("firstPersonCameraController1", ControllerType.FirstPerson,
                AppData.CameraMoveKeys, AppData.CameraMoveSpeed, AppData.CameraStrafeSpeed, AppData.CameraRotationSpeed, this.mouseManager, this.keyboardManager, this.cameraManager));
            this.cameraManager.Add(camera);
            #endregion

            #region Initialise the 1st security camera
            //it's important to instanciate a new transform and not simply reset the vales on the transform of the first camera. why? if we dont, then modifying one transform will modify the other
            transform = new Transform3D(new Vector3(0, 0, 20), -Vector3.UnitZ, Vector3.UnitY);

            //define a viewport at the top of the main screen e.g. a security camera view - obviously there is relationship between the dimensions of this window and the use of ScreenUtility::Pad() above
            //x-axis position is screen width/2 - 160/2 = 1024/2 - 80 = 432
            viewPort = new Viewport(0, 0, smallViewPortWidth, smallViewPortHeight);

            //create the camera and attachte security controller
            camera = new Camera3D("security camera 1", ActorType.Camera, transform, ProjectionParameters.StandardMediumFourThree, viewPort, 0, StatusType.Update);
            camera.AttachController(new SecurityCameraController("securityCameraController1", ControllerType.Security,
                60, AppData.SecurityCameraRotationSpeedSlow, AppData.SecurityCameraRotationAxisYaw));
            this.cameraManager.Add(camera);
            #endregion

            #region Initialise the 2nd security camera
            //it's important to instanciate a new transform and not simply reset the vales on the transform of the first camera. why? if we dont, then modifying one transform will modify the other
            transform = new Transform3D(new Vector3(0, 0, 20), -Vector3.UnitZ, Vector3.UnitY);

            //x-axis position is right of the previous camera
            viewPort = new Viewport(0, smallViewPortHeight, smallViewPortWidth, smallViewPortHeight);

            //create the camera and attachte security controller
            camera = new Camera3D("security camera 2", ActorType.Camera, transform, ProjectionParameters.StandardMediumFourThree, viewPort, 0, StatusType.Update);
            camera.AttachController(new SecurityCameraController("securityCameraController2", ControllerType.Security,
                45, AppData.SecurityCameraRotationSpeedMedium, new Vector3(1, 1, 0))); //note the rotation axis - this will yaw and pitch
            this.cameraManager.Add(camera);
            #endregion

            #region Initialise the 3rd security camera
            //it's important to instanciate a new transform and not simply reset the vales on the transform of the first camera. why? if we dont, then modifying one transform will modify the other
            transform = new Transform3D(new Vector3(0, 0, 20), -Vector3.UnitZ, Vector3.UnitY);

            //x-axis position is right of the previous camera
            viewPort = new Viewport(0, 2 * smallViewPortHeight, smallViewPortWidth, smallViewPortHeight);

            //create the camera and attach the security controller
            camera = new Camera3D("security camera 3", ActorType.Camera, transform, ProjectionParameters.StandardMediumFourThree, viewPort, 0, StatusType.Update);
            camera.AttachController(new SecurityCameraController("securityCameraController3", ControllerType.Security,
                30, AppData.SecurityCameraRotationSpeedFast, new Vector3(4, 1, 0))); //note the rotation axis - this will yaw and pitch but yaw 4 times for every pitch
            this.cameraManager.Add(camera);
            #endregion

            #region Initialise the track camera
            //it's important to instanciate a new transform and not simply reset the vales on the transform of the first camera. why? if we dont, then modifying one transform will modify the other
            transform = new Transform3D(new Vector3(0, 0, 20), -Vector3.UnitZ, Vector3.UnitY);

            //x-axis position is right of the previous camera
            viewPort = new Viewport(0, 3 * smallViewPortHeight, smallViewPortWidth, smallViewPortHeight);

            //create the camera curve to be applied to the track controller
            Transform3DCurve transform3DCurve = new Transform3DCurve(CurveLoopType.Oscillate);
            transform3DCurve.Add(new Vector3(0, 0, 60), -Vector3.UnitZ, Vector3.UnitY, 0); //start position
            //add more points and make the camera point in other directions here...
            transform3DCurve.Add(new Vector3(0, 20, 0), -Vector3.UnitY, -Vector3.UnitZ, 8); //curve mid-point
            //add more points and make the camera point in other directions here...
            transform3DCurve.Add(new Vector3(0, 0, 60), -Vector3.UnitZ, Vector3.UnitY, 12); //end position - same as start for zero-discontinuity on cycle

            //create the camera and attach the track controller controller
            camera = new Camera3D("track camera 1", ActorType.Camera, transform, ProjectionParameters.StandardMediumFourThree, viewPort, 0, StatusType.Update);
            camera.AttachController(new CurveController("trackCameraController1", ControllerType.Track, transform3DCurve, PlayStatusType.Play));
            this.cameraManager.Add(camera);
            #endregion

            #region Initialise the rail camera
            //remember that the camera will automatically situate itself along the rail, so its initial transform settings are irrelevant
            transform = Transform3D.Zero;

            //x-axis position is right of the previous camera
            viewPort = new Viewport(0, 4 * smallViewPortHeight, smallViewPortWidth, smallViewPortHeight);

            //create the camera curve to be applied to the track controller
            RailParameters railParameters = new RailParameters("rail1 - parallel to x-axis", new Vector3(-20, 10, 40), new Vector3(20, 10, 40));

            //create the camera and attach the track controller controller
            camera = new Camera3D("rail camera 1", ActorType.Camera, transform, ProjectionParameters.StandardMediumFourThree, viewPort, 0, StatusType.Update);
            camera.AttachController(new RailController("railCameraController1", ControllerType.Rail, this.drivableBoxObject, railParameters));
            this.cameraManager.Add(camera);
            #endregion

        }

        private void InitializeManagers(Integer2 screenResolution, bool isMouseVisible)
        {
            this.cameraManager = new CameraManager(this, 1);
            Components.Add(this.cameraManager);

            //create the object manager - notice that its not a drawablegamecomponent. See ScreeManager::Draw()
            this.objectManager = new ObjectManager(this, cameraManager, 10);

            //create the manager which supports multiple camera viewports
            this.screenManager = new ScreenManager(this, graphics, screenResolution, ScreenUtility.ScreenType.MultiScreen, this.objectManager, this.cameraManager);
            Components.Add(this.screenManager);

            //add mouse and keyboard managers
            this.mouseManager = new MouseManager(this, isMouseVisible, /*screen centre*/ new Vector2(screenResolution.X / 2.0f, screenResolution.Y / 2.0f));
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
            //modelEffect.EnableDefaultLighting();
        }

        

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

#if DEBUG
            #region Debug Info
            this.debugDrawer = new DebugDrawer(this, this.screenManager, this.cameraManager, spriteBatch, this.fontDictionary["debug"], 
                Color.White, new Vector2(5, 5));
            Components.Add(this.debugDrawer);
            #endregion
#endif
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            //formally call garbage collection to de-allocate resources from RAM
            this.modelDictionary.Dispose();
            this.textureDictionary.Dispose();
            this.fontDictionary.Dispose();
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

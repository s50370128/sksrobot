using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using StillDesign.PhysX;


namespace Building
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        SpriteFont font;
        //Model grid;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Core _core;
        Scene _scene;
        List<Actor> _boxActor, _wheel;
        List<RevoluteJoint> wheelj;
        Actor box11, box12;
        DistanceJoint boxpick;
        List<FixedJoint> boxes;

        double checkx, checkz;

        bool control5 = false;
        bool control6 = false;

        Model beam;
        Model map;
        Model ballmodel;
        Camera _camera1, _camera2;

        Viewport view1, view2;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //this.graphics.PreferredBackBufferWidth = 480;
            //this.graphics.PreferredBackBufferHeight = 640;
            //this.graphics.IsFullScreen = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            //Initialize the core of physics engine
            CoreDescription coreDesc = new CoreDescription();
            _core = new Core(coreDesc, null);
            //Create a physics scene
            _scene = _core.CreateScene(new StillDesign.PhysX.MathPrimitives.Vector3(0, -9.81f, 0), true);

            _boxActor = new List<Actor>();
            _wheel = new List<Actor>();
            wheelj = new List<RevoluteJoint>();
            boxes = new List<FixedJoint>();

            //Create a material
            MaterialDescription materialDec = new MaterialDescription();
            materialDec.DirectionOfAnisotropy = new StillDesign.PhysX.MathPrimitives.Vector3(0, 0, 1);
            materialDec.DynamicFriction = 0.5f;
            materialDec.DynamicFrictionV = 0.5f;
            materialDec.StaticFriction = 0.5f;
            materialDec.StaticFrictionV = 0.5f;
            materialDec.Restitution = 0.8f;
            materialDec.Flags = MaterialFlag.Ansiotropic;

            Material material1 = _scene.CreateMaterial(materialDec);

            ActorDescription boxADec = new ActorDescription();
            boxADec.BodyDescription = new BodyDescription(5);
            boxADec.Shapes.Add(new BoxShapeDescription(2, 2, 2) { LocalPosition = new StillDesign.PhysX.MathPrimitives.Vector3(0, 1, 0), Material = material1 });
            boxADec.Shapes.Add(new BoxShapeDescription(1, 3, 1) { LocalPosition = new StillDesign.PhysX.MathPrimitives.Vector3(0, 3f, 0), Material = material1 });
            boxADec.Shapes.Add(new BoxShapeDescription(3, 1, 1) { LocalPosition = new StillDesign.PhysX.MathPrimitives.Vector3(-1f, 5, 0), Material = material1 });
            boxADec.GlobalPose = StillDesign.PhysX.MathPrimitives.Matrix.Translation(47f, 1f, 6f);
            _boxActor.Add(_scene.CreateActor(boxADec));

            ActorDescription wheel1 = new ActorDescription();
            wheel1.BodyDescription = new BodyDescription(5);
            wheel1.Shapes.Add(new SphereShapeDescription(1) { Material = material1 });
            wheel1.GlobalPose = StillDesign.PhysX.MathPrimitives.Matrix.Translation(45.3f, 1f, 4.3f);
            _wheel.Add(_scene.CreateActor(wheel1));

            ActorDescription wheel2 = new ActorDescription();
            wheel2.BodyDescription = new BodyDescription(5);
            wheel2.Shapes.Add(new SphereShapeDescription(1) { Material = material1 });
            wheel2.GlobalPose = StillDesign.PhysX.MathPrimitives.Matrix.Translation(45.3f, 1, 7.8f);
            _wheel.Add(_scene.CreateActor(wheel2));

            ActorDescription wheel3 = new ActorDescription();
            wheel3.BodyDescription = new BodyDescription(5);
            wheel3.Shapes.Add(new SphereShapeDescription(1) { Material = material1 });
            wheel3.GlobalPose = StillDesign.PhysX.MathPrimitives.Matrix.Translation(48.7f, 1, 4.3f);
            _wheel.Add(_scene.CreateActor(wheel3));

            ActorDescription wheel4 = new ActorDescription();
            wheel4.BodyDescription = new BodyDescription(5);
            wheel4.Shapes.Add(new SphereShapeDescription(1) { Material = material1 });
            wheel4.GlobalPose = StillDesign.PhysX.MathPrimitives.Matrix.Translation(48.7f, 1, 7.8f);
            _wheel.Add(_scene.CreateActor(wheel4));

            ActorDescription boxADec1 = new ActorDescription();
            boxADec1.BodyDescription = new BodyDescription(1000);
            boxADec1.Shapes.Add(new BoxShapeDescription(35, 10, 1));
            boxADec1.GlobalPose = StillDesign.PhysX.MathPrimitives.Matrix.Translation(33f, 5, 12.1f);
            _boxActor.Add(_scene.CreateActor(boxADec1));

            ActorDescription boxADec2 = new ActorDescription();
            boxADec2.BodyDescription = new BodyDescription(1000);
            boxADec2.Shapes.Add(new BoxShapeDescription(1, 10, 25.5f));
            boxADec2.GlobalPose = StillDesign.PhysX.MathPrimitives.Matrix.Translation(15f, 5, 24.4f);
            _boxActor.Add(_scene.CreateActor(boxADec2));

            ActorDescription boxADec3 = new ActorDescription();
            boxADec3.BodyDescription = new BodyDescription(1000);
            boxADec3.Shapes.Add(new BoxShapeDescription(4f, 10, 1f));
            boxADec3.GlobalPose = StillDesign.PhysX.MathPrimitives.Matrix.Translation(49f, 5, -1f);
            _boxActor.Add(_scene.CreateActor(boxADec3));

            ActorDescription boxADec4 = new ActorDescription();
            boxADec4.BodyDescription = new BodyDescription(1000);
            boxADec4.Shapes.Add(new BoxShapeDescription(1f, 10, 70f));
            boxADec4.GlobalPose = StillDesign.PhysX.MathPrimitives.Matrix.Translation(51f, 5, -3f);
            _boxActor.Add(_scene.CreateActor(boxADec4));

            ActorDescription boxADec5 = new ActorDescription();
            boxADec5.BodyDescription = new BodyDescription(1000);
            boxADec5.Shapes.Add(new BoxShapeDescription(105f, 10, 1f));
            boxADec5.GlobalPose = StillDesign.PhysX.MathPrimitives.Matrix.Translation(0f, 5, 37.7f);
            _boxActor.Add(_scene.CreateActor(boxADec5));

            ActorDescription boxADec6 = new ActorDescription();
            boxADec6.BodyDescription = new BodyDescription(1000);
            boxADec6.Shapes.Add(new BoxShapeDescription(1, 10, 25.5f));
            boxADec6.GlobalPose = StillDesign.PhysX.MathPrimitives.Matrix.Translation(-16.6f, 5, 24.4f);
            _boxActor.Add(_scene.CreateActor(boxADec6));

            ActorDescription boxADec7 = new ActorDescription();
            boxADec7.BodyDescription = new BodyDescription(1000);
            boxADec7.Shapes.Add(new BoxShapeDescription(1, 10, 42f));
            boxADec7.GlobalPose = StillDesign.PhysX.MathPrimitives.Matrix.Translation(-51f, 5, 16.5f);
            _boxActor.Add(_scene.CreateActor(boxADec7));

            ActorDescription boxADec8 = new ActorDescription();
            boxADec8.BodyDescription = new BodyDescription(1000);
            boxADec8.Shapes.Add(new BoxShapeDescription(8.5f, 10, 1f));
            boxADec8.GlobalPose = StillDesign.PhysX.MathPrimitives.Matrix.Translation(-46.25f, 5, 12.15f);
            _boxActor.Add(_scene.CreateActor(boxADec8));

            ActorDescription boxADec9 = new ActorDescription();
            boxADec9.BodyDescription = new BodyDescription(1000);
            boxADec9.Shapes.Add(new BoxShapeDescription(8.5f, 10, 1f));
            boxADec9.GlobalPose = StillDesign.PhysX.MathPrimitives.Matrix.Translation(-21f, 5, 12.15f);
            _boxActor.Add(_scene.CreateActor(boxADec9));

            ActorDescription boxADec10 = new ActorDescription();
            boxADec10.BodyDescription = new BodyDescription(1);
            boxADec10.Shapes.Add(new BoxShapeDescription(1, 1, 1));
            boxADec10.GlobalPose = StillDesign.PhysX.MathPrimitives.Matrix.Translation(-20, 0.5f, 30);
            _boxActor.Add(_scene.CreateActor(boxADec10));

            ActorDescription boxADec11 = new ActorDescription();
            boxADec11.BodyDescription = new BodyDescription(5);
            boxADec11.Shapes.Add(new BoxShapeDescription(1, 1, 1) { Material = material1 });
            boxADec11.GlobalPose = StillDesign.PhysX.MathPrimitives.Matrix.Translation(45, 4, 6);
            box11 = _scene.CreateActor(boxADec11);

            ActorDescription boxADec12 = new ActorDescription();
            boxADec12.BodyDescription = new BodyDescription(1000);
            boxADec12.Shapes.Add(new BoxShapeDescription(4f, 10, 1f));
            boxADec12.GlobalPose = StillDesign.PhysX.MathPrimitives.Matrix.Translation(27.5f, 5, -1f);
            _boxActor.Add(_scene.CreateActor(boxADec12));

            ActorDescription boxADec13 = new ActorDescription();
            boxADec13.BodyDescription = new BodyDescription(1000);
            boxADec13.Shapes.Add(new BoxShapeDescription(21.5f, 10, 1f));
            boxADec13.GlobalPose = StillDesign.PhysX.MathPrimitives.Matrix.Translation(36f, 5, -26.8f);
            _boxActor.Add(_scene.CreateActor(boxADec13));

            ActorDescription boxADec14 = new ActorDescription();
            boxADec14.BodyDescription = new BodyDescription(1000);
            boxADec14.Shapes.Add(new BoxShapeDescription(4f, 10, 1f));
            boxADec14.GlobalPose = StillDesign.PhysX.MathPrimitives.Matrix.Translation(49f, 5, -26.8f);
            _boxActor.Add(_scene.CreateActor(boxADec14));

            ActorDescription boxADec15 = new ActorDescription();
            boxADec15.BodyDescription = new BodyDescription(1000);
            boxADec15.Shapes.Add(new BoxShapeDescription(106f, 10, 1f));
            boxADec15.GlobalPose = StillDesign.PhysX.MathPrimitives.Matrix.Translation(0f, 5, -38.1f);
            _boxActor.Add(_scene.CreateActor(boxADec15));

            ActorDescription boxADec16 = new ActorDescription();
            boxADec16.BodyDescription = new BodyDescription(1000);
            boxADec16.Shapes.Add(new BoxShapeDescription(1, 10, 26.8f));
            boxADec16.GlobalPose = StillDesign.PhysX.MathPrimitives.Matrix.Translation(25f, 5, -13.9f);
            _boxActor.Add(_scene.CreateActor(boxADec16));

            ActorDescription boxADec17 = new ActorDescription();
            boxADec17.BodyDescription = new BodyDescription(1000);
            boxADec17.Shapes.Add(new BoxShapeDescription(17.2f, 0.25f, 1f));
            boxADec17.GlobalPose = StillDesign.PhysX.MathPrimitives.Matrix.Translation(38.1f, 0.125f, -1f);
            _boxActor.Add(_scene.CreateActor(boxADec17));

            ActorDescription boxADec18 = new ActorDescription();
            boxADec18.BodyDescription = new BodyDescription(1000);
            boxADec18.Shapes.Add(new BoxShapeDescription(25f, 0.25f, 24.5f));
            boxADec18.GlobalPose = StillDesign.PhysX.MathPrimitives.Matrix.Translation(38f, 0.25f, -14f);
            _boxActor.Add(_scene.CreateActor(boxADec18));

            ActorDescription boxADec19 = new ActorDescription();
            boxADec19.BodyDescription = new BodyDescription(1000);
            boxADec19.Shapes.Add(new BoxShapeDescription(17.2f, 0.25f, 1f));
            boxADec19.GlobalPose = StillDesign.PhysX.MathPrimitives.Matrix.Translation(38.1f, 0.5f, -2f);
            _boxActor.Add(_scene.CreateActor(boxADec19));

            ActorDescription boxADec20 = new ActorDescription();
            boxADec20.BodyDescription = new BodyDescription(1000);
            boxADec20.Shapes.Add(new BoxShapeDescription(17.2f, 0.5f, 1f));
            boxADec20.GlobalPose = StillDesign.PhysX.MathPrimitives.Matrix.Translation(38.1f, 0.5f, -3f);
            _boxActor.Add(_scene.CreateActor(boxADec20));

            ActorDescription boxADec21 = new ActorDescription();
            boxADec21.BodyDescription = new BodyDescription(1000);
            boxADec21.Shapes.Add(new BoxShapeDescription(17.2f, 0.75f, 1f));
            boxADec21.GlobalPose = StillDesign.PhysX.MathPrimitives.Matrix.Translation(38.1f, 1f, -4f);
            _boxActor.Add(_scene.CreateActor(boxADec21));

            ActorDescription boxADec22 = new ActorDescription();
            boxADec22.BodyDescription = new BodyDescription(1000);
            boxADec22.Shapes.Add(new BoxShapeDescription(17.2f, 1f, 1f));
            boxADec22.GlobalPose = StillDesign.PhysX.MathPrimitives.Matrix.Translation(38.1f, 1.25f, -5f);
            _boxActor.Add(_scene.CreateActor(boxADec22));

            ActorDescription boxADec23 = new ActorDescription();
            boxADec23.BodyDescription = new BodyDescription(1000);
            boxADec23.Shapes.Add(new BoxShapeDescription(25f, 1f, 20.5f));
            boxADec23.GlobalPose = StillDesign.PhysX.MathPrimitives.Matrix.Translation(38f, 1.5f, -16f);
            _boxActor.Add(_scene.CreateActor(boxADec23));

            ActorDescription boxADec24 = new ActorDescription();
            boxADec24.BodyDescription = new BodyDescription(1000);
            boxADec24.Shapes.Add(new BoxShapeDescription(75f, 10, 1f));
            boxADec24.GlobalPose = StillDesign.PhysX.MathPrimitives.Matrix.Translation(-13f, 5, -1f);
            _boxActor.Add(_scene.CreateActor(boxADec24));

            ActorDescription boxADec25 = new ActorDescription();
            boxADec25.BodyDescription = new BodyDescription(5);
            boxADec25.Shapes.Add(new BoxShapeDescription(1, 1, 1) { Material = material1 });
            boxADec25.GlobalPose = StillDesign.PhysX.MathPrimitives.Matrix.Translation(48, 4, 6);
            box12 = _scene.CreateActor(boxADec25);

            RevoluteJointDescription jointDec = new RevoluteJointDescription();
            jointDec.Actor1 = _boxActor[0];
            jointDec.Actor2 = _wheel[0];
            jointDec.SetGlobalAnchor(new StillDesign.PhysX.MathPrimitives.Vector3(45.3f, 1f, 4.3f));
            jointDec.SetGlobalAxis(new StillDesign.PhysX.MathPrimitives.Vector3(0, 0, 1));
            wheelj.Add(_scene.CreateJoint<RevoluteJoint>(jointDec));

            RevoluteJointDescription jointDec1 = new RevoluteJointDescription();
            jointDec1.Actor1 = _boxActor[0];
            jointDec1.Actor2 = _wheel[1];
            jointDec1.SetGlobalAnchor(new StillDesign.PhysX.MathPrimitives.Vector3(45.3f, 1, 7.8f));
            jointDec1.SetGlobalAxis(new StillDesign.PhysX.MathPrimitives.Vector3(0, 0, -1));
            wheelj.Add(_scene.CreateJoint<RevoluteJoint>(jointDec1));

            RevoluteJointDescription jointDec2 = new RevoluteJointDescription();
            jointDec2.Actor1 = _boxActor[0];
            jointDec2.Actor2 = _wheel[2];
            jointDec2.SetGlobalAnchor(new StillDesign.PhysX.MathPrimitives.Vector3(48.7f, 1, 4.3f));
            jointDec2.SetGlobalAxis(new StillDesign.PhysX.MathPrimitives.Vector3(0, 0, 1));
            wheelj.Add(_scene.CreateJoint<RevoluteJoint>(jointDec2));

            RevoluteJointDescription jointDec3 = new RevoluteJointDescription();
            jointDec3.Actor1 = _boxActor[0];
            jointDec3.Actor2 = _wheel[3];
            jointDec3.SetGlobalAnchor(new StillDesign.PhysX.MathPrimitives.Vector3(48.7f, 1, 7.8f));
            jointDec3.SetGlobalAxis(new StillDesign.PhysX.MathPrimitives.Vector3(0, 0, -1));
            wheelj.Add(_scene.CreateJoint<RevoluteJoint>(jointDec3));

            DistanceJointDescription jointDec4 = new DistanceJointDescription();
            jointDec4.Actor1 = _boxActor[10];
            jointDec4.Actor2 = box11;
            jointDec4.MaximumDistance = 1000000;
            jointDec4.MinimumDistance = 1;
            jointDec4.Flags = DistanceJointFlag.EnforceMaximumDistance | DistanceJointFlag.EnforceMinimumDistance;
            boxpick = _scene.CreateJoint<DistanceJoint>(jointDec4);

            FixedJointDescription jointDec5 = new FixedJointDescription();
            jointDec5.Actor1 = _boxActor[0];
            jointDec5.Actor2 = box11;
            boxes.Add(_scene.CreateJoint<FixedJoint>(jointDec5));

            FixedJointDescription jointDec6 = new FixedJointDescription();
            jointDec6.Actor1 = _boxActor[0];
            jointDec6.Actor2 = box12;
            boxes.Add(_scene.CreateJoint<FixedJoint>(jointDec6));
            
            view1 = graphics.GraphicsDevice.Viewport;
            view2 = view1;
            view1.Height = 480;
            view1.Width = 400;
            view1.X = 400;
            view1.Y = 0;
            view2.Height = 480;
            view2.Width = 400;
            view2.X = 0;
            view2.Y = 0;

            _camera1 = new Camera("camera1", 1024, 768, 1024f / 768f, 1, 1f, 100000, true, view1);
            _camera1.Initialize(new Vector3(_boxActor[0].GlobalPosition.X, _boxActor[0].GlobalPosition.Y + 3, _boxActor[0].GlobalPosition.Z), new Vector3(0, 4, 6));
            
            _camera2 = new Camera("camera2", 1024, 768, 1024f / 768f, 1, 1f, 100000, true, view2);
            _camera2.Initialize(new Vector3(_boxActor[0].GlobalPosition.X, _boxActor[0].GlobalPosition.Y + 3, _boxActor[0].GlobalPosition.Z), new Vector3(100, 4, 6));
            this.IsMouseVisible = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            map = Content.Load<Model>("project1_map");
            beam = Content.Load<Model>("box");
            ballmodel = Content.Load<Model>("Sphere");
            float width = (float)graphics.GraphicsDevice.Viewport.Width;
            float height = (float)graphics.GraphicsDevice.Viewport.Height;

            // TODO: use this.Content to load your game content here
            font = Content.Load<SpriteFont>("font");
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
        /// 
        KeyboardState previousState = Keyboard.GetState(); //Save the previous keyboard state
        KeyboardState keyState = Keyboard.GetState();      //Save the current keyboard state

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            // TODO: Add your update logic here
            checkx = Math.Abs(box11.GlobalPosition.X - _boxActor[10].GlobalPosition.X);
            checkz = Math.Abs(box11.GlobalPosition.Z - _boxActor[10].GlobalPosition.Z);

            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.W))
                _camera1.MoveForward(1);
            if (state.IsKeyDown(Keys.S))
                _camera1.MoveForward(-1);
            if (state.IsKeyDown(Keys.A))
                _camera1.MoveHorizontal(1);
            if (state.IsKeyDown(Keys.D))
                _camera1.MoveHorizontal(-1);
            if (state.IsKeyDown(Keys.Q))
                _camera1.Turning(-1);
            if (state.IsKeyDown(Keys.E))
                _camera1.Turning(1);

            if (checkx < 5 && checkz < 5)
            {
                if (state.IsKeyDown(Keys.Z))
                {
                    DistanceJointDescription change1 = boxpick.SaveToDescription();
                    change1.MaximumDistance = 3f;
                    boxpick.LoadFromDescription(change1);
                }
            }
            if (state.IsKeyDown(Keys.X))
            {
                DistanceJointDescription change1 = boxpick.SaveToDescription();
                change1.MaximumDistance = 100000f;
                boxpick.LoadFromDescription(change1);
            }
            
            if (state.IsKeyDown(Keys.Up))
            {
                wheelj[0].Motor = new MotorDescription(-2f, 100, false);
                wheelj[1].Motor = new MotorDescription(2f, 100, false);
                wheelj[2].Motor = new MotorDescription(-2f, 100, false);
                wheelj[3].Motor = new MotorDescription(2f, 100, false);
            }
            else if (state.IsKeyDown(Keys.Down))
            {
                wheelj[0].Motor = new MotorDescription(2f, 100, false);
                wheelj[1].Motor = new MotorDescription(-2f, 100, false);
                wheelj[2].Motor = new MotorDescription(2f, 100, false);
                wheelj[3].Motor = new MotorDescription(-2f, 100, false);
            }
            else if (state.IsKeyDown(Keys.Left))
            {
                wheelj[0].Motor = new MotorDescription(-3f, 100, false);
                wheelj[1].Motor = new MotorDescription(1f, 100, false);
                wheelj[2].Motor = new MotorDescription(-3f, 100, false);
                wheelj[3].Motor = new MotorDescription(1f, 100, false);
            }
            else if (state.IsKeyDown(Keys.Right))
            {
                wheelj[0].Motor = new MotorDescription(-1f, 100, false);
                wheelj[1].Motor = new MotorDescription(3f, 100, false);
                wheelj[2].Motor = new MotorDescription(-1f, 100, false);
                wheelj[3].Motor = new MotorDescription(3f, 100, false);
            }
            else
            {
                wheelj[0].Motor = new MotorDescription(0, 100, false);
                wheelj[1].Motor = new MotorDescription(0, 100, false);
                wheelj[2].Motor = new MotorDescription(0, 100, false);
                wheelj[3].Motor = new MotorDescription(0, 100, false);
            }

            _camera1.UpdateCamera(new Vector3(_boxActor[0].GlobalPosition.X, _boxActor[0].GlobalPosition.Y + 3, _boxActor[0].GlobalPosition.Z), new Vector3(box11.GlobalPosition.X, box11.GlobalPosition.Y, box11.GlobalPosition.Z), Vector3.UnitY);
            _camera2.UpdateCamera(new Vector3(_boxActor[0].GlobalPosition.X, _boxActor[0].GlobalPosition.Y + 3, _boxActor[0].GlobalPosition.Z), new Vector3(box12.GlobalPosition.X, box12.GlobalPosition.Y, box12.GlobalPosition.Z), Vector3.Up);

            //Update the physics engine
            _scene.Simulate((float)gameTime.ElapsedGameTime.TotalSeconds);
            _scene.FlushStream();
            _scene.FetchResults(SimulationStatus.RigidBodyFinished, true);

            base.Update(gameTime);
        }

        /// <summary>
        /// Helper used by the UpdateFire method. Chooses a random location
        /// around a circle, at which a fire particle will be created.
        /// </summary>
        /// 
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Viewport = _camera1.Viewport;
            GraphicsDevice.Clear(Color.CornflowerBlue);
            foreach (Actor ac in _boxActor)
            {
                foreach (BoxShape box in ac.Shapes)
                {
                    DrawTool.DrawModel(beam, StillDesign.PhysX.MathPrimitives.Matrix.Scaling(box.Dimensions * 2) * box.GlobalPose, _camera1);
                }
            }
            DrawTool.DrawModel(ballmodel, StillDesign.PhysX.MathPrimitives.Matrix.Scaling(0.8f, 0.8f, 0.8f) * _wheel[0].GlobalPose, _camera1);
            DrawTool.DrawModel(ballmodel, StillDesign.PhysX.MathPrimitives.Matrix.Scaling(0.8f, 0.8f, 0.8f) * _wheel[1].GlobalPose, _camera1);
            DrawTool.DrawModel(ballmodel, StillDesign.PhysX.MathPrimitives.Matrix.Scaling(0.8f, 0.8f, 0.8f) * _wheel[2].GlobalPose, _camera1);
            DrawTool.DrawModel(ballmodel, StillDesign.PhysX.MathPrimitives.Matrix.Scaling(0.8f, 0.8f, 0.8f) * _wheel[3].GlobalPose, _camera1);
            DrawTool.DrawModel(map, StillDesign.PhysX.MathPrimitives.Matrix.Scaling(10, 1, 10) * StillDesign.PhysX.MathPrimitives.Matrix.Translation(0, 0, 0), _camera1);

            graphics.GraphicsDevice.Viewport = _camera2.Viewport;
            foreach (Actor ac in _boxActor)
            {
                foreach (BoxShape box in ac.Shapes)
                {
                    DrawTool.DrawModel(beam, StillDesign.PhysX.MathPrimitives.Matrix.Scaling(box.Dimensions * 2) * box.GlobalPose, _camera2);
                }
            }
            DrawTool.DrawModel(ballmodel, StillDesign.PhysX.MathPrimitives.Matrix.Scaling(0.8f, 0.8f, 0.8f) * _wheel[0].GlobalPose, _camera2);
            DrawTool.DrawModel(ballmodel, StillDesign.PhysX.MathPrimitives.Matrix.Scaling(0.8f, 0.8f, 0.8f) * _wheel[1].GlobalPose, _camera2);
            DrawTool.DrawModel(ballmodel, StillDesign.PhysX.MathPrimitives.Matrix.Scaling(0.8f, 0.8f, 0.8f) * _wheel[2].GlobalPose, _camera2);
            DrawTool.DrawModel(ballmodel, StillDesign.PhysX.MathPrimitives.Matrix.Scaling(0.8f, 0.8f, 0.8f) * _wheel[3].GlobalPose, _camera2);
            DrawTool.DrawModel(map, StillDesign.PhysX.MathPrimitives.Matrix.Scaling(10, 1, 10) * StillDesign.PhysX.MathPrimitives.Matrix.Translation(0, 0, 0), _camera2);

            base.Draw(gameTime);

        }

        }
    }


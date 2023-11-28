using System.Collections.Generic;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TGC.MonoGame.TP.Actors;
using TGC.MonoGame.TP.Bounds;
using TGC.MonoGame.TP.Components;
using TGC.MonoGame.TP.Components.AI;
using TGC.MonoGame.TP.Components.Graphics;
using TGC.MonoGame.TP.Components.Inputs;
using TGC.MonoGame.TP.Components.Physics;
using TGC.MonoGame.TP.Defaults;
using TGC.MonoGame.TP.HUD;
using TGC.MonoGame.TP.Menu;
using TGC.MonoGame.TP.Physics;
using TGC.MonoGame.TP.Scenes;

namespace TGC.MonoGame.TP
{
    /// <summary>
    ///     Esta es la clase principal del juego.
    ///     Inicialmente puede ser renombrado o copiado para hacer mas ejemplos chicos, en el caso de copiar para que se
    ///     ejecute el nuevo ejemplo deben cambiar la clase que ejecuta Program <see cref="Program.Main()" /> linea 10.
    /// </summary>
    public class TGCGame : Game
    {

        /// <summary>
        ///     Constructor del juego.
        /// </summary>
        /// 

        public TGCGame()
        {
            // Maneja la configuracion y la administracion del dispositivo grafico.
            Graphics = new GraphicsDeviceManager(this);

            Graphics.GraphicsProfile = GraphicsProfile.HiDef;

            // Para que el juego sea pantalla completa se puede usar Graphics IsFullScreen.
            // Carpeta raiz donde va a estar toda la Media.
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            GizmosRenderer = Gizmos.Gizmos.GetInstance();
        }
        public Gizmos.Gizmos GizmosRenderer { get; }
        private GraphicsDeviceManager Graphics { get; }
        private MenuComponent Menu { get; set; }
        private HUDComponent HUD { get; set; }
        private SpriteBatch SpriteBatch { get; set; }
        private BoundsComponent Bounds { get; set; }
        private Terrain Terrain;
        public bool IsMenuActive { get; set; }

        public const string ContentFolderEffects = "Effects/";
        private MouseCamera MouseCamera { get; set; }
        private Song MainSong { get; set; }
        private SoundEffectInstance Instance { get; set; }
        private SoundEffect SoundEffect { get; set; }
        private List<GameObject> Objects { get; set; }
        private Forest Forest { get; set; }
        private GameObject Player { get; set; }
        private List<GameObject> TeamPanzer { get; set; }
        private List<GameObject> TeamT90 { get; set; }
        private List<GameObject> Collisionables { get; set; }
        private PhysicsEngine PhysicsEngine { get; set; }
        private const int TEAMS_SIZE = 5;

        protected override void Initialize()
        {

            //Pantalla Completa
            Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 100;
            Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100;
            Graphics.ApplyChanges();

            MainSong = Content.Load<Song>("Audio/mainSong");
            SoundEffect = Content.Load<SoundEffect>("Audio/warsounds");
            SoundEffect.MasterVolume -= .9f;
            MediaPlayer.Volume -= .7f;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(MainSong);
            Instance = SoundEffect.CreateInstance();

            IsMenuActive = true;

            PhysicsEngine = new PhysicsEngine();
            PhysicsEngine.Initialize();

            //Cambio textura de cursor
            Mouse.SetCursor(MouseCursor.FromTexture2D(Content.Load<Texture2D>("Textures/Menu/cursor"), 0, 0));

            Menu = new MenuComponent(this);
            HUD = new HUDComponent(PlayerDefaults.TankName, PlayerDefaults.Health, PlayerDefaults.CoolDown);
            Terrain = new Terrain(Content, GraphicsDevice, "Textures/heightmaps/hills-heightmap", "Textures/heightmaps/hills", 20.0f, 8.0f);
            MouseCamera = new MouseCamera(GraphicsDevice);
            Bounds = new BoundsComponent(Content, Terrain, PhysicsEngine);
            var panzerBox = new Box(500f, 250f, 1000f);
            var t90Box = new Box(500f, 250f, 1000f);
            var playerPosition = new Vector3(0f, Terrain.Height(0f, 0f), 0f);
            var physicsComponent = new DynamicPhysicsComponent<Box>(
                PhysicsEngine,
                "Panzer Box",
                panzerBox,
                playerPosition,
                Quaternion.CreateFromAxisAngle(Vector3.Up, PlayerDefaults.RotationAngle),
                Terrain);
            Player = new GameObject(
                new List<IComponent>() { new TankInputComponent(PlayerDefaults.DriveSpeed, PlayerDefaults.RotationSpeed, PlayerDefaults.CoolDown, MouseCamera, Terrain, HUD, physicsComponent) },
                new PanzerGraphicsComponent(),
                physicsComponent,
                PlayerDefaults.Scale,
                PlayerDefaults.Health
            );

            Objects = new List<GameObject>() { Player };
            Collisionables = new List<GameObject>();

            TeamPanzer = new List<GameObject>();
            TeamT90 = new List<GameObject>();

            for (var i = 0; i <= TEAMS_SIZE; i++)
            {
                var panzerPosition = new Vector3(2000f, Terrain.Height(2000f, 2000f + 1000f * i), 2000f + 1000f * i);
                var panzerPhysicsComponent = new DynamicPhysicsComponent<Box>(
                    PhysicsEngine,
                    "Panzer Box",
                    panzerBox,
                    panzerPosition,
                    Quaternion.CreateFromAxisAngle(Vector3.Up, PlayerDefaults.RotationAngle),
                    Terrain);
                var panzer = new GameObject(
                    new PanzerGraphicsComponent(),
                    panzerPhysicsComponent,
                    PlayerDefaults.Scale,
                    PlayerDefaults.Health
                );

                var t90Position = new Vector3(-2000f, Terrain.Height(-2000f, -2000f + 1000f * i), -2000f + 1000f * i);
                var t90PhysicsComponent = new DynamicPhysicsComponent<Box>(
                    PhysicsEngine,
                    "T90 Box",
                    t90Box,
                    t90Position,
                    Quaternion.CreateFromAxisAngle(Vector3.Up, PlayerDefaults.RotationAngle),
                    Terrain);
                var t90 = new GameObject(
                    new T90GraphicsComponent(),
                    t90PhysicsComponent,
                    PlayerDefaults.Scale,
                    PlayerDefaults.Health
                );

                panzer.AddComponent(new AITankComponent(PlayerDefaults.DriveSpeed, PlayerDefaults.RotationSpeed, PlayerDefaults.CoolDown, 5000, t90, Collisionables, Terrain, panzerPhysicsComponent));
                t90.AddComponent(new AITankComponent(PlayerDefaults.DriveSpeed, PlayerDefaults.RotationSpeed, PlayerDefaults.CoolDown, 5000, panzer, Collisionables, Terrain, t90PhysicsComponent));

                TeamPanzer.Add(panzer);
                TeamT90.Add(t90);

                Collisionables.Add(panzer);
                Collisionables.Add(t90);

            }

            Forest = new Forest(ForestDefaults.Center, ForestDefaults.Radius, 0f, PhysicsEngine);

            base.Initialize();
        }


        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Menu.LoadContent(Content, GraphicsDevice);
            HUD.LoadContent(Content);
            Terrain.LoadContent(Content, GraphicsDevice);
            Bounds.LoadContent(Content);
            Forest.LoadContent(Content, Terrain, Objects);
            GizmosRenderer.LoadContent(GraphicsDevice, Content);
            Player.LoadContent(Content);

            foreach (var t90 in TeamT90) t90.LoadContent(Content);
            foreach (var panzer in TeamPanzer) panzer.LoadContent(Content);

            base.LoadContent();
        }


        protected override void Update(GameTime gameTime)
        {
            PhysicsEngine.Update(gameTime);

            // Capturar Input teclado
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                //Salgo del juego.
                Exit();
            }

            if (!IsMenuActive)
            {
                MediaPlayer.Volume = .1f;

                //MediaPlayer.Stop();
                Instance.Play();
            }

            Menu.Update();
            MouseCamera.Update(gameTime, Player.World, IsMenuActive);

            foreach (var obj in Objects)
            {
                obj.Update(gameTime);
            }

            Bounds.Update(gameTime);
            GizmosRenderer.UpdateViewProjection(MouseCamera.View, MouseCamera.Projection);

            foreach (var t90 in TeamT90) t90.Update(gameTime);
            foreach (var panzer in TeamPanzer) panzer.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            Terrain.Draw(GraphicsDevice, MouseCamera.View, MouseCamera.Projection);
            Bounds.Draw(gameTime, MouseCamera.View, MouseCamera.Projection, GizmosRenderer);
            foreach (var obj in Objects)
            {
                obj.Draw(gameTime, MouseCamera.View, MouseCamera.Projection, GizmosRenderer);
            }
            foreach (var t90 in TeamT90) t90.Draw(gameTime, MouseCamera.View, MouseCamera.Projection, GizmosRenderer);
            foreach (var panzer in TeamPanzer) panzer.Draw(gameTime, MouseCamera.View, MouseCamera.Projection, GizmosRenderer);
            GizmosRenderer.SetColor(Color.Yellow);
            GizmosRenderer.Draw();
            if (IsMenuActive) Menu.Draw(SpriteBatch); else HUD.Draw(SpriteBatch);

        }
        protected override void UnloadContent()
        {
            Content.Unload();
            Terrain.UnloadContent();

            base.UnloadContent();
        }
    }
}
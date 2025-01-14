﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Xml.Linq;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TGC.MonoGame.TP.Actors;
using TGC.MonoGame.TP.Bounds;
using TGC.MonoGame.TP.Collisions;
using TGC.MonoGame.TP.Components;
using TGC.MonoGame.TP.Components.AI;
using TGC.MonoGame.TP.Components.Collisions;
using TGC.MonoGame.TP.Components.Graphics;
using TGC.MonoGame.TP.Components.Inputs;
using TGC.MonoGame.TP.Controllers;
using TGC.MonoGame.TP.Defaults;
using TGC.MonoGame.TP.Helpers;
using TGC.MonoGame.TP.HUD;
using TGC.MonoGame.TP.Menu;
using TGC.MonoGame.TP.Scenes;
using static System.Formats.Asn1.AsnWriter;

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

            Gizmos = new Gizmos.Gizmos();
        }
        public Gizmos.Gizmos Gizmos { get; }
        private GraphicsDeviceManager Graphics { get; }
        private MenuComponent Menu { get; set; }
        private HUDComponent HUD { get; set; }
        private SpriteBatch SpriteBatch { get; set; }
        private BoundsComponent Bounds { get; set; }
        private Terrain Terrain;
        private SkyBox SkyBox;
        public bool IsMenuActive { get; set; }
        
        public const string ContentFolderEffects = "Effects/";
        public MouseCamera MouseCamera { get; set; }
        private Song MainSong { get; set; }
        private SoundEffectInstance Instance { get; set; }
        private SoundEffect SoundEffect { get; set; }
        private List<GameObject> Objects { get; set; }
        private Forest Forest { get; set; }
        public GameObject Player { get; set; }
        private List<GameObject> TeamPanzer { get; set; }
        private List<GameObject> TeamT90 { get; set; }
        private List<GameObject> Collisionables { get; set; }
        private BoundingFrustum BoundingFrustum { get; set; }

        private RenderTarget2D HorizontalRenderTarget;

        private RenderTarget2D MainRenderTarget;
        private FullScreenQuad FullScreenQuad;
        private Effect FullScreenQuadEffect;
        private HeightMap HeightMap;

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
            
            //Cambio textura de cursor
            Mouse.SetCursor(MouseCursor.FromTexture2D(Content.Load<Texture2D>("Textures/Menu/cursor"), 0, 0));

            HUD = new HUDComponent(this, PlayerDefaults.TankName, PlayerDefaults.Health, PlayerDefaults.CoolDown, TeamPanzer);
            Terrain = new Terrain(Content, GraphicsDevice, "Textures/heightmaps/hills-heightmap", "Textures/heightmaps/hills", 20.0f, 8.0f);
            HeightMap = new HeightMap(Content, GraphicsDevice, "Textures/heightmaps/heightmap", "Textures/heightmaps/colormap",
                "Textures/heightmaps/greenGrass", "Textures/heightmaps/ground", 2000, 3);
            MouseCamera = new MouseCamera(GraphicsDevice);
            Bounds = new BoundsComponent(Content, HeightMap);

            Player = new GameObject(
                new List<IComponent>() { new TankInputComponent(PlayerDefaults.DriveSpeed, PlayerDefaults.RotationSpeed, PlayerDefaults.CoolDown, MouseCamera, HeightMap, HUD ) },
                new PanzerGraphicsComponent(),
                new PanzerCollisionComponent(),
                new Vector3(0f, HeightMap.Height(0f, 0f), 0f),
                PlayerDefaults.RotationAngle,
                Vector3.Up,
                PlayerDefaults.Scale,
                PlayerDefaults.Health
            );
            Menu = new MenuComponent(this, Player.Health);

            Objects = new List<GameObject>() { Player };
            Collisionables = new List<GameObject>();

            TeamPanzer = new List<GameObject>();
            TeamT90 = new List<GameObject>();

            for(var i = 0; i <= TEAMS_SIZE; i++)
            {
                var panzer = new GameObject(
                    new PanzerGraphicsComponent(),
                    new PanzerCollisionComponent(),
                    new Vector3(2000f, HeightMap.Height(2000f, 2000f + 1000f * i), 2000f + 1000f * i),
                    PlayerDefaults.RotationAngle,
                    PlayerDefaults.Scale,
                    PlayerDefaults.Health
                );

                var t90 = new GameObject(
                    new T90GraphicsComponent(),
                    new T90CollisionComponent(),
                    new Vector3(-2000f, HeightMap.Height(-2000f, -2000f + 1000f * i), -2000f + 1000f * i),
                    PlayerDefaults.RotationAngle,
                    PlayerDefaults.Scale,
                    PlayerDefaults.Health
                );

                TeamPanzer.Add(panzer);
                TeamT90.Add(t90);

                
                Collisionables.Add(panzer);
                Collisionables.Add(t90);

                panzer.AddComponent(new AITankComponent(PlayerDefaults.DriveSpeed, PlayerDefaults.RotationSpeed, PlayerDefaults.CoolDown, 5000, TeamT90, t90, Collisionables, HeightMap));
                t90.AddComponent(new AITankComponent(PlayerDefaults.DriveSpeed, PlayerDefaults.RotationSpeed, PlayerDefaults.CoolDown, 5000, TeamPanzer, panzer, Collisionables, HeightMap));

            }
            SkyBox = new SkyBox("Models/skybox/cube", "Textures/skyboxes/skybox/skybox", 50000f);
            Forest = new Forest(ForestDefaults.Center, ForestDefaults.Radius, ForestDefaults.Density);
            BoundingFrustum = new BoundingFrustum(MouseCamera.View * MouseCamera.Projection);

            base.Initialize();
        }


        protected override void LoadContent()
        {

            FullScreenQuadEffect = Content.Load<Effect>(ContentFolderEffects + "FullScreenQuad");
            FullScreenQuadEffect.Parameters["screenSize"]
               .SetValue(new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));

            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Menu.LoadContent(Content, GraphicsDevice);
            HUD.LoadContent(Content, GraphicsDevice);
            MouseCamera.LoadContent(Content);
            FullScreenQuad = new FullScreenQuad(GraphicsDevice);

            MainRenderTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8, 0,
                RenderTargetUsage.DiscardContents);
            HorizontalRenderTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height, false, SurfaceFormat.Color, DepthFormat.None, 0,
                RenderTargetUsage.DiscardContents);


            HeightMap.LoadContent(Content, GraphicsDevice);
            Terrain.LoadContent(Content, GraphicsDevice);
            Bounds.LoadContent(Content);
            Player.LoadContent(Content);
            Forest.LoadContent(Content, HeightMap, Objects);
            Gizmos.LoadContent(GraphicsDevice, Content);

            foreach (var t90 in TeamT90) t90.LoadContent(Content);
            foreach (var panzer in TeamPanzer) panzer.LoadContent(Content);

            SkyBox.LoadContent(Content);

            base.LoadContent();
        }


        protected override void Update(GameTime gameTime)
        {
            // Aca deberiamos poner toda la logica de actualizacion del juego.

            // Capturar Input teclado
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                //Salgo del juego.
                Exit();
            }

            if(!IsMenuActive)
            {
                HUD.Update();
                if (MediaPlayer.Volume != 0) MediaPlayer.Volume = .1f;
                Instance.Play();
            } else {
                Menu.Update();
            }


            if (Player.Health > 0 && TeamT90.Count != 0)
            {
                MouseCamera.Update(gameTime, Player.World, IsMenuActive);
            } else
            {
                IsMouseVisible = true;
            }

            foreach (var obj in Objects)
            {
                obj.Update(gameTime);
            }

      
            BoundingFrustum.Matrix = MouseCamera.View * MouseCamera.Projection;

            Bounds.Update(gameTime);
            Gizmos.UpdateViewProjection(MouseCamera.View, MouseCamera.Projection);

            foreach (var t90 in TeamT90) t90.Update(gameTime);
            foreach (var panzer in TeamPanzer) panzer.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (IsMenuActive) DrawWithScreenBlur(gameTime);
            else DrawDefault(gameTime);
        }
        protected override void UnloadContent()
        {
            Content.Unload();
            Terrain.UnloadContent();

            base.UnloadContent();
        }

        protected void DrawDefault(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.BlendState = BlendState.Opaque;


            RasterizerState originalRasterizerState = GraphicsDevice.RasterizerState;
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;

            SkyBox.Draw(MouseCamera.View, MouseCamera.Projection, MouseCamera.OffsetPosition);
            HeightMap.Draw(GraphicsDevice, Matrix.Identity, MouseCamera.View, MouseCamera.Projection);
            GraphicsDevice.RasterizerState = originalRasterizerState;


            //Terrain.Draw(GraphicsDevice, MouseCamera.View, MouseCamera.Projection, MouseCamera.OffsetPosition, "Default");
            Bounds.Draw(gameTime, MouseCamera.View, MouseCamera.Projection, MouseCamera.OffsetPosition);

            foreach (var obj in Objects)
            {
                if (BoundingFrustum.Intersects(obj.CollisionComponent.BoxWorldSpace))
                {
                    obj.Draw(gameTime, MouseCamera.View, MouseCamera.Projection, MouseCamera.OffsetPosition);
                }
            }
            foreach (var t90 in TeamT90) t90.Draw(gameTime, MouseCamera.View, MouseCamera.Projection, MouseCamera.OffsetPosition);
            foreach (var panzer in TeamPanzer) panzer.Draw(gameTime, MouseCamera.View, MouseCamera.Projection, MouseCamera.OffsetPosition);

            Gizmos.Draw();

            MouseCamera.Draw(SpriteBatch);
            HUD.Draw(SpriteBatch, Player.Health, TeamT90);

        }
        protected void DrawWithScreenBlur(GameTime gameTime)
        {
            #region Main Render Target
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.BlendState = BlendState.Opaque;

            GraphicsDevice.SetRenderTarget(MainRenderTarget);
            GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.CornflowerBlue, 1f, 0);

            RasterizerState originalRasterizerState = GraphicsDevice.RasterizerState;
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;
            SkyBox.Draw(MouseCamera.View, MouseCamera.Projection, MouseCamera.OffsetPosition);
            HeightMap.Draw(GraphicsDevice, Matrix.Identity, MouseCamera.View, MouseCamera.Projection);
            GraphicsDevice.RasterizerState = originalRasterizerState;

            // Set the main render target as our render target


            //Terrain.Draw(GraphicsDevice, MouseCamera.View, MouseCamera.Projection, MouseCamera.OffsetPosition, "Default");
            HeightMap.Draw(GraphicsDevice, Matrix.Identity, MouseCamera.View, MouseCamera.Projection);
            Bounds.Draw(gameTime, MouseCamera.View, MouseCamera.Projection, MouseCamera.OffsetPosition);
            foreach (var obj in Objects)
            {
                if (BoundingFrustum.Intersects(obj.CollisionComponent.BoxWorldSpace))
                {
                    obj.Draw(gameTime, MouseCamera.View, MouseCamera.Projection, MouseCamera.OffsetPosition);
                }
            }
            foreach (var t90 in TeamT90) t90.Draw(gameTime, MouseCamera.View, MouseCamera.Projection, MouseCamera.OffsetPosition);
            foreach (var panzer in TeamPanzer) panzer.Draw(gameTime, MouseCamera.View, MouseCamera.Projection, MouseCamera.OffsetPosition);

            Gizmos.Draw();

            #endregion

            #region BlurHorizontal

            GraphicsDevice.DepthStencilState = DepthStencilState.None;

            GraphicsDevice.SetRenderTarget(HorizontalRenderTarget);
            GraphicsDevice.Clear(Color.Black);


            FullScreenQuadEffect.CurrentTechnique = FullScreenQuadEffect.Techniques["BlurHorizontalTechnique"];
            FullScreenQuadEffect.Parameters["ModelTexture"].SetValue(MainRenderTarget);
            FullScreenQuad.Draw(FullScreenQuadEffect);


            #endregion

            #region BlurVerticalTechnique
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);

            FullScreenQuadEffect.CurrentTechnique = FullScreenQuadEffect.Techniques["BlurVerticalTechnique"];
            FullScreenQuadEffect.Parameters["ModelTexture"].SetValue(HorizontalRenderTarget);
            FullScreenQuad.Draw(FullScreenQuadEffect);
            Menu.Draw(SpriteBatch);
            #endregion

        }

        private void DetectCollisions()
        {
            var collision = false;
            for (int i = 0; i < Objects.Count; i++)
            {
                if (Objects[i].CollidesWith(Player) && Objects[i] != Player )
                {
                    Objects.Remove(Objects[i]);
                    collision = true;
                    break;
                }
            }
            var color = collision ? Color.Red : Color.Yellow;
            
            Gizmos.SetColor(color);
            Gizmos.UpdateViewProjection(MouseCamera.View, MouseCamera.Projection);
        }
    }
}
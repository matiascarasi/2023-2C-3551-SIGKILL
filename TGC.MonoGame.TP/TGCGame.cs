﻿using System;
using System.Linq;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Cameras;
using TGC.MonoGame.TP.Components;
using TGC.MonoGame.TP.Content.Actors;
using TGC.MonoGame.TP.Controllers;
using TGC.MonoGame.TP.Defaults;

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
        public TGCGame()
        {
            // Maneja la configuracion y la administracion del dispositivo grafico.
            Graphics = new GraphicsDeviceManager(this);
            // Para que el juego sea pantalla completa se puede usar Graphics IsFullScreen.
            // Carpeta raiz donde va a estar toda la Media.
            Content.RootDirectory = "Content";
            // Hace que el mouse sea visible.
            IsMouseVisible = true;
        }

        private GraphicsDeviceManager Graphics { get; }
        private SpriteBatch SpriteBatch { get; set; }
        private Matrix View { get; set; }
        private Matrix Projection { get; set; }
        private GameObject Player { get; set; }
        private FollowCamera FollowCamera { get; set; }
        private Terrain Terrain;
        
        public const string ContentFolderEffects = "Effects/";

        private Effect Effect { get; set; }
        
        private GameObject Box { get; set; }
        
        private float Rotation { get; set; }
        private Matrix World { get; set; }
        
        private Model Model { get; set; }

        
        private Camera Camera { get; set; }

        
        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aqui el codigo de inicializacion: el procesamiento que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void Initialize()
        {
            // La logica de inicializacion que no depende del contenido se recomienda poner en este metodo.

            // Apago el backface culling.
            // Esto se hace por un problema en el diseno del modelo del logo de la materia.
            // Una vez que empiecen su juego, esto no es mas necesario y lo pueden sacar.
            var rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;
            // Seria hasta aca.

            // Configuramos nuestras matrices de la escena.
            View = Matrix.CreateLookAt(new Vector3(0f, 2500f, 5000f), Vector3.Zero, Vector3.Up);
            Projection =
                Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1, 200000);
            FollowCamera = new FollowCamera(GraphicsDevice.Viewport.AspectRatio);

            Terrain = new Terrain("Textures/heightmaps/hills-heightmap", "Textures/heightmaps/hills");

            Player = new GameObject(
                new TankGraphicsComponent(Content, PlayerDefaults.TankName), 
                new PlayerInputComponent(PlayerDefaults.DriveSpeed, PlayerDefaults.RotationSpeed), 
                PlayerDefaults.Position, 
                PlayerDefaults.YAxisRotation, 
                PlayerDefaults.Scale
            );
            
            // Box = new GameObject(
            //     new MiscellaneousGraphicsComponent(Content, "Rock", "Rock07-Base"),
            //     new PlayerInputComponent(0f, 0f),
            //     new Vector3(0f, 300f, 0f),
            //     0f,
            //     1f
            // );

            Camera = new SimpleCamera(GraphicsDevice.Viewport.AspectRatio, new Vector3(-400f, 1000f, 2000f), 400, 1.0f, 1,
                int.MaxValue);

            
            base.Initialize();
        }

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo, despues de Initialize.
        ///     Escribir aqui el codigo de inicializacion: cargar modelos, texturas, estructuras de optimizacion, el procesamiento
        ///     que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void LoadContent()
        {
            // Aca es donde deberiamos cargar todos los contenido necesarios antes de iniciar el juego.
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            Terrain.LoadContent(Content, GraphicsDevice);
            
            Player.LoadContent();
            
            //Box.LoadContent();
            
            base.LoadContent();
        }

        /// <summary>
        ///     Se llama en cada frame.
        ///     Se debe escribir toda la logica de computo del modelo, asi como tambien verificar entradas del usuario y reacciones
        ///     ante ellas.
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            // Aca deberiamos poner toda la logica de actualizacion del juego.

            // Capturar Input teclado
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                //Salgo del juego.
                Exit();
            }
            Camera.Update(gameTime);

            Player.Update(gameTime);
            
            base.Update(gameTime);
        }

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aqui el codigo referido al renderizado.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            // Aca deberiamos poner toda la logia de renderizado del juego.
            GraphicsDevice.Clear(Color.Blue);
            Terrain.Draw(GraphicsDevice, Camera.View, Camera.Projection);
            Player.Draw(gameTime, Camera.View, Camera.Projection);
           // Box.Draw(gameTime, Camera.View, Camera.Projection);

        }
        

        /// <summary>
        ///     Libero los recursos que se cargaron en el juego.
        /// </summary>
        protected override void UnloadContent()
        {
            // Libero los recursos.
            Content.Unload();
            Terrain.UnloadContent();

            base.UnloadContent();
        }
    }
}
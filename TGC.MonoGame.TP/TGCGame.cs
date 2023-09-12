using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Components;
using TGC.MonoGame.TP.Content.Actors;
using TGC.MonoGame.TP.Content.Models;
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

            Graphics.GraphicsProfile = GraphicsProfile.HiDef;

            // Para que el juego sea pantalla completa se puede usar Graphics IsFullScreen.
            // Carpeta raiz donde va a estar toda la Media.
            Content.RootDirectory = "Content";
            // Hace que el mouse sea visible.
            IsMouseVisible = true;
        }
        private CityScene City { get; set; } // CIUDAD TP0
        private GraphicsDeviceManager Graphics { get; }
        private SpriteBatch SpriteBatch { get; set; }
        private Matrix View { get; set; }
        private Matrix Projection { get; set; }
        private GameObject Player { get; set; }
        private GameObject Box { get; set; }
        private MouseCamera MouseCamera { get; set; }

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aqui el codigo de inicializacion: el procesamiento que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void Initialize()
        {

            IsMouseVisible = true;
            // La logica de inicializacion que no depende del contenido se recomienda poner en este metodo.

            // Apago el backface culling
            // Seria hasta aca.

            // Configuramos nuestras matrices de la escena.
            View = Matrix.CreateLookAt(Vector3.UnitZ * 1500, Vector3.Zero, Vector3.Up);
            Projection =
                Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1, 2000);
            MouseCamera = new MouseCamera(GraphicsDevice);

            Player = new GameObject(
                new TankGraphicsComponent(Content, PlayerDefaults.TankName),
                new PlayerInputComponent(PlayerDefaults.DriveSpeed, PlayerDefaults.RotationSpeed),
                PlayerDefaults.Position,
                PlayerDefaults.YAxisRotation,
                PlayerDefaults.Scale
            );

            //OBJETO MODELO EJEMPLO
            Box = new GameObject(
               new MiscellaneousGraphicsComponent(Content, "Rock", "Rock07-Base"),
               new PlayerInputComponent(0f, 0f),
               new Vector3(0f,0f, 0f),
               0f,
               1f
           );

            base.Initialize();
        }

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo, despues de Initialize.
        ///     Escribir aqui el codigo de inicializacion: cargar modelos, texturas, estructuras de optimizacion, el procesamiento
        ///     que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void LoadContent()
        {

            City = new CityScene(Content); //CARGO MODELO CIUDAD 

            // Aca es donde deberiamos cargar todos los contenido necesarios antes de iniciar el juego.
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            Player.LoadContent();
            Box.LoadContent();

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

            Player.Update(gameTime);
            MouseCamera.Update(gameTime, Player.World);

            base.Update(gameTime);
        }

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aqui el codigo referido al renderizado.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            // Aca deberiamos poner toda la logia de renderizado del juego.
            GraphicsDevice.Clear(Color.Black);
            City.Draw(gameTime, MouseCamera.View, MouseCamera.Projection);
            Player.Draw(gameTime, MouseCamera.View, MouseCamera.Projection);
            Box.Draw(gameTime, MouseCamera.View, MouseCamera.Projection);
        }

        /// <summary>
        ///     Libero los recursos que se cargaron en el juego.
        /// </summary>
        protected override void UnloadContent()
        {
            // Libero los recursos.
            Content.Unload();

            base.UnloadContent();
        }
    }
}
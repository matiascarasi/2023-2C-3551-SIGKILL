using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP.Menu
{
    internal class MenuElement
    {
        private Texture2D MenuBtnTexture;
        private Rectangle MenuBtnRect;
        private string assetName;
        private MouseState PrevMouseState { get; set; }
        public string AssetName
        {
            get { return assetName; }
            set { assetName = value; }
        }
        public delegate void ElementClicked(string element);
        public event ElementClicked clickElement;
        public MenuElement(string assetName)
        {
            this.assetName = assetName;
            PrevMouseState = Mouse.GetState();
        }

        public void LoadContent(ContentManager content, GraphicsDevice GraphicsDevice)
        {
            MenuBtnTexture = content.Load<Texture2D>("Textures/Menu/" + assetName);
            var x = GraphicsDevice.Viewport.Width / 2 - MenuBtnTexture.Width / 4;
            var y = GraphicsDevice.Viewport.Height / 2 - MenuBtnTexture.Height / 4;

            MenuBtnRect = new Rectangle(x,y,MenuBtnTexture.Width / 2, MenuBtnTexture.Height / 2);
        }

        public void Update()
        {
            MouseState mouseState = Mouse.GetState();

            if (MenuBtnRect.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)) 
                && Mouse.GetState().LeftButton == ButtonState.Pressed && PrevMouseState.LeftButton == ButtonState.Released)
            {
                clickElement(assetName);
            }
            PrevMouseState = Mouse.GetState();
        }

        public void Draw(SpriteBatch spriteBatch) {         
            spriteBatch.Draw(MenuBtnTexture, MenuBtnRect, Color.White);
        }

        public void MoveElement(int x, int y)
        {
            MenuBtnRect = new Rectangle(MenuBtnRect.X + x, MenuBtnRect.Y + y, MenuBtnRect.Width, MenuBtnRect.Height);
        }
        public void SizeElement(int width, int height)
        {
            MenuBtnRect = new Rectangle(MenuBtnRect.X , MenuBtnRect.Y , MenuBtnRect.Width + width, MenuBtnRect.Height + height);
        }
    }
}

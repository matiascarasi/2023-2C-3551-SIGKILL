using System;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP.Menu
{
    internal class HUDElement
    {
        private Texture2D HudElementTexture;
        private Rectangle HudElementRect;
        private string assetName;

        public string AssetName
        {
            get { return assetName; }
            set { assetName = value; }
        }
        private float InitialWidth;
        private float Width;
        private MouseState PrevMouseState { get; set; }
        public delegate void ElementClicked(string element);
        public event ElementClicked clickElement;
        public HUDElement(string assetName)
        {
            this.assetName = assetName;
            PrevMouseState = Mouse.GetState();
        }

        public void LoadContent(ContentManager content)
        {
            HudElementTexture = content.Load<Texture2D>("Textures/HUD/" + assetName);
            InitialWidth = HudElementTexture.Width;
            HudElementRect = new Rectangle(0,0, HudElementTexture.Width , HudElementTexture.Height);
        }
        public void UpdateElementWidth(float widthDifference)
        {
            float amount = InitialWidth * (widthDifference) / Width;
            if (widthDifference <= 0) amount = HudElementTexture.Width;
            HudElementRect = new Rectangle(HudElementRect.X, HudElementRect.Y, HudElementTexture.Width - (int) amount, HudElementRect.Height);
        }
        public void Update()
        {
            MouseState mouseState = Mouse.GetState();

            if (HudElementRect.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y))
                && Mouse.GetState().LeftButton == ButtonState.Pressed && PrevMouseState.LeftButton == ButtonState.Released)
            {
                clickElement(assetName);
            }
            PrevMouseState = Mouse.GetState();
        }


        public void Draw(SpriteBatch spriteBatch) {         
            spriteBatch.Draw(HudElementTexture, HudElementRect, Color.White);
        }
        public Texture2D GetTexture()
        {
            return HudElementTexture;
        }
        public void MoveElement(int x, int y)
        {
            HudElementRect = new Rectangle(HudElementRect.X + x, HudElementRect.Y + y, HudElementRect.Width, HudElementRect.Height);
        }
        public void SizeElement(int width, int height)
        {
            HudElementRect = new Rectangle(HudElementRect.X, HudElementRect.Y, HudElementRect.Width + width, HudElementRect.Height + height);
        }
        public void SetWidth(float width)
        {
            Width = width;
        }
    }
}

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
        private float InitialHealth;
        private float Health;

        public HUDElement(string assetName)
        {
            this.assetName = assetName;
        }

        public void LoadContent(ContentManager content)
        {
            HudElementTexture = content.Load<Texture2D>("Textures/HUD/" + assetName);
            InitialWidth = HudElementTexture.Width;
            HudElementRect = new Rectangle(0,0, HudElementTexture.Width , HudElementTexture.Height);
        }

        public void Update(float newHealth)
        {
            if (newHealth < Health) {
                float healthDifference = InitialHealth - newHealth;
                float amount = InitialWidth * (healthDifference) / InitialHealth;
                if(healthDifference <= 0 ) {
                    amount = HudElementTexture.Width;
                }
                HudElementRect = new Rectangle(HudElementRect.X, HudElementRect.Y, HudElementTexture.Width - (int) amount, HudElementRect.Height);
            }
            Health = newHealth;
        }

        public void Draw(SpriteBatch spriteBatch) {         
            spriteBatch.Draw(HudElementTexture, HudElementRect, Color.White);
        }

        public void MoveElement(int x, int y)
        {
            HudElementRect = new Rectangle(HudElementRect.X + x, HudElementRect.Y + y, HudElementRect.Width, HudElementRect.Height);
        }

        public void SetHealth(float health)
        {
            InitialHealth = health;
            Health = health;
        }
    }
}

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.MonoGame.TP.Menu;

namespace TGC.MonoGame.TP.HUD
{
    public class HUDComponent
    {
        private Dictionary<string, HUDElement> Elements;
        private float initialHealth;
        private float shootingCooldown;
        private string tankName;
        public HUDComponent(string tankName, float health, float shootingCooldown)
        {
            this.shootingCooldown = shootingCooldown;
            this.initialHealth = health;
            this.tankName = tankName;

            Elements = new Dictionary<string, HUDElement>()
            {
                { "cannonBar", new HUDElement("cannonBar") },
                { "healthBar", new HUDElement("healthBar") },
                { "healthGauge", new HUDElement("healthGauge") },
                { "cannonGauge", new HUDElement("cannonGauge") },
                { tankName, new HUDElement(tankName) }
            };

        }
        public void LoadContent(ContentManager content)
        {
            foreach (var (_, element) in Elements) 
            {
                element.LoadContent(content);
            }

            Elements["healthGauge"].SetWidth(initialHealth);
            Elements["cannonGauge"].SetWidth(shootingCooldown);

            Elements[tankName].MoveElement(80, 50);
            Elements["healthBar"].MoveElement(150, 50);
            Elements["healthGauge"].MoveElement(183, 57);
            Elements["cannonBar"].MoveElement(160, 70);
            Elements["cannonGauge"].MoveElement(194, 97);

        }

        public void UpdatePlayerHealth(float newHealth)
        {
            Elements["healthGauge"].Update(newHealth);
        }

        public void UpdatePlayerCooldown(float cooldown)
        {
            if(cooldown < shootingCooldown)
            {
                float cooldownDifference = shootingCooldown - cooldown;
                Elements["cannonGauge"].Update(cooldownDifference);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            foreach (var (_, element) in Elements)
            {
                element.Draw(spriteBatch);
            }
            spriteBatch.End();
        }

    }
}

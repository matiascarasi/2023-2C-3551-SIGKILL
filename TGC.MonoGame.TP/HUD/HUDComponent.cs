using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.MonoGame.TP.Menu;

namespace TGC.MonoGame.TP.HUD
{
    internal class HUDComponent
    {
        private List<HUDElement> elementsList = new List<HUDElement>();
        private float initialHealth;
        private float shootingCooldown;
        private float health;
        private string tankName;
        public HUDComponent(string tankName, float health, float shootingCooldown)
        {
            System.Diagnostics.Debug.WriteLine(tankName);
            this.health = health;
            this.shootingCooldown = shootingCooldown;
            this.initialHealth = health;
            this.tankName = tankName;
            elementsList.Add(new HUDElement("cannonBar"));
            elementsList.Add(new HUDElement("healthBar"));
            elementsList.Add(new HUDElement("healthGauge"));
            elementsList.Add(new HUDElement("cannonGauge"));
            elementsList.Add(new HUDElement(tankName));

        }
        public void LoadContent(ContentManager content)
        {
            for (int i = 0; i < elementsList.Count; i++)
            {
                elementsList[i].LoadContent(content);
            }
            elementsList.Find(x => x.AssetName == "healthGauge").SetWidth(initialHealth);
            elementsList.Find(x => x.AssetName == "cannonGauge").SetWidth(shootingCooldown);

            elementsList.Find(x => x.AssetName == tankName).MoveElement(80, 50);
            elementsList.Find(x => x.AssetName == "healthBar").MoveElement(150, 50);
            elementsList.Find(x => x.AssetName == "healthGauge").MoveElement(183, 57);
            elementsList.Find(x => x.AssetName == "cannonBar").MoveElement(160, 70);
            elementsList.Find(x => x.AssetName == "cannonGauge").MoveElement(194, 97);

        }

        public void Update(float newhealth, float cooldown)
        {
            if (newhealth < health)
            {
                float healthDifference = initialHealth - newhealth;
                elementsList.Find(x => x.AssetName == "healthGauge").Update(healthDifference);
            }
            if(cooldown < shootingCooldown)
            {
                float cooldownDifference = shootingCooldown - cooldown;
                elementsList.Find(x => x.AssetName == "cannonGauge").Update(cooldownDifference);
            }
             health = newhealth;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            foreach (HUDElement element in elementsList)
            {
                element.Draw(spriteBatch);
            }
            spriteBatch.End();
        }

    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.MonoGame.TP.Actors;
using TGC.MonoGame.TP.Menu;

namespace TGC.MonoGame.TP.HUD
{
    public class HUDComponent
    {
        private Dictionary<string, HUDElement> Elements;
        private Dictionary<string, HUDElement> winElements;
        private Dictionary<string, HUDElement> loseElements;
        private List<GameObject> EnemyList { get; set; }
        private TGCGame game;
        private float initialHealth;
        private float shootingCooldown;
        private string tankName;
        public HUDComponent(TGCGame game , string tankName, float health, float shootingCooldown, List<GameObject> TankList)
        {
            this.shootingCooldown = shootingCooldown;
            this.initialHealth = health;
            this.tankName = tankName;
            EnemyList = TankList;
            this.game = game;

            Elements = new Dictionary<string, HUDElement>()
            {
                { "cannonBar", new HUDElement("cannonBar") },
                { "healthBar", new HUDElement("healthBar") },
                { "healthGauge", new HUDElement("healthGauge") },
                { "cannonGauge", new HUDElement("cannonGauge") },
                { tankName, new HUDElement(tankName) }
            };

            winElements = new Dictionary<string, HUDElement>()
            {
                { "retry", new HUDElement("retry") },
                { "home", new HUDElement("home") },
                { "youReWinner", new HUDElement("youReWinner") },
            };

            loseElements = new Dictionary<string, HUDElement>()
            {
                { "retry", new HUDElement("retry") },
                { "home", new HUDElement("home") },
                { "gameOver", new HUDElement("gameOver") },
            };

        }
        public void LoadContent(ContentManager content)
        {
            foreach (var (_, element) in Elements) 
            {
                element.LoadContent(content);
            }

            foreach (var (_, element) in winElements)
            {
                element.LoadContent(content);
            }

            foreach (var (_, element) in loseElements)
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

            winElements["youReWinner"].SizeElement(50, 50);
            winElements["youReWinner"].MoveElement(600, 50);
            winElements["home"].SizeElement(-100, -120);
            winElements["home"].MoveElement(900, 600); 
            winElements["retry"].SizeElement(-100, -115);
            winElements["retry"].MoveElement(760, 600);

            winElements["home"].clickElement += OnClick;
            winElements["retry"].clickElement += OnClick;

            loseElements["gameOver"].MoveElement(370, 170);
            loseElements["home"].SizeElement(-100, -120);
            loseElements["home"].MoveElement(900, 600);
            loseElements["retry"].SizeElement(-100, -115);
            loseElements["retry"].MoveElement(760, 600);

            loseElements["home"].clickElement += OnClick;
            loseElements["retry"].clickElement += OnClick;

        }

        public void UpdatePlayerHealth(float newHealth)
        {
            Elements["healthGauge"].UpdateElementWidth(newHealth);

            System.Diagnostics.Debug.WriteLine(newHealth);
            

        }

        public void Update()
        {
            loseElements["home"].Update();
            loseElements["retry"].Update();
            winElements["home"].Update();
            winElements["retry"].Update();
        }


        public void UpdatePlayerCooldown(float cooldown)
        {
            if(cooldown < shootingCooldown)
            {
                float cooldownDifference = shootingCooldown - cooldown;
                Elements["cannonGauge"].UpdateElementWidth(cooldownDifference);
            }
        }

        public void Draw(SpriteBatch spriteBatch, float health , List<GameObject> tankList)
        {
            var state = "";

            spriteBatch.Begin();

            if (health <= 0) state = "lose";
            if (tankList.Count == 0) state = "win";

            if (state == "win")
            {
                foreach (var (_, element) in winElements)
                {
                    element.Draw(spriteBatch);
                }
            }
            else if (state == "lose")
            {
                foreach (var (_, element) in loseElements)
                {
                    element.Draw(spriteBatch);
                }
            }
            else
            {
                foreach (var (_, element) in Elements)
                {
                    element.Draw(spriteBatch);
                }
            }
            
            spriteBatch.End();
        }
        public void OnClick(string element)
        {
            if (element == "home")
            {
                game.IsMenuActive = true;
            }
            else if (element == "retry")
            {

            }
        }
    }
}

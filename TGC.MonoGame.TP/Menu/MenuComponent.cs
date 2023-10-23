using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace TGC.MonoGame.TP.Menu
{
    internal class MenuComponent
    {
        private TGCGame game;
        private List<MenuElement> elementsList = new List<MenuElement>();
        public MenuComponent(TGCGame game) {
            this.game = game;
            elementsList.Add(new MenuElement("tankWarsLogo"));
            elementsList.Add(new MenuElement("play2"));
            elementsList.Add(new MenuElement("options2"));
            elementsList.Add(new MenuElement("exit2"));
        }

        public void LoadContent(ContentManager content, GraphicsDevice device)
        {   
            for (int i = 0; i < elementsList.Count; i++) {
                elementsList[i].LoadContent(content, device);
                elementsList[i].MoveElement(0, 150 * i);
                elementsList[i].clickElement += OnClick;
                if(elementsList[i].AssetName != "tankWarsLogo")
                {
                    elementsList[i].SizeElement(-150, -100);
                    elementsList[i].MoveElement(75, -100);

                }
            }
            elementsList.Find(x => x.AssetName == "tankWarsLogo").MoveElement(-150, -500);
            elementsList.Find(x => x.AssetName == "tankWarsLogo").SizeElement(300, 400);

        }

        public void Update()
        {
            foreach (MenuElement element in elementsList)
            {
                element.Update();
            }
        }

        public void Draw (SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            foreach (MenuElement element in elementsList)
            {
                element.Draw(spriteBatch);
            }
            spriteBatch.End();
        }

        public void OnClick(string element)
        {
            if (element == "play2") 
            {
                game.IsMouseVisible = false;
                game.IsMenuActive = false;
            }
            else if (element == "exit2")
            {
                game.Exit();
            }
        }
    }
}

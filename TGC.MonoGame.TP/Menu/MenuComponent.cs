using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using TGC.MonoGame.TP.Actors;

namespace TGC.MonoGame.TP.Menu
{
    internal class MenuComponent
    {
        private TGCGame game;
        private List<MenuElement> elementsList = new List<MenuElement>();
        private List<MenuElement> optionsList = new List<MenuElement>();

        private bool optionsVisible;
        private bool god_mode;
        private bool mute_effect;
        private bool mute_music;
        private float player_health;
        
        public MenuComponent(TGCGame game, float health) {
            this.game = game;
            player_health = health;
            elementsList.Add(new MenuElement("tankWarsLogo"));
            elementsList.Add(new MenuElement("play2"));
            elementsList.Add(new MenuElement("options2"));
            elementsList.Add(new MenuElement("exit2"));
            optionsList.Add(new MenuElement("exitcross"));
            optionsList.Add(new MenuElement("options_container"));
            optionsList.Add(new MenuElement("modo_god"));
            optionsList.Add(new MenuElement("mute_music"));
            optionsList.Add(new MenuElement("mute_effects"));

            optionsList.Add(new MenuElement("empty_chekbox_modogod"));
            optionsList.Add(new MenuElement("empty_chekbox_muteeffects"));
            optionsList.Add(new MenuElement("empty_chekbox_mutemusic"));
            optionsList.Add(new MenuElement("tick_modogod"));
            optionsList.Add(new MenuElement("tick_mutemusic"));
            optionsList.Add(new MenuElement("tick_muteeffects"));

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
            for (int i = 0; i < optionsList.Count; i++)
            {
                optionsList[i].LoadContent(content, device);
                optionsList[i].clickElement += OnClick;
                if (optionsList[i].AssetName == "exitcross")
                {
                    optionsList[i].MoveElement(-700, -350);
                }
                else if (optionsList[i].AssetName.Contains("tick"))
                {
                    optionsList[i].SizeElement(-200, -180);
                    optionsList[i].MoveElement(380, -2230);
                    optionsList[i].MoveElement(-100, 150 * i);

                }
                else if (optionsList[i].AssetName.Contains("empty_chekbox"))
                {
                    optionsList[i].SizeElement(-150, -150);
                    optionsList[i].MoveElement(350, -800);
                    optionsList[i].MoveElement(-100, 150 * i);
                }

                else if (optionsList[i].AssetName == "options_container")
                {
                    optionsList[i].MoveElement(0, 0);

                }
                else
                {
                    optionsList[i].MoveElement(0, -400);
                    optionsList[i].SizeElement(0, -50);
                    optionsList[i].MoveElement(-100, 150 * i);
                }
            }

            elementsList.Find(x => x.AssetName == "tankWarsLogo").MoveElement(-150, -500);
            elementsList.Find(x => x.AssetName == "tankWarsLogo").SizeElement(300, 400);

        }

        public void Update()
        {
            if(!optionsVisible)
            {
                foreach (MenuElement element in elementsList)
                {
                    element.Update();
                }
            } else {
                foreach (MenuElement element in optionsList)
                {
                    element.Update();
                }
            }
        }

        public void Draw (SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            if (!optionsVisible)
            {
                foreach (MenuElement element in elementsList)
                {
                    element.Draw(spriteBatch);
                }
            }
            else
            {
                foreach (MenuElement element in optionsList)
                {
                    element.Draw(spriteBatch);
                }
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
            else if (element == "options2")
            {
                this.optionsVisible = true;
            }
            else if (element == "exitcross")
            {
                this.optionsVisible = false;
            }
            else if (element == "modo_god")
            {
                this.god_mode = !god_mode;
                if (god_mode)
                {
                    optionsList.Find(x => x.AssetName == "tick_modogod").MoveElement(0, 1000);
                    game.Player.Health = 99999999999999f;
                }
                else { 
                    optionsList.Find(x => x.AssetName == "tick_modogod").MoveElement(0, -1000);
                    game.Player.Health = player_health;
                }

            }
            else if (element == "mute_effects")
            {
                this.mute_effect = !mute_effect;
                if (mute_effect)
                {
                    optionsList.Find(x => x.AssetName == "tick_muteeffects").MoveElement(0, 1000);
                    SoundEffect.MasterVolume -= .1f;
                }
                else
                {
                    optionsList.Find(x => x.AssetName == "tick_muteeffects").MoveElement(0, -1000);
                    SoundEffect.MasterVolume += .1f;
                }
            }
            else if (element == "mute_music")
            {
                this.mute_music = !mute_music;
                if (mute_music)
                {
                    optionsList.Find(x => x.AssetName == "tick_mutemusic").MoveElement(0, 1000);
                    MediaPlayer.Volume -= .7f;
                }
                else
                {
                    optionsList.Find(x => x.AssetName == "tick_mutemusic").MoveElement(0, -1000);
                    MediaPlayer.Volume += .5f;
                }
            }
            else if (element == "exit2")
            {
                game.Exit();
            }
        }
    }
}

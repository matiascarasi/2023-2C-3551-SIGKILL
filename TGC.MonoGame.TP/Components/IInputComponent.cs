using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.MonoGame.TP.Content.Actors;

namespace TGC.MonoGame.TP.Components
{
    public interface IInputComponent
    {
        public void LoadContent(ContentManager content);
        public void Update(GameObject gameObject, GameTime gameTime, MouseCamera mouseCamera, bool IsMenuActive);
    }
}

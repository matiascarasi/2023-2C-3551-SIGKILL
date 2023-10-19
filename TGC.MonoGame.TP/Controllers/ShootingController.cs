using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TGC.MonoGame.TP.Content.Actors;

namespace TGC.MonoGame.TP.Controllers
{
    class ShootingController
    {
        MouseCamera MouseCamera { get; set; }

        public ShootingController(MouseCamera camera)
        {
           MouseCamera = camera;
        }

        public void Shoot(List<GameObject> objects) {

            var ray = MouseCamera.OffsetPosition - MouseCamera.FollowedPosition;
            System.Diagnostics.Debug.WriteLine(ray);

        }
    }
}

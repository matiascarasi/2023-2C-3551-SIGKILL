﻿using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Collisions;

namespace TGC.MonoGame.TP.Components.Collisions
{
    class T90CollisionComponent : CollisionComponent
    {
        public override void LoadContent(GameObject gameObject)
        {
            var boundingBox = BoundingVolumesExtensions.FromMatrix(Matrix.CreateScale(390f, 250f, 660f));
            SetBoundingBox(gameObject, boundingBox);
        }
    }
}
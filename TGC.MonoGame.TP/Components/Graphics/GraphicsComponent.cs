using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Actors;

namespace TGC.MonoGame.TP.Components.Graphics
{
    class GraphicsComponent : IComponent
    {
        private readonly string ModelPath;
        private readonly Dictionary<string, string> EffectPaths;
        private readonly Dictionary<string, string> TexturePaths;
        private readonly string DefaultEffectPath;
        private readonly string DefaultTexturePath;
        private Dictionary<string, Effect> Effects;
        private Dictionary<string, Texture> Textures;

        public GraphicsComponent(string model, string defaultEffect, string defaultTexture)
        {
            ModelPath = model;
            DefaultEffectPath = defaultEffect;
            DefaultTexturePath = defaultTexture;
            EffectPaths = new Dictionary<string, string>();
            TexturePaths = new Dictionary<string, string>();
        }
        public GraphicsComponent(string model, string defaultEffect, string defaultTexture, Dictionary<string, string> effects, Dictionary<string, string> textures) : this(model, defaultEffect, defaultTexture)
        {
            EffectPaths = effects;
            TexturePaths = textures;
        }

        public virtual void LoadContent(GameObject Object, ContentManager Content)
        {
            Object.Model = Content.Load<Model>(ModelPath);
            Effects = EffectPaths.ToDictionary(kv => kv.Key, kv => Content.Load<Effect>(kv.Value));
            Textures = TexturePaths.ToDictionary(kv => kv.Key, kv => Content.Load<Texture>(kv.Value));

            var meshNames = Object.Model.Meshes.Select(mesh => mesh.Name);
            foreach (var meshName in Effects.Keys)
            {
                if (!meshNames.Contains(meshName))
                {
                    throw new Exception($"Invalid mesh \"{meshName}\" in effects dictionary");
                }
            }
            foreach (var meshName in Textures.Keys)
            {
                if (!meshNames.Contains(meshName))
                {
                    throw new Exception($"Invalid mesh \"{meshName}\" in textures dictionary");
                }
            }
            foreach (var meshName in meshNames)
            {
                if (!Effects.ContainsKey(meshName))
                {
                    Effects[meshName] = Content.Load<Effect>(DefaultEffectPath);
                }
                if (!Textures.ContainsKey(meshName))
                {
                    Textures[meshName] = Content.Load<Texture>(DefaultTexturePath);
                }
            }

            foreach (var mesh in Object.Model.Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = Effects[mesh.Name];
                }
            }
        }
        public virtual void Draw(GameObject Object, GameTime gameTime, Matrix view, Matrix projection)
        {

            var scaleMatrix = Matrix.CreateScale(Object.Scale);
            var rotationMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(Object.RotationAngle));
            var translationMatrix = Matrix.CreateTranslation(Object.Position);

            var world = scaleMatrix * rotationMatrix * translationMatrix;

            Matrix[] matrices = new Matrix[Object.Model.Bones.Count];
            Object.Model.CopyAbsoluteBoneTransformsTo(matrices);
            Object.World = world;

            Effect effect;
            foreach (var mesh in Object.Model.Meshes)
            {
                effect = Effects[mesh.Name];
                effect.Parameters["Texture"].SetValue(Textures[mesh.Name]);
                effect.Parameters["View"].SetValue(view);
                effect.Parameters["Projection"].SetValue(projection);
                world = matrices[mesh.ParentBone.Index] * Object.World;
                effect.Parameters["World"].SetValue(world);
                mesh.Draw();
            }
        }

        public virtual void Update(GameObject gameObject, GameTime gameTime)
        {
        }
    }
}
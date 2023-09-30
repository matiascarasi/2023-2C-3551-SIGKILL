using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Collisions;
using TGC.MonoGame.TP.Content.Actors;

namespace TGC.MonoGame.TP.Graphics
{
    class GraphicsComponent : IGraphicsComponent
    {
        private readonly string ModelPath;
        private readonly Dictionary<string, string> EffectPaths;
        private readonly Dictionary<string, string> TexturePaths;
        private readonly string DefaultEffectPath;
        private readonly string DefaultTexturePath;
        private readonly ContentManager Content;
        public Model Model { get; set; }
        private Dictionary<string, Effect> Effects;
        private Dictionary<string, Texture> Textures;
        private BoundingBox ObjectBox { get; set; }

        public GraphicsComponent(ContentManager content, string model, string defaultEffect, string defaultTexture)
        {
            Content = content;
            ModelPath = model;
            DefaultEffectPath = defaultEffect;
            DefaultTexturePath = defaultTexture;
            EffectPaths = new Dictionary<string, string>();
            TexturePaths = new Dictionary<string, string>();
        }
        public GraphicsComponent(ContentManager content, string model, string defaultEffect, string defaultTexture, Dictionary<string, string> effects, Dictionary<string, string> textures) : this(content, model, defaultEffect, defaultTexture)
        {
            EffectPaths = effects;
            TexturePaths = textures;
        }

        public void LoadContent(GameObject gameObject)
        {
            Model = Content.Load<Model>(ModelPath);
            Effects = EffectPaths.ToDictionary(kv => kv.Key, kv => Content.Load<Effect>(kv.Value));
            Textures = TexturePaths.ToDictionary(kv => kv.Key, kv => Content.Load<Texture>(kv.Value));
            ObjectBox = BoundingVolumesExtensions.CreateAABBFrom(Model);

            gameObject.BoundingBox = new BoundingBox(ObjectBox.Min + gameObject.Position, ObjectBox.Max + gameObject.Position);

            var meshNames = Model.Meshes.Select(mesh => mesh.Name);
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

            foreach (var mesh in Model.Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = Effects[mesh.Name];
                }
            }
        }

        public void Draw(GameObject Object, GameTime gameTime, Matrix view, Matrix projection)
        {

            var scaleMatrix = Matrix.CreateScale(Object.Scale);
            var rotationMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(Object.YAxisRotation));
            var translationMatrix = Matrix.CreateTranslation(Object.Position);
            var world = scaleMatrix * rotationMatrix * translationMatrix;
            Matrix[] matrices = new Matrix[Model.Bones.Count];
            Model.CopyAbsoluteBoneTransformsTo(matrices);
            Object.World = world;

            Effect effect;
            foreach (var mesh in Model.Meshes)
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

    }
}
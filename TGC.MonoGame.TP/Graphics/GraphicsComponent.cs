using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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


           
            var temporaryCubeAABB = BoundingVolumesExtensions.CreateAABBFrom(Model);

            if (ModelPath.Contains("Panzer"))
            {
                foreach (var mesh in Model.Meshes)
                {
                    if (mesh.Name == "Hull")
                    {
                        temporaryCubeAABB =
                            BoundingVolumesExtensions.FromMatrix(
                                Matrix.CreateScale(390f, 250f, 660f));
                    }
                }
            }

            temporaryCubeAABB = BoundingVolumesExtensions.Scale(temporaryCubeAABB, 1f);
            // Create an Oriented Bounding Box from the AABB
            gameObject.OBB = OrientedBoundingBox.FromAABB(temporaryCubeAABB);
            // Move the center
        

            gameObject.OBB.Center = BoundingVolumesExtensions.GetCenter(temporaryCubeAABB) + gameObject.Position;


            // Then set its orientation!
            gameObject.OBB.Orientation = Matrix.CreateRotationY(gameObject.YAxisRotation);

            // Create an OBB World-matrix so we can draw a cube representing it
            gameObject.OBBWorld = Matrix.CreateScale(gameObject.OBB.Extents * 2f) *
                 gameObject.OBB.Orientation *
                 Matrix.CreateTranslation(gameObject.Position + new Vector3(0f,150f,0f));

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
                //System.Diagnostics.Debug.WriteLine(mesh.Name);
                effect = Effects[mesh.Name];
                effect.Parameters["Texture"].SetValue(Textures[mesh.Name]);
                effect.Parameters["View"].SetValue(view);
                effect.Parameters["Projection"].SetValue(projection);
                world = matrices[mesh.ParentBone.Index] * Object.World;
                effect.Parameters["World"].SetValue(world);
                if (mesh.Name.Contains("Treadmill"))
                {
                    effect.Parameters["Time"]?.SetValue(Convert.ToSingle(gameTime.TotalGameTime.TotalSeconds));
                    effect.Parameters["Speed"].SetValue(Object.Speed);

                }
                mesh.Draw();
            }
        }

    }
}
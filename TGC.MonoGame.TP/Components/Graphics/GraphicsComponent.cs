using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Actors;
using TGC.MonoGame.TP.Cameras;
using TGC.MonoGame.TP.Defaults;

namespace TGC.MonoGame.TP.Components.Graphics
{
    public class GraphicsComponent
    {
        private readonly string ModelPath;
        private readonly Dictionary<string, string> EffectPaths;
        private readonly Dictionary<string, string> TexturePaths;
        private readonly string DefaultEffectPath;
        private readonly string DefaultTexturePath;
        protected Dictionary<string, Effect> Effects;
        private Dictionary<string, Texture> Textures;

        private readonly string DefaultNormalPath;
        private Dictionary<string, string> NormalPaths;
        private Dictionary<string, Texture> Normals;
        private string BlinnPhongType;


        public GraphicsComponent(string model, string defaultEffect, string defaultTexture)
        {
            ModelPath = model;
            DefaultEffectPath = defaultEffect;
            DefaultTexturePath = defaultTexture;
            EffectPaths = new Dictionary<string, string>();
            TexturePaths = new Dictionary<string, string>();
        }
        public GraphicsComponent(
            string model, 
            string defaultEffect,
            string defaultNormal,
            string defaultTexture,
            Dictionary<string, string> effects,
            Dictionary<string, string> normals,
            Dictionary<string, string> textures,
            string blinnPhongType)
            
            : this(model, defaultEffect, defaultTexture)
        {
            EffectPaths = effects;
            TexturePaths = textures;
            NormalPaths = normals;
            DefaultNormalPath = defaultNormal;
            BlinnPhongType = blinnPhongType;
        }
        protected virtual void SetCustomEffectParameters(Effect effect, GameObject gameObject, string meshName) { }
        public virtual void LoadContent(GameObject Object, ContentManager Content)
        {
            Object.Model = Content.Load<Model>(ModelPath);
            Object.Bones = new Matrix[Object.Model.Bones.Count];
            Object.Model.CopyAbsoluteBoneTransformsTo(Object.Bones);

            Effects = EffectPaths.ToDictionary(kv => kv.Key, kv => Content.Load<Effect>(kv.Value));
            Textures = TexturePaths.ToDictionary(kv => kv.Key, kv => Content.Load<Texture>(kv.Value));

            if(BlinnPhongType == "NormalMapping")
            {
                Normals = NormalPaths.ToDictionary(kv => kv.Key, kv => Content.Load<Texture>(kv.Value));
            }

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
                if (BlinnPhongType == "NormalMapping")
                {

                    if (!Normals.ContainsKey(meshName))
                    {
                        Normals[meshName] = Content.Load<Texture>(DefaultNormalPath);
                    }
                }

            }

            foreach (var mesh in Object.Model.Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = Effects[mesh.Name];


                    if (BlinnPhongType == null) BlinnPhongType = "Default";
                        meshPart.Effect.CurrentTechnique = Effects[mesh.Name].Techniques[BlinnPhongType];
                        meshPart.Effect.Parameters["lightPosition"]?.SetValue(new Vector3(0f,2000f,500f));

                        meshPart.Effect.Parameters["ambientColor"]?.SetValue(PhongShadingDefaults.ambientColor);
                        meshPart.Effect.Parameters["diffuseColor"]?.SetValue(PhongShadingDefaults.diffuseColor);
                        meshPart.Effect.Parameters["specularColor"]?.SetValue(PhongShadingDefaults.specularColor);

                        meshPart.Effect.Parameters["KAmbient"]?.SetValue(PhongShadingDefaults.KA);
                        meshPart.Effect.Parameters["KDiffuse"]?.SetValue(PhongShadingDefaults.KD);
                        meshPart.Effect.Parameters["KSpecular"]?.SetValue(PhongShadingDefaults.KS);
                        meshPart.Effect.Parameters["shininess"]?.SetValue(PhongShadingDefaults.Shininess);

                }
            }
        }
        public virtual void Draw(GameObject Object, GameTime gameTime, Matrix view, Matrix projection, Vector3 cameraPosition)
        {


            var scaleMatrix = Matrix.CreateScale(Object.Scale);
            var rotationMatrix = Matrix.CreateFromQuaternion(Object.Rotation);
            var translationMatrix = Matrix.CreateTranslation(Object.Position);

            var world = scaleMatrix * rotationMatrix * translationMatrix;
            Object.World = world;

            Effect effect;
            var viewProjection = view * projection;
            
                foreach (var mesh in Object.Model.Meshes)
                {
                    effect = Effects[mesh.Name];

                    if(Effects.ContainsKey(mesh.Name))
                    {
                        SetCustomEffectParameters(effect, Object, mesh.Name);
                    }
                    if (BlinnPhongType == "NormalMapping")
                    {
                        world = Object.Bones[mesh.ParentBone.Index] * Object.World;

                        effect.Parameters["Velocity"]?.SetValue(Object.Velocity.Length());
                        effect.Parameters["Time"]?.SetValue(Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds));
                        effect.Parameters["eyePosition"].SetValue(cameraPosition);
                        effect.Parameters["ModelTexture"].SetValue(Textures[mesh.Name]);
                        effect.Parameters["NormalTexture"].SetValue(Normals[mesh.Name]);
                        effect.Parameters["World"].SetValue(world);
                        effect.Parameters["InverseTransposeWorld"].SetValue(Matrix.Invert(Matrix.Transpose(world)));
                        effect.Parameters["WorldViewProjection"].SetValue(world * viewProjection);
                        effect.Parameters["Tiling"].SetValue(Vector2.One);
                    } 
                    else
                    {
                        world = Object.Bones[mesh.ParentBone.Index] * Object.World;

                        effect.Parameters["eyePosition"].SetValue(cameraPosition);
                        effect.Parameters["ModelTexture"].SetValue(Textures[mesh.Name]);
                        effect.Parameters["World"].SetValue(world);
                        effect.Parameters["InverseTransposeWorld"].SetValue(Matrix.Invert(Matrix.Transpose(world)));
                        effect.Parameters["WorldViewProjection"].SetValue(world * viewProjection);
                        effect.Parameters["Tiling"].SetValue(Vector2.One);
                }
                    mesh.Draw();
                }
        }

        public void setParams()
        {

        }

        public virtual void Update(GameObject gameObject, GameTime gameTime)
        {
        }
    }
}
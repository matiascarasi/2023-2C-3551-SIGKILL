using System;
using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP
{
    public class SkyBox
    {

        private readonly string ModelPath;
        private readonly string TexturePath;
        private Model Model;
        private TextureCube Texture;
        private Effect Effect;
        private float _size;
        public const string ContentFolderEffects = "Effects/";
        public SkyBox(string modelPath, string texturePath, float size)
        {
            ModelPath = modelPath;
            TexturePath = texturePath;
            _size = size;
        }
        public void LoadContent(ContentManager contentManager)
        {
            Model = contentManager.Load<Model>(ModelPath);
            Texture = contentManager.Load<TextureCube>(TexturePath);
            Effect = contentManager.Load<Effect>(ContentFolderEffects + "SkyBox");
        }
        public void Draw(Matrix view, Matrix projection, Vector3 cameraPosition)
        {
            // Go through each pass in the effect, but we know there is only one...
            foreach (var pass in Effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                // Draw all of the components of the mesh, but we know the cube really
                // only has one mesh
                foreach (var mesh in Model.Meshes)
                {
                    // Assign the appropriate values to each of the parameters
                    foreach (var part in mesh.MeshParts)
                    {
                        part.Effect = Effect;
                        part.Effect.Parameters["World"].SetValue(
                            Matrix.CreateScale(_size) * Matrix.CreateTranslation(Vector3.Zero));
                        part.Effect.Parameters["View"].SetValue(view);
                        part.Effect.Parameters["Projection"].SetValue(projection);
                        part.Effect.Parameters["SkyBoxTexture"].SetValue(Texture);
                        part.Effect.Parameters["CameraPosition"].SetValue(cameraPosition);
                    }

                    // Draw the mesh with the SkyBox effect
                    mesh.Draw();
                }
            }
        }
    }
}
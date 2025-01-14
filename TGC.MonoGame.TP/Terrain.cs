﻿using System;
using BepuPhysics.Collidables;
using BepuPhysics.Constraints;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Defaults;

namespace TGC.MonoGame.TP
{
    public class Terrain
    {

        private Effect Effect;
        private VertexBuffer VertexBuffer;
        private IndexBuffer IndexBuffer;
        private int PrimitiveCount;
        private readonly string HeightmapPath;
        private readonly string TexturePath;
        private Texture2D Texture;
        public Texture2D currentHeightmap;
        public const string ContentFolderEffects = "Effects/";
        public float[,] heightmap;
        private Vector2 ViewPortSize;
        private readonly float scaleXZ;
        private readonly float scaleY;

        public Terrain(ContentManager contentManager, GraphicsDevice graphicsDevice, string heightmapPath, string texturePath, float _scaleXZ, float _scaleY)
        {
            HeightmapPath = heightmapPath;
            TexturePath = texturePath;
            scaleXZ = _scaleXZ;
            scaleY = _scaleY;
            currentHeightmap = contentManager.Load<Texture2D>(HeightmapPath);
            ViewPortSize = new Vector2(graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);

            CreateHeightmapMesh(graphicsDevice);
        }

        public void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            Texture = contentManager.Load<Texture2D>(TexturePath);
            Effect = contentManager.Load<Effect>(ContentFolderEffects + "Terrain");

            Effect.CurrentTechnique = Effect.Techniques["Default"];
            Effect.Parameters["lightPosition"]?.SetValue(new Vector3(0f, 1000f, 500f));

            Effect.Parameters["ambientColor"]?.SetValue(new Vector3(.897f, .897f, .897f));
            Effect.Parameters["diffuseColor"]?.SetValue(new Vector3(.517f, .534f, .338f));
            Effect.Parameters["specularColor"]?.SetValue(new Vector3(1f, 1f, 1f));

            Effect.Parameters["KAmbient"]?.SetValue(.8f);
            Effect.Parameters["KDiffuse"]?.SetValue(.7f);
            Effect.Parameters["KSpecular"]?.SetValue(.5f);
            Effect.Parameters["shininess"]?.SetValue(2f);
        }

        public void Draw(GraphicsDevice graphicsDevice, Matrix view, Matrix projection, Vector3 cameraPosition, string effectTechnique)
        {
            graphicsDevice.DepthStencilState = DepthStencilState.Default;
            graphicsDevice.SetVertexBuffer(VertexBuffer);
            graphicsDevice.Indices = IndexBuffer;

            var viewProjection = view * projection;


            Effect.CurrentTechnique = Effect.Techniques[effectTechnique];

            Effect.Parameters["eyePosition"].SetValue(cameraPosition);
            Effect.Parameters["ModelTexture"].SetValue(Texture);
            Effect.Parameters["World"].SetValue(Matrix.Identity);
            Effect.Parameters["InverseTransposeWorld"].SetValue(Matrix.Invert(Matrix.Transpose(Matrix.Identity)));
            Effect.Parameters["WorldViewProjection"].SetValue(Matrix.Identity * viewProjection);
            Effect.Parameters["Tiling"].SetValue(Vector2.One);

            foreach (var pass in Effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, PrimitiveCount);
            }
        }

        private void CreateHeightmapMesh(GraphicsDevice graphicsDevice)
        {
            LoadHeightmap(currentHeightmap);

            CreateVertexBuffer(graphicsDevice);

            var heightMapWidthMinusOne = heightmap.GetLength(0) - 1;
            var heightMapLengthMinusOne = heightmap.GetLength(1) - 1;

            PrimitiveCount = 2 * heightMapWidthMinusOne * heightMapLengthMinusOne;

            CreateIndexBuffer(graphicsDevice, (uint)heightMapWidthMinusOne, (uint)heightMapLengthMinusOne);
        }

        private void LoadHeightmap(Texture2D texture)
        {
            var texels = new Color[texture.Width * texture.Height];

            texture.GetData(texels);

            heightmap = new float[texture.Width, texture.Height];

            for (var x = 0; x < texture.Width; x++)
                for (var y = 0; y < texture.Height; y++)
                {
                    var texel = texels[y * texture.Width + x];
                    heightmap[x, y] = (texel.R + texel.G + texel.B) / 3;
                }
        }

        private void CreateVertexBuffer(GraphicsDevice graphicsDevice)
        {
            var heightMapWidth = heightmap.GetLength(0);
            var heightMapLength = heightmap.GetLength(1);

            var offsetX = heightMapWidth * scaleXZ * 0.5f;
            var offsetZ = heightMapLength * scaleXZ * 0.5f;

            var vertexCount = heightMapWidth * heightMapLength;

            var vertices = new VertexPositionTexture[vertexCount];

            var index = 0;
            Vector3 position;
            Vector2 textureCoordinate;
            for (var x = 0; x < heightMapWidth; x++)
            {
                var xCoordinate = x * scaleXZ - offsetX;
                for (var z = 0; z < heightMapLength; z++)
                {
                    position = new Vector3(xCoordinate, heightmap[x, z] * scaleY, z * scaleXZ - offsetZ);
                    textureCoordinate = new Vector2((float)x / heightMapWidth, (float)z / heightMapLength);
                    vertices[index] = new VertexPositionTexture(position, textureCoordinate);
                    index++;
                }
            }

            VertexBuffer = new VertexBuffer(graphicsDevice, VertexPositionTexture.VertexDeclaration, vertexCount,
                BufferUsage.None);
            VertexBuffer.SetData(vertices);
        }


        private void CreateIndexBuffer(GraphicsDevice graphicsDevice, uint quadsInX, uint quadsInZ)
        {
            var indexCount = (int)(3 * 2 * quadsInX * quadsInZ);

            var indices = new uint[indexCount];
            var index = 0;

            uint right;
            uint top;
            uint bottom;

            var vertexCountX = quadsInX + 1;
            for (uint x = 0; x < quadsInX; x++)
                for (uint z = 0; z < quadsInZ; z++)
                {
                    right = x + 1;
                    bottom = z * vertexCountX;
                    top = (z + 1) * vertexCountX;

                    //  d __ c  
                    //   | /|
                    //   |/_|
                    //  a    b

                    var a = x + bottom;
                    var b = right + bottom;
                    var c = right + top;
                    var d = x + top;

                    // ACB
                    indices[index] = a;
                    index++;
                    indices[index] = c;
                    index++;
                    indices[index] = b;
                    index++;

                    // ADC
                    indices[index] = a;
                    index++;
                    indices[index] = d;
                    index++;
                    indices[index] = c;
                    index++;
                }
            IndexBuffer =
                new IndexBuffer(graphicsDevice, IndexElementSize.ThirtyTwoBits, indexCount, BufferUsage.None);
            IndexBuffer.SetData(indices);
        }

        public float Height(float x, float z)
        {
            var width = heightmap.GetLength(0);
            var length = heightmap.GetLength(1);

            var pos_i = x / scaleXZ + width / 2.0f;
            var pos_j = z / scaleXZ + length / 2.0f;
            var pi = (int)pos_i;
            var fracc_i = pos_i - pi;
            var pj = (int)pos_j;
            var fracc_j = pos_j - pj;

            if (pi < 0)
                pi = 0;
            else if (pi >= width)
                pi = width - 1;

            if (pj < 0)
                pj = 0;
            else if (pj >= length)
                pj = length - 1;

            var pi1 = pi + 1;
            var pj1 = pj + 1;
            if (pi1 >= width)
                pi1 = width - 1;
            if (pj1 >= length)
                pj1 = length - 1;

            var H0 = heightmap[pi, pj];
            var H1 = heightmap[pi1, pj];
            var H2 = heightmap[pi, pj1];
            var H3 = heightmap[pi1, pj1];
            var H = (H0 * (1 - fracc_i) + H1 * fracc_i) * (1 - fracc_j) + (H2 * (1 - fracc_i) + H3 * fracc_i) * fracc_j;

            return H * scaleY;
        }

        public void UnloadContent()
        {
            VertexBuffer.Dispose();
        }

        public float GetScaleXZ()
        {
            return scaleXZ;
        }

        public float GetScaleY()
        {
            return scaleY;
        }

    }
}
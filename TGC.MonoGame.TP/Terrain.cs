using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP
{
    public class Terrain
    {

        private Effect TerrainEffect;
        private VertexBuffer VertexBuffer;
        private IndexBuffer IndexBuffer;
        private int PrimitiveCount;
        private readonly string HeightmapPath;
        private readonly string TexturePath;
        public const string ContentFolderEffects = "Effects/";

        public Terrain(string heightmapPath, string texturePath)
        {
            HeightmapPath = heightmapPath;
            TexturePath = texturePath;
        }

        public void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            var currentHeightmap = contentManager.Load<Texture2D>(HeightmapPath);

            var scaleXZ = 50.0f;
            var scaleY = -1.0f;
            CreateHeightmapMesh(graphicsDevice, currentHeightmap, scaleXZ, scaleY);

            var terrainTexture = contentManager.Load<Texture2D>(TexturePath);

     
            TerrainEffect = contentManager.Load<Effect>(ContentFolderEffects + "BasicShader"); 
            TerrainEffect.Parameters["World"].SetValue(Matrix.Identity);
           // Effect.Parameters["Texture"].SetValue(terrainTexture);
        }

        public void Draw(GraphicsDevice graphicsDevice, Matrix view, Matrix projection)
        {
            graphicsDevice.DepthStencilState = DepthStencilState.Default;
            graphicsDevice.SetVertexBuffer(VertexBuffer);
            graphicsDevice.Indices = IndexBuffer;

            TerrainEffect.Parameters["View"].SetValue(view);
            TerrainEffect.Parameters["Projection"].SetValue(projection);
        //    TerrainEffect.Parameters["DiffuseColor"].SetValue(Color.DarkGreen.ToVector3());

            foreach (var pass in TerrainEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, PrimitiveCount);
            }
        }

        private void CreateHeightmapMesh(GraphicsDevice graphicsDevice, Texture2D texture, float scaleXZ, float scaleY)
        {
            var heightMap = LoadHeightmap(texture);

            CreateVertexBuffer(graphicsDevice, heightMap, scaleXZ, scaleY);

            var heightMapWidthMinusOne = heightMap.GetLength(0) - 1;
            var heightMapLengthMinusOne = heightMap.GetLength(1) - 1;

            PrimitiveCount = 2 * heightMapWidthMinusOne * heightMapLengthMinusOne;
            
            CreateIndexBuffer(graphicsDevice, (uint)heightMapWidthMinusOne, (uint)heightMapLengthMinusOne);
        }

        private float[,] LoadHeightmap(Texture2D texture)
        {
            var texels = new Color[texture.Width * texture.Height];

            texture.GetData(texels);

            var heightmap = new float[texture.Width, texture.Height];

            for (var x = 0; x < texture.Width; x++)
                for (var y = 0; y < texture.Height; y++)
                {
                    var texel = texels[y * texture.Width + x];
                    heightmap[x, y] = (texel.R+texel.G+texel.B)/3;
                }

            return heightmap;
        }

        private void CreateVertexBuffer(GraphicsDevice graphicsDevice, float[,] heightMap, float scaleXZ, float scaleY)
        {
            var heightMapWidth = heightMap.GetLength(0);
            var heightMapLength = heightMap.GetLength(1);

            var offsetX = heightMapWidth * scaleXZ * 0.5f;
            var offsetZ = heightMapLength * scaleXZ * 0.5f;

            var vertexCount = heightMapWidth * heightMapLength;

            var vertices = new VertexPositionTexture[vertexCount];
            
            var index = 0;
            Vector3 position;
            Vector2 textureCoordinates;
            for (var x = 0; x < heightMapWidth; x++)
            {
                var xCoordinate = x * scaleXZ - offsetX;
                for (var z = 0; z < heightMapLength; z++)
                {
                    position = new Vector3(xCoordinate, heightMap[x, z] * scaleY, z * scaleXZ - offsetZ);
                    textureCoordinates = new Vector2((float)x / heightMapWidth, (float)z / heightMapLength);
                    vertices[index] = new VertexPositionTexture(position, textureCoordinates);
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

        public void UnloadContent()
        {
            VertexBuffer.Dispose();
        }
    }
}
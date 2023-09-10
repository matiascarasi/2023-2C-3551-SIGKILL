using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP
{
    public class Terrain
    {

        private BasicEffect Effect;
        private VertexBuffer VertexBuffer;
        private IndexBuffer IndexBuffer;
        private int PrimitiveCount;
        private readonly string HeightmapPath;
        private readonly string TexturePath;

        public Terrain(string heightmapPath, string texturePath)
        {
            HeightmapPath = heightmapPath;
            TexturePath = texturePath;
        }

        public void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            var currentHeightmap = contentManager.Load<Texture2D>(HeightmapPath);

            var scaleXZ = 50f;
            var scaleY = 4f;
            CreateHeightmapMesh(graphicsDevice, currentHeightmap, scaleXZ, scaleY);

            var terrainTexture = contentManager.Load<Texture2D>(TexturePath);

            Effect = new BasicEffect(graphicsDevice)
            {
                World = Matrix.Identity,
                TextureEnabled = true,
                Texture = terrainTexture
            };
        }

        public void Draw(GraphicsDevice graphicsDevice, Matrix view, Matrix projection)
        {
            graphicsDevice.DepthStencilState = DepthStencilState.Default;
            graphicsDevice.SetVertexBuffer(VertexBuffer);
            graphicsDevice.Indices = IndexBuffer;

            Effect.View = view;
            Effect.Projection = projection;

            foreach (var pass in Effect.CurrentTechnique.Passes)
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

            CreateIndexBuffer(graphicsDevice, heightMapWidthMinusOne, heightMapLengthMinusOne);
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
                    heightmap[x, y] = texel.R;
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
                for (var z = 0; z < heightMapLength; z++)
                {
                    position = new Vector3(x * scaleXZ - offsetX, heightMap[x, z] * scaleY, z * scaleXZ - offsetZ);
                    textureCoordinates = new Vector2((float)x / heightMapWidth, (float)z / heightMapLength);
                    vertices[index] = new VertexPositionTexture(position, textureCoordinates);
                    index++;
                }

            VertexBuffer = new VertexBuffer(graphicsDevice, VertexPositionTexture.VertexDeclaration, vertexCount,
                BufferUsage.None);
            VertexBuffer.SetData(vertices);
        }


        private void CreateIndexBuffer(GraphicsDevice graphicsDevice, int quadsInX, int quadsInZ)
        {
            var indexCount = 3 * 2 * quadsInX * quadsInZ;

            var indices = new ushort[indexCount];
            var index = 0;

            int right;
            int top;
            int bottom;

            var vertexCountX = quadsInX + 1;
            for (var x = 0; x < quadsInX; x++)
                for (var z = 0; z < quadsInZ; z++)
                {
                    right = x + 1;
                    bottom = z * vertexCountX;
                    top = (z + 1) * vertexCountX;

                    //  d __ c  
                    //   | /|
                    //   |/_|
                    //  a    b

                    var a = (ushort)(x + bottom);
                    var b = (ushort)(right + bottom);
                    var c = (ushort)(right + top);
                    var d = (ushort)(x + top);

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
                new IndexBuffer(graphicsDevice, IndexElementSize.SixteenBits, indexCount, BufferUsage.None);
            IndexBuffer.SetData(indices);
        }

        public void UnloadContent()
        {
            VertexBuffer.Dispose();
        }
    }
}
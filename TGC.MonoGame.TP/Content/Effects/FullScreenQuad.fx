#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float4x4 WorldViewProjection;
float4x4 World;
float4x4 InverseTransposeWorld;

float3 ambientColor; // Light's Ambient Color
float3 diffuseColor; // Light's Diffuse Color
float3 specularColor; // Light's Specular Color
float KAmbient; 
float KDiffuse; 
float KSpecular;
float shininess; 
float3 lightPosition;
float3 eyePosition; // Camera position
float2 Tiling;

static const int kernel_r = 6;
static const int kernel_size = 13;
static const float Kernel[kernel_size] =
{
    0.002216, 0.008764, 0.026995, 0.064759, 0.120985, 0.176033, 0.199471, 0.176033, 0.120985, 0.064759, 0.026995, 0.008764, 0.002216,
};

float2 screenSize;

texture ModelTexture;
sampler2D textureSampler = sampler_state
{
    Texture = (ModelTexture);
    MagFilter = Linear;
    MinFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
    MIPFILTER = LINEAR;
};



//Textura para Normals
texture NormalTexture;
sampler2D normalSampler = sampler_state
{
    Texture = (NormalTexture);
    ADDRESSU = WRAP;
    ADDRESSV = WRAP;
    MINFILTER = LINEAR;
    MAGFILTER = LINEAR;
    MIPFILTER = LINEAR;
};

float3 getNormalFromMap(float2 textureCoordinates, float3 worldPosition, float3 worldNormal)
{
    float3 tangentNormal = tex2D(normalSampler, textureCoordinates).xyz * 2.0 - 1.0;

    float3 Q1 = ddx(worldPosition);
    float3 Q2 = ddy(worldPosition);
    float2 st1 = ddx(textureCoordinates);
    float2 st2 = ddy(textureCoordinates);

    worldNormal = normalize(worldNormal.xyz);
    float3 T = normalize(Q1 * st2.y - Q2 * st1.y);
    float3 B = -normalize(cross(worldNormal, T));
    float3x3 TBN = float3x3(T, B, worldNormal);

    return normalize(mul(tangentNormal, TBN));
}

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float2 TextureCoordinates : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float2 TextureCoordinates : TEXCOORD0;
};


VertexShaderOutput MainVS(in VertexShaderInput input)
{
    VertexShaderOutput output;
    output.Position = input.Position;
    output.TextureCoordinates = input.TextureCoordinates;
    return output;
}


float4 BlurPS(in VertexShaderOutput input) : COLOR
{
    float4 finalColor = float4(0, 0, 0, 1);
    for (int x = 0; x < kernel_size; x++)
        for (int y = 0; y < kernel_size; y++)
        {
            float2 scaledTextureCoordinates = input.TextureCoordinates + float2((float) (x - kernel_r) / screenSize.x, (float) (y - kernel_r) / screenSize.y);
            finalColor += tex2D(textureSampler, scaledTextureCoordinates) * Kernel[x] * Kernel[y];
        }
    return finalColor;
}

float4 BlurHorizontal(in VertexShaderOutput input) : COLOR
{
    float4 finalColor = float4(0, 0, 0, 1);
    for (int i = 0; i < kernel_size; i++)
    {
        float2 scaledTextureCoordinates = input.TextureCoordinates + float2((float) (i - kernel_r) / screenSize.x, 0);
        finalColor += tex2D(textureSampler, scaledTextureCoordinates) * Kernel[i];
    }
    return finalColor;
}

float4 BlurVertical(in VertexShaderOutput input) : COLOR
{
    float4 finalColor = float4(0, 0, 0, 1);
    for (int i = 0; i < kernel_size; i++)
    {
        float2 scaledTextureCoordinates = input.TextureCoordinates + float2(0, (float) (i - kernel_r) / screenSize.y);
        finalColor += tex2D(textureSampler, scaledTextureCoordinates) * Kernel[i];
    }
    return finalColor;
}



technique Blur
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL MainVS();
        PixelShader = compile PS_SHADERMODEL BlurPS();
    }
};

technique BlurHorizontalTechnique
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL MainVS();
        PixelShader = compile PS_SHADERMODEL BlurHorizontal();
    }
};

technique BlurVerticalTechnique
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL MainVS();
        PixelShader = compile PS_SHADERMODEL BlurVertical();
    }
};



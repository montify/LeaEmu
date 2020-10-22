
float4x4 Projection;
Texture2D fontAtlas;

RasterizerState MyCull
{
	//FrontCounterClockwise = FALSE; // or FALSE
    FillMode = solid;
};

struct VS_IN
{
    float2 pos : POSITION;
    float2 uv : TEXCOORD;
    float4 col : COLOR;

};

struct PS_IN
{
    float4 pos : SV_POSITION0;
    float2 uv : TEXCOORD0;
    float4 col : COLOR;
	
};

SamplerState TrilinearSampler
{
    Filter = MIN_MAG_MIP_Linear;
    AddressU = wrap;
    AddressV = wrap;
};

PS_IN VS(VS_IN input)
{
    PS_IN output = (PS_IN) 0;


    output.pos = mul(float4(input.pos, 0, 1), Projection);
    output.uv = input.uv;
    output.col = input.col;

    return output;
}

float4 PS(PS_IN input) : SV_Target0
{
    return input.col * fontAtlas.Sample(TrilinearSampler, input.uv);
}

technique11 Render
{
    pass P0
    {
        SetRasterizerState(MyCull);
        SetVertexShader(CompileShader(vs_5_0, VS()));
        SetDomainShader(NULL);
        SetHullShader(NULL);
        SetPixelShader(CompileShader(ps_5_0, PS()));
    }
}
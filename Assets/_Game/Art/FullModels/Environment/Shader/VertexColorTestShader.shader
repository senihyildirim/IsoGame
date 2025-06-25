Shader "Custom/VertexColorTestShader"
{
    Properties
    {
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR; // This will hold the vertex color data
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 color : COLOR; // Pass the vertex color to the fragment shader
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex); // Standard vertex transformation
                o.color = v.color; // Pass the vertex color to the fragment shader
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return i.color; // Output the vertex color directly
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}

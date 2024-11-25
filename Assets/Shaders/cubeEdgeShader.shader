Shader "Custom/Wireframe"
{
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma geometry geom
            #pragma fragment frag
            #pragma target 4.0

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2g
            {
                float4 pos : POSITION;
            };

            struct g2f
            {
                float4 pos : SV_POSITION;
                float4 color : COLOR;
            };

            v2g vert(appdata v)
            {
                v2g o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            [maxvertexcount(6)]
            void geom(triangle v2g input[3], inout LineStream<g2f> output)
            {
                for (int i = 0; i < 3; ++i)
                {
                    g2f o;
                    o.pos = input[i].pos;
                    o.color = float4(1, 1, 1, 1); // White color for edges
                    output.Append(o);
                }
            }

            float4 frag(g2f i) : SV_Target
            {
                return i.color;
            }
            ENDCG
        }
    }
}

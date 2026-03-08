Shader "Custom/Simple Diffuse"
{
    Properties
    {
        _Color("Color", Color) = (1, 1, 1, 1)
        _Ambient("Ambient", Color) = (0.5, 0.5, 0.5, 1)
        _MainTex("Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "LightMode" = "ForwardBase"
        }

        Pass
        {
            ZWrite On
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Back

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            uniform fixed4 _Color;
            uniform fixed4 _Ambient;
            uniform sampler2D _MainTex;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float3 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 uv : TEXCOORD0;
                fixed3 diff : COLOR0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;

                half3 worldNormal = UnityObjectToWorldNormal(v.normal);
                half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));

                o.diff = nl * _LightColor0.rgb;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                return col * half4(i.diff + _Ambient.rgb, 1);
            }

            ENDCG
        }
    }
}

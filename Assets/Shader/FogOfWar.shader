Shader "Custom/FogOfWar"
{
    // Define shader properties that can be set in the Unity Inspector
    Properties {
        _MainTex ("Current Vision", 2D) = "black" {}    // Current vision range texture
        _ExploredTex ("Explored Area", 2D) = "black" {} // Previously explored areas texture
        _FogColor ("Fog Color", Color) = (0,0,0,1)      // Color of the fog
        [Toggle] _DebugMode ("Debug Mode", Float) = 0    // Toggle for debug visualization
    }
    
    SubShader {
        // Set rendering order and type
        Tags {
            "Queue" = "Transparent" 
            "RenderType" = "Transparent"
        }
        
        Pass {
            // Enable alpha blending
            Blend SrcAlpha OneMinusSrcAlpha
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // Input vertex data
            struct appdata {
                float4 vertex : POSITION;  // Vertex position
                float2 uv : TEXCOORD0;     // Texture coordinates
            };

            // Data passed from vertex to fragment shader
            struct v2f {
                float2 uv : TEXCOORD0;     // Texture coordinates
                float4 vertex : SV_POSITION; // Clip space position
            };

            // Variables to receive the properties
            sampler2D _MainTex;
            sampler2D _ExploredTex;
            fixed4 _FogColor;
            float _DebugMode;

            // Vertex shader
            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // Fragment/pixel shader
            fixed4 frag (v2f i) : SV_Target {
                // Sample both textures at the current UV coordinate
                fixed4 currentVision = tex2D(_MainTex, i.uv);
                fixed4 exploredArea = tex2D(_ExploredTex, i.uv);

                // Debug visualization mode
                if (_DebugMode > 0.5) {
                    // Red channel shows current vision
                    // Green channel shows explored areas
                    return fixed4(currentVision.r, exploredArea.r, 0, 1);
                }

                // Normal fog of war rendering
                if (currentVision.r > 0.1) {
                    return fixed4(0,0,0,0);     // Fully visible - no fog
                }
                else if (exploredArea.r > 0.1) {
                    return fixed4(_FogColor.rgb, 0.5); // Explored but not visible - partial fog
                }
                else {
                    return _FogColor;           // Unexplored - full fog
                }
            }
            ENDCG
        }
    }
}

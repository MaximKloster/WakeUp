// Shader created with Shader Forge Beta 0.30 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.30;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:True,lprd:True,enco:True,frtr:True,vitr:True,dbil:True,rmgx:True,hqsc:True,hqlp:False,blpr:0,bsrc:0,bdst:0,culm:0,dpts:2,wrdp:True,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.2,fgrn:10,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:32719,y:32712|diff-196-OUT,spec-30-OUT,gloss-37-OUT,emission-49-OUT,amdfl-61-OUT,amspl-215-OUT;n:type:ShaderForge.SFN_Color,id:2,x:33464,y:32218,ptlb:Color,ptin:_Color,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Tex2d,id:3,x:33456,y:32391,ptlb:Diffuse,ptin:_Diffuse,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:4,x:33244,y:32311|A-2-RGB,B-3-RGB;n:type:ShaderForge.SFN_ValueProperty,id:30,x:33403,y:32648,ptlb:Specular,ptin:_Specular,glob:False,v1:0.05;n:type:ShaderForge.SFN_ValueProperty,id:37,x:33403,y:32745,ptlb:Gloss,ptin:_Gloss,glob:False,v1:0.01;n:type:ShaderForge.SFN_ValueProperty,id:43,x:34098,y:32964,ptlb:Emission,ptin:_Emission,glob:False,v1:-1;n:type:ShaderForge.SFN_Exp,id:45,x:33757,y:32879,et:0|IN-48-OUT;n:type:ShaderForge.SFN_Vector1,id:47,x:34098,y:32879,v1:10;n:type:ShaderForge.SFN_Multiply,id:48,x:33921,y:32879|A-47-OUT,B-43-OUT;n:type:ShaderForge.SFN_Multiply,id:49,x:33575,y:32790|A-50-RGB,B-45-OUT;n:type:ShaderForge.SFN_Color,id:50,x:34072,y:32708,ptlb:Emission Color,ptin:_EmissionColor,glob:False,c1:1,c2:0.3931035,c3:0,c4:1;n:type:ShaderForge.SFN_Cubemap,id:56,x:33879,y:33078,ptlb:Diffuse Ambient,ptin:_DiffuseAmbient,cube:f466cf7415226e046b096197eb7341aa,pvfc:0|DIR-58-OUT,MIP-95-OUT;n:type:ShaderForge.SFN_NormalVector,id:58,x:34284,y:33037,pt:True;n:type:ShaderForge.SFN_ValueProperty,id:60,x:33840,y:33270,ptlb:Cubemap Brightness,ptin:_CubemapBrightness,glob:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:61,x:33523,y:33045|A-56-RGB,B-60-OUT;n:type:ShaderForge.SFN_Multiply,id:76,x:33341,y:33382|A-77-RGB,B-60-OUT;n:type:ShaderForge.SFN_Cubemap,id:77,x:33639,y:33318,ptlb:Specular Ambient,ptin:_SpecularAmbient,cube:a596436b21c6d484bb9b3b6385e3e666,pvfc:0|DIR-78-OUT,MIP-116-OUT;n:type:ShaderForge.SFN_ViewReflectionVector,id:78,x:33957,y:33434;n:type:ShaderForge.SFN_Slider,id:95,x:34284,y:33211,ptlb:Ambient Blurriness,ptin:_AmbientBlurriness,min:1,cur:5,max:7;n:type:ShaderForge.SFN_Slider,id:116,x:34169,y:33399,ptlb:Specular Bluriness,ptin:_SpecularBluriness,min:1,cur:1,max:7;n:type:ShaderForge.SFN_Fresnel,id:195,x:33270,y:31986|NRM-208-OUT,EXP-202-OUT;n:type:ShaderForge.SFN_Add,id:196,x:32986,y:32227|A-195-OUT,B-4-OUT;n:type:ShaderForge.SFN_ValueProperty,id:202,x:33464,y:32053,ptlb:Fresnel,ptin:_Fresnel,glob:False,v1:0;n:type:ShaderForge.SFN_NormalVector,id:208,x:33557,y:31808,pt:True;n:type:ShaderForge.SFN_Add,id:215,x:33066,y:33070|A-195-OUT,B-76-OUT;proporder:2-3-30-37-43-50-56-60-77-95-116-202;pass:END;sub:END;*/

Shader "WakeUp/IBL Ubershader" {
    Properties {
        _Color ("Color", Color) = (0.5,0.5,0.5,1)
        _Diffuse ("Diffuse", 2D) = "white" {}
        _Specular ("Specular", Float ) = 0.05
        _Gloss ("Gloss", Float ) = 0.01
        _Emission ("Emission", Float ) = -1
        _EmissionColor ("Emission Color", Color) = (1,0.3931035,0,1)
        _DiffuseAmbient ("Diffuse Ambient", Cube) = "_Skybox" {}
        _CubemapBrightness ("Cubemap Brightness", Float ) = 1
        _SpecularAmbient ("Specular Ambient", Cube) = "_Skybox" {}
        _AmbientBlurriness ("Ambient Blurriness", Range(1, 7)) = 5
        _SpecularBluriness ("Specular Bluriness", Range(1, 7)) = 1
        _Fresnel ("Fresnel", Float ) = 0
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            #pragma glsl
            #ifndef LIGHTMAP_OFF
                sampler2D unity_Lightmap;
                float4 unity_LightmapST;
                #ifndef DIRLIGHTMAP_OFF
                    sampler2D unity_LightmapInd;
                #endif
            #endif
            uniform float4 _Color;
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform float _Specular;
            uniform float _Gloss;
            uniform float _Emission;
            uniform float4 _EmissionColor;
            uniform samplerCUBE _DiffuseAmbient;
            uniform float _CubemapBrightness;
            uniform samplerCUBE _SpecularAmbient;
            uniform float _AmbientBlurriness;
            uniform float _SpecularBluriness;
            uniform float _Fresnel;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float4 uv0 : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 binormalDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
                #ifndef LIGHTMAP_OFF
                    float2 uvLM : TEXCOORD7;
                #else
                    float3 shLight : TEXCOORD7;
                #endif
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.uv0;
                #ifdef LIGHTMAP_OFF
                    o.shLight = ShadeSH9(float4(v.normal * unity_Scale.w,1));
                #endif
                o.normalDir = mul(float4(v.normal,0), _World2Object).xyz;
                o.tangentDir = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                #ifndef LIGHTMAP_OFF
                    o.uvLM = v.uv1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
                #endif
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.binormalDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
/////// Normals:
                float3 normalDirection =  i.normalDir;
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                #ifndef LIGHTMAP_OFF
                    float4 lmtex = tex2D(unity_Lightmap,i.uvLM);
                    #ifndef DIRLIGHTMAP_OFF
                        float3 lightmap = DecodeLightmap(lmtex);
                        float3 scalePerBasisVector = DecodeLightmap(tex2D(unity_LightmapInd,i.uvLM));
                        UNITY_DIRBASIS
                        half3 normalInRnmBasis = saturate (mul (unity_DirBasis, float3(0,0,1)));
                        lightmap *= dot (normalInRnmBasis, scalePerBasisVector);
                    #else
                        float3 lightmap = DecodeLightmap(tex2D(unity_Lightmap,i.uvLM));
                    #endif
                #endif
                #ifndef LIGHTMAP_OFF
                    #ifdef DIRLIGHTMAP_OFF
                        float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                    #else
                        float3 lightDirection = normalize (scalePerBasisVector.x * unity_DirBasis[0] + scalePerBasisVector.y * unity_DirBasis[1] + scalePerBasisVector.z * unity_DirBasis[2]);
                        lightDirection = mul(lightDirection,tangentTransform); // Tangent to world
                    #endif
                #else
                    float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                #endif
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i)*2;
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
                float NdotL = dot( normalDirection, lightDirection );
                #ifndef LIGHTMAP_OFF
                    float3 diffuse = lightmap;
                #else
                    float3 diffuse = max( 0.0, NdotL)*InvPi * attenColor;
                #endif
////// Emissive:
                float3 emissive = (_EmissionColor.rgb*exp((10.0*_Emission)));
///////// Gloss:
                float gloss = exp2(_Gloss*10.0+1.0);
////// Specular:
                NdotL = max(0.0, NdotL);
                float node_195 = pow(1.0-max(0,dot(normalDirection, viewDirection)),_Fresnel);
                float4 node_77 = texCUBElod(_SpecularAmbient,float4(viewReflectDirection,_SpecularBluriness));
                float3 specularColor = float3(_Specular,_Specular,_Specular);
                float specularMonochrome = dot(specularColor,float3(0.3,0.59,0.11));
                float normTerm = (gloss + 8.0 ) / (8.0 * Pi);
                float3 specularAmb = (node_195+(node_77.rgb*_CubemapBrightness)) * specularColor;
                float3 specular = (floor(attenuation) * _LightColor0.xyz) * pow(max(0,dot(halfDirection,normalDirection)),gloss) * specularColor*normTerm + specularAmb;
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                float4 node_56 = texCUBElod(_DiffuseAmbient,float4(normalDirection,_AmbientBlurriness));
                diffuseLight += (node_56.rgb*_CubemapBrightness); // Diffuse Ambient Light
                #ifdef LIGHTMAP_OFF
                    diffuseLight += i.shLight; // Per-Vertex Light Probes / Spherical harmonics
                #endif
                diffuseLight *= 1-specularMonochrome;
                float2 node_228 = i.uv0;
                float4 node_3 = tex2D(_Diffuse,TRANSFORM_TEX(node_228.rg, _Diffuse));
                finalColor += diffuseLight * (node_195+(_Color.rgb*node_3.rgb));
                finalColor += specular;
                finalColor += emissive;
/// Final Color:
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "ForwardAdd"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            Fog { Color (0,0,0,0) }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            #pragma glsl
            #ifndef LIGHTMAP_OFF
                sampler2D unity_Lightmap;
                float4 unity_LightmapST;
                #ifndef DIRLIGHTMAP_OFF
                    sampler2D unity_LightmapInd;
                #endif
            #endif
            uniform float4 _Color;
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform float _Specular;
            uniform float _Gloss;
            uniform float _Emission;
            uniform float4 _EmissionColor;
            uniform float _Fresnel;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float4 uv0 : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 binormalDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.uv0;
                o.normalDir = mul(float4(v.normal,0), _World2Object).xyz;
                o.tangentDir = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.binormalDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
/////// Normals:
                float3 normalDirection =  i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i)*2;
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
                float NdotL = dot( normalDirection, lightDirection );
                float3 diffuse = max( 0.0, NdotL)*InvPi * attenColor;
///////// Gloss:
                float gloss = exp2(_Gloss*10.0+1.0);
////// Specular:
                NdotL = max(0.0, NdotL);
                float3 specularColor = float3(_Specular,_Specular,_Specular);
                float specularMonochrome = dot(specularColor,float3(0.3,0.59,0.11));
                float normTerm = (gloss + 8.0 ) / (8.0 * Pi);
                float3 specular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),gloss) * specularColor*normTerm;
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                diffuseLight *= 1-specularMonochrome;
                float node_195 = pow(1.0-max(0,dot(normalDirection, viewDirection)),_Fresnel);
                float2 node_229 = i.uv0;
                float4 node_3 = tex2D(_Diffuse,TRANSFORM_TEX(node_229.rg, _Diffuse));
                finalColor += diffuseLight * (node_195+(_Color.rgb*node_3.rgb));
                finalColor += specular;
/// Final Color:
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}

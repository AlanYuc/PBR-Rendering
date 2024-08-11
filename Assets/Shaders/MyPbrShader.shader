
Shader "MyPBR/MyPbrRenderingShader"
{
    Properties
    {
        _AlbedoMap ("Albedo Map", 2D) = "white" {}
        _NormalMap ("Normal Map", 2D) = "bump" {}
        _AOMap ("AO Map", 2D) = "white" {}
        _MetallicMap ("Metallic Map" , 2D) = "black" {}
        _SpecularMap ("Specular Map" , 2D) = "white" {}
        _HeightMap ("HeightMap" , 2D) = "black"{}

        _BumpScale ("Bump Scale" , Range(-100,100)) = 0
        _Metallic ("Metallic", Range(0, 1)) = 0
        _Roughness ("Roughness", Range(0, 1)) = 0.5
        _HeightScale ("HeightScale" , Range(0 , 2)) = 0
        _Ao ("Ao" , Range(0, 1)) = 1

        _ColorTint ("Color Tint", Color) = (1, 1, 1, 1)
        _Gloss ("Gloss" , Range(2.0 , 64)) = 20
        _GammaCorrect ("Gamma Correct" , Range(1 , 5)) = 2.2

        _SpecularMapScale ("__SpecularMap Scale", Range(0,100)) = 1
        _SpecularIntensity ("Specular Intensity", Range(1.0,100)) = 1.0

        _UseMetallicMap ("Use MetallicMap" , Float) = 0
        _UseCook_Torrance("Use Cook-Torrance" , Float) = 0
        _UseKajiya_Kay ("Use Kajiya-Kay" , Float) = 0
        _UseAshikhmin_Shirley ("Use Ashikhmin-Shirley" , Float) = 0
        _UseChristensen_Burley ("Use Christensen-Burley" , Float) = 0
        _UseOren_Nayar ("Use Oren-Nayar" , Float) = 0
        _UsePBR_Disney ("Use PBR Disney" , Float) = 0

        _BrightnessU ("Shininess U", Range(0.1, 100)) = 30.0
        _BrightnessV ("Shininess V", Range(0.1, 100)) = 30.0
        _SpecularDiffuseScale ("Specular Reflectance", Range(0.0, 1.0)) = 0.5
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            Tags { "LightMode" = "ForwardBase" }
            CGPROGRAM
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "UnityStandardUtils.cginc"
            #include "UnityPBSLighting.cginc"

            struct a2v
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
                float4 tangent : TANGENT;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float3 worldNormal : TEXCOORD2;
                float3 worldTangent : TEXCOORD3;
                float3 worldBinormal : TEXCOORD4;
            };

            sampler2D _AlbedoMap;
            sampler2D _NormalMap;
            sampler2D _AOMap;
            sampler2D _MetallicMap;
            sampler2D _SpecularMap;
            sampler2D _HeightMap;
            float _Metallic;
            float _Roughness;
            float4 _ColorTint;
            float _BumpScale;
            float _Gloss;
            float _Ao;
            float _GammaCorrect;

            float _HeightScale;
            float _SpecularMapScale;
            float _SpecularIntensity;

            float _UseMetallicMap;
            float _UseCook_Torrance;
            float _UseKajiya_Kay;
            float _UseAshikhmin_Shirley;
            float _UseChristensen_Burley;
            float _UseOren_Nayar;
            float _UsePBR_Disney;

            float _BrightnessU;
            float _BrightnessV;
            float _SpecularDiffuseScale;

            float DistributionGGX(float3 N, float3 H, float R)
            {
                float a = R * R;
                float a2 = a * a;
                float NdotH = max(dot(N, H), 0.0);
                float NdotH2 = NdotH * NdotH;

                float nom = a2;
                float denom = (NdotH2 * (a2 - 1.0) + 1.0);
                denom = UNITY_PI * denom * denom;

                return nom / denom;
            }

            float GeometrySchlickGGX(float NdotV, float R)
            {
                float r = (R + 1.0);
                float k = (r * r) / 8.0;

                float nom = NdotV;
                float denom = NdotV * (1.0 - k) + k;

                return nom / denom;
            }

            float GeometrySmith(float3 N, float3 V, float3 L, float R)
            {
                float NdotV = max(dot(N, V), 0.0);
                float NdotL = max(dot(N, L), 0.0);
                float ggx2 = GeometrySchlickGGX(NdotV, R);
                float ggx1 = GeometrySchlickGGX(NdotL, R);

                return ggx1 * ggx2;
            }

            float3 fresnelSchlick(float cosTheta, float3 F0)
            {
                return F0 + (1.0 - F0) * pow(1.0 - cosTheta, 5.0);
            }

            //KajiyaKay
            float3 KajiyaKaySpecular(float3 L, float3 V, float3 N, float3 H, float3 T, float R)
             {
                float3 Ht = normalize(H - dot(H, T) * T);
                float3 Lt = normalize(L - dot(L, T) * T);
                float3 Vt = normalize(V - dot(V, T) * T);

                float Ht_dot_N = max(dot(Ht, N), 0.0);
                float Vt_dot_Ht = max(dot(Vt, Ht), 0.0);
                float Vt_dot_T = max(dot(Vt, T), 0.0);

                float specular = pow(Ht_dot_N, 1.0 / (R + 0.001)) / (4.0 * Vt_dot_Ht * Vt_dot_T + 0.0001);

                return float3(specular, specular, specular);
             }

            //***********AshikhminShirley***********************
            float AshikhminShirley_D(float3 H, float3 N, float shininessU, float shininessV)
            {
                float NH = saturate(dot(N, H));
                float HU = H.x;
                float HV = H.z;

                float e = shininessU * HU * HU + shininessV * HV * HV;
                float normalizationFactor = sqrt((shininessU + 1.0) * (shininessV + 1.0)) / (8.0 * UNITY_PI);

                return normalizationFactor * pow(NH, e);
            }

            float AshikhminShirley_G(float3 V, float3 L, float3 N)
            {
                float NV = saturate(dot(N, V));
                float NL = saturate(dot(N, L));
                return 2.0 * NV * NL / (NV + NL);
            }

            float3 AshikhminShirley_Diffuse(float3 L, float3 V, float3 N, float3 diffuseColor, float S)
            {
                float NV = saturate(dot(N, V));
                float NL = saturate(dot(N, L));

                float diffuseFactor = (28.0 / (23.0 * UNITY_PI)) * (1.0 - S / 2.0) * (1.0 - pow(1.0 - NL / 2.0, 5.0)) * (1.0 - pow(1.0 - NV / 2.0, 5.0));

                return diffuseColor * diffuseFactor;
            }
            //**********************************

            //Christensen Burley BRDF
            float3 ChristensenBurleySpecular(float3 L, float3 V, float3 N, float3 H, float R, float F)
            {
                //s=1.9 - A + 3.5(A - 0.8)2
                float a = R;
                float s = 1.9 - a + 3.5 * pow(a - 0.8, 2.0);

                float NdotH = max(dot(N, H), 0.0);
                float D = exp(-pow(NdotH - 1.0, 2.0) / (s * s)) / (UNITY_PI * s * s * pow(NdotH, 4.0));

                float3 specularCB = (D * F) / (4.0 * max(dot(N, L), 0.001) * max(dot(N, V), 0.001));
                return specularCB;
            }
            //

            //Oren-Nayar BRDF
            float3 OrenNayarDiffuse(float3 L, float3 V, float3 N, float3 albedo, float R)
            {
                float3 H = normalize(L + V);
                float A = 1.0 - 0.5 * (R * R) / (R * R + 0.33);
                float B = 0.45 * (R * R) / (R * R + 0.09);
                
                float NdotL = max(dot(N, L), 0.0);
                float NdotV = max(dot(N, V), 0.0);
                float LdotV = max(dot(L, V), 0.0);

                float3 diffuseColor = albedo / UNITY_PI;
                float i = acos(NdotL);
                float o = acos(NdotV);
                float delta = abs(i - o);

                float part = max(0.0, cos(i) * cos(o) - sin(i) * sin(o) * cos(delta));
                float all = A + B * part;

                return diffuseColor * all;
            }
            //

            //Disney BRDF

            float3 DisneyDiffuse(float3 L, float3 V, float3 N, float R, float3 H)
            {
                float NdotL = max(dot(N, L), 0.0);
                float NdotV = max(dot(N, V), 0.0);
                float VdotH = max(dot(V, H), 0.0);
                
                float FD90 = 0.5 + 2 * VdotH * VdotH * R;
                float FV = 1 + (FD90 - 1) * Pow5(1 - NdotV);
                float FL = 1 + (FD90 - 1) * Pow5(1 - NdotL);
                return ( UNITY_PI * FV * FL );
            }
            //



            v2f vert(a2v v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.uv = v.uv;

                float3 worldNormal = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));
                float3 worldTangent = normalize(mul((float3x3)unity_ObjectToWorld, v.tangent.xyz));
                float3 worldBinormal = cross(worldNormal, worldTangent) * v.tangent.w;

                o.worldNormal = worldNormal;
                o.worldTangent = worldTangent;
                o.worldBinormal = worldBinormal;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float3 albedo = pow(tex2D(_AlbedoMap, i.uv).rgb * _ColorTint.rgb, _GammaCorrect);
                //float3 albedo = pow(tex2D(_AlbedoMap, i.uv).rgb * _ColorTint.rgb, 2.2);
                
                float metallic = _Metallic;
                if(_UseMetallicMap == 1){
                    //metallic  = tex2D(_MetallicMap , i.uv).r;
                    metallic  = tex2D(_MetallicMap , i.uv).r * _Metallic;
                }
                
                float roughness = _Roughness;
                float ao = tex2D(_AOMap, i.uv).r;
                //float3 specularMapValue = tex2D(_SpecularMap , i.uv).rgb;
                float4 specularMapValue = tex2D(_SpecularMap , i.uv) * _SpecularMapScale;


                float3 normal = UnpackNormal(tex2D(_NormalMap, i.uv));
                //normal = (normal * 0.5) + float3(0.5, 0.5, 0.5);
                normal.xy *= _BumpScale;
                normal.z = sqrt(1 - saturate(dot(normal.xy , normal.xy)));
                normal = normalize(mul(float3x3(i.worldTangent, i.worldBinormal, i.worldNormal), normal));
                

                //HeightMap
                //float heightValue = tex2D(_HeightMap, i.uv).r;
                //i.worldPos += normal * heightValue;

                float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);

                float3 F0 = float3(0.04,0.04,0.04);
                F0 = lerp(F0, albedo, metallic);

                //float3 lightDir = normalize(_WorldSpaceLightPos0.xyz - i.worldPos);
                float3 lightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));
                float3 halfV = normalize(viewDir + lightDir);
                float distance = length(_WorldSpaceLightPos0.xyz - i.worldPos);
                //float attenuation = 1.0 / (distance * distance);
                //float3 radiance = _LightColor0.rgb * attenuation;
                float3 radiance = _LightColor0.rgb;


                // Cook-Torrance BRDF
                float NDF = DistributionGGX(normal, halfV, roughness);
                float G = GeometrySmith(normal, viewDir, lightDir, roughness);
                float3 F = fresnelSchlick(max(dot(halfV, viewDir), 0.0), F0);

                float3 numerator = NDF * G * F;
                float denominator = 4.0 * max(dot(normal, viewDir), 0.0) * max(dot(normal, lightDir), 0.0) + 0.0001;
                float3 specularCT = numerator / denominator;


                //KajiyaKaySpecular
                float3 specularKK = KajiyaKaySpecular(lightDir , viewDir , normal , halfV , i.worldTangent , roughness);

                //Ashikhmin-Shirley
                float AS_D = AshikhminShirley_D(halfV, normal, _BrightnessU, _BrightnessV);
                float3 AS_F = F;
                float AS_G = AshikhminShirley_G(viewDir, lightDir, normal);

                float3 specularAS = (AS_D * AS_F * AS_G) / (4.0 * max(dot(normal, viewDir), 0.001) * max(dot(normal, lightDir), 0.001));
                float3 diffuseAS = AshikhminShirley_Diffuse(lightDir, viewDir, normal, albedo, _SpecularDiffuseScale);

                //Christensen Burley
                float3 specularCB = ChristensenBurleySpecular(lightDir, viewDir, normal, halfV, _Roughness, F0);
                //float3 diffuseCB = albedo * radiance / UNITY_PI;
                float3 diffuseCB = albedo * radiance;

                //Oren-Nayar BRDF
                float3 diffuseON = OrenNayarDiffuse(lightDir, viewDir, normal, albedo, _Roughness);

                //Disney
                float3 diffuseD = DisneyDiffuse(lightDir, viewDir, normal, _Roughness, halfV);


                float3 kS = F;
                float3 kD = float3(1.0,1.0,1.0) - kS;
                kD *= 1.0 - metallic;

                float NdotL = max(dot(normal, lightDir), 0.0);

                //float3 specularFinal = specular * radiance * pow(NdotL , _Gloss) * pow(specularMapValue.rgb , _SpecularScale);
                //float3 specularFinal = specular * radiance * pow(NdotL , _Gloss) * specularMapValue.rgb * _SpecularIntensity;
                //float3 diffuseFinal = kD * albedo / UNITY_PI * radiance * NdotL;
                //diffuseFinal = diffuseAS * albedo * radiance * NdotL;

                //set Cook-Torrance as default
                //float3 specularFinal = specular * radiance * pow(NdotL , _Gloss) * _SpecularIntensity;
                //float3 diffuseFinal = kD * albedo * radiance * NdotL;
                

                float3 specular;
                float3 diffuse;
                if(_UseCook_Torrance == 1){
                    specular = specularCT * radiance * pow(NdotL , _Gloss) * _SpecularIntensity;
                    diffuse = kD * albedo * radiance * NdotL;
                }
                else if(_UseKajiya_Kay == 1){
                    specular = specularKK * radiance * pow(NdotL , _Gloss) * _SpecularIntensity;
                    diffuse = kD * albedo * radiance * NdotL;
                }
                else if(_UseAshikhmin_Shirley == 1){
                    specular = specularAS * radiance * pow(NdotL , _Gloss) * _SpecularIntensity;
                    diffuse = diffuseAS;
                }
                else if(_UseChristensen_Burley == 1){
                    specular = specularCB * radiance * pow(NdotL , _Gloss) * _SpecularIntensity;
                    diffuse = diffuseCB;
                }
                else if(_UseOren_Nayar == 1){
                    specular = specularCT * radiance * pow(NdotL , _Gloss) * _SpecularIntensity;
                    diffuse = diffuseON;
                }
                else if(_UsePBR_Disney == 1){
                    specular = specularCT * radiance * pow(NdotL , _Gloss) * _SpecularIntensity;
                    diffuse = albedo * diffuseD / UNITY_PI;
                }
                else{
                    specular = specularCT * radiance * pow(NdotL , _Gloss) * _SpecularIntensity;
                    diffuse = kD * albedo * radiance * NdotL;
                }


                //simple IBL
                float3 ambientIBL = ShadeSH9(float4(i.worldNormal , 1));
                ambientIBL = max(0,ambientIBL);
                //diffuse = diffuse * ambientIBL;


                //float3 ambient = float3(0.03,0.03,0.03) * albedo * ao;
                //float3 ambient = UNITY_LIGHTMODEL_AMBIENT * albedo * ao * ambientIBL;
                float3 ambient = albedo * ao * ambientIBL;
                float3 color = ambient + specular + diffuse;

                color = color / (color + float3(1.0,1.0,1.0));
                color = pow(color, 1.0 / _GammaCorrect);

                return float4(color, 1.0);
            }
            ENDCG
        }
    }
}

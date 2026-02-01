#ifndef MAINLIGHT_INCLUDED
#define MAINLIGHT_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

void GetMainLightData_float(
    out half3 direction,
    out half3 color,
    out half distanceAttenuation,
    out half shadowAttenuation
)
{
    // ===== INICIALIZACIÓN OBLIGATORIA (Shader Graph) =====
    direction = half3(0, -1, 0);
    color = half3(1, 1, 1);
    distanceAttenuation = 1.0;
    shadowAttenuation = 1.0;

#ifdef SHADERGRAPH_PREVIEW
    // Preview de Shader Graph
    direction = normalize(half3(-0.3, -0.8, 0.6));

#else

#if defined(UNIVERSAL_LIGHTING_INCLUDED)

        Light mainLight;

#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
            float3 worldPos = float3(0, 0, 0);
            float4 shadowCoord = TransformWorldToShadowCoord(worldPos);
            mainLight = GetMainLight(shadowCoord);
#else
            mainLight = GetMainLight();
#endif

        direction = mainLight.direction;
        color = mainLight.color;
        distanceAttenuation = mainLight.distanceAttenuation;
        shadowAttenuation = mainLight.shadowAttenuation;

#endif

#endif
}

#endif

#version 330 core
out vec4 FragColor;

in vec2 TexCoords;

uniform sampler2D scene;
uniform sampler2D bloomBlur;
uniform bool bloom;
uniform bool hdr;
uniform bool gamma;
uniform float exposure;

void main()
{
    const float gammaValue = 2.2;
    vec3 hdrColor = texture(scene, TexCoords).rgb;
    vec3 bloomColor = texture(bloomBlur, TexCoords).rgb;

    if(bloom) {
        hdrColor += bloomColor; // additive blending
    }

    vec3 result = hdrColor;

    if(hdr) {
        result = vec3(1.0) - exp(-hdrColor*exposure);
        result = pow(result, vec3(1.0 / gammaValue));
    }

    if(gamma && !hdr) {
        result = pow(result, vec3(1.0 / gammaValue));
    }

    FragColor = vec4(result, 1.0);
}
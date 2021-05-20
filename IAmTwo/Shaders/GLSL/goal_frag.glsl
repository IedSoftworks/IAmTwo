#version 330

in vec3 v_VertexPosition;

uniform float yPos;
uniform float Brightness;

uniform vec4 Color;

layout(location = 0) out vec4 color;

#define rot(x, k) (((x) << (k)) | ((x) >> (32 - (k))))
#define final(a, b, c)  { c ^= b; c -= rot(b, 14); a ^= c; a -= rot(c, 11); b ^= a; b -= rot(a, 25); c ^= b; c -= rot(b, 16); a ^= c; a -= rot(c, 4); b ^= a; b -= rot(a, 14); c ^= b; c -= rot(b, 24); }
#define FLOORFRAC(x, x_int, x_fract) { float x_floor = floor(x); x_int = int(x_floor); x_fract = x - x_floor; }

mat3 euler_to_mat3(vec3 euler)
{
  float cx = cos(euler.x);
  float cy = cos(euler.y);
  float cz = cos(euler.z);
  float sx = sin(euler.x);
  float sy = sin(euler.y);
  float sz = sin(euler.z);

  mat3 mat;
  mat[0][0] = cy * cz;
  mat[0][1] = cy * sz;
  mat[0][2] = -sy;

  mat[1][0] = sy * sx * cz - cx * sz;
  mat[1][1] = sy * sx * sz + cx * cz;
  mat[1][2] = cy * sx;

  mat[2][0] = sy * cx * cz + sx * sz;
  mat[2][1] = sy * cx * sz - sx * cz;
  mat[2][2] = cy * cx;
  return mat;
}

vec3 mapping_point(vec3 vector, vec3 location, vec3 rotation, vec3 scale)
{
  return (euler_to_mat3(rotation) * (vector * scale)) + location;
}
uint hash_uint2(uint kx, uint ky)
{
  uint a, b, c;
  a = b = c = 0xdeadbeefu + (2u << 2u) + 13u;

  b += ky;
  a += kx;
  final(a, b, c);

  return c;
}
uint hash_int2(int kx, int ky)
{
  return hash_uint2(uint(kx), uint(ky));
}

float hash_uint2_to_float(uint kx, uint ky)
{
  return float(hash_uint2(kx, ky)) / float(0xFFFFFFFFu);
}
float hash_vec2_to_float(vec2 k)
{
  return hash_uint2_to_float(floatBitsToUint(k.x), floatBitsToUint(k.y));
}

float fade(float t)
{
  return t * t * t * (t * (t * 6.0 - 15.0) + 10.0);
}
float negate_if(float value, uint condition)
{
  return (condition != 0u) ? -value : value;
}

float noise_grad(uint hash, float x, float y)
{
  uint h = hash & 7u;
  float u = h < 4u ? x : y;
  float v = 2.0 * (h < 4u ? y : x);
  return negate_if(u, h & 1u) + negate_if(v, h & 2u);
}
float bi_mix(float v0, float v1, float v2, float v3, float x, float y)
{
  float x1 = 1.0 - x;
  return (1.0 - y) * (v0 * x1 + v1 * x) + y * (v2 * x1 + v3 * x);
}
float noise_perlin(vec2 vec)
{
  int X, Y;
  float fx, fy;

  FLOORFRAC(vec.x, X, fx);
  FLOORFRAC(vec.y, Y, fy);

  float u = fade(fx);
  float v = fade(fy);

  float r = bi_mix(noise_grad(hash_int2(X, Y), fx, fy),
                   noise_grad(hash_int2(X + 1, Y), fx - 1.0, fy),
                   noise_grad(hash_int2(X, Y + 1), fx, fy - 1.0),
                   noise_grad(hash_int2(X + 1, Y + 1), fx - 1.0, fy - 1.0),
                   u,
                   v);

  return r;
}

float noise_scale2(float result)
{
  return 0.6616 * result;
}

float snoise(vec2 p)
{
  float r = noise_perlin(p);
  return (isinf(r)) ? 0.0 : noise_scale2(r);
}
vec2 random_vec2_offset(float seed)
{
  return vec2(100.0 + hash_vec2_to_float(vec2(seed, 0.0)) * 100.0,
              100.0 + hash_vec2_to_float(vec2(seed, 1.0)) * 100.0);
}
float noise(vec2 p)
{
  return 0.5 * snoise(p) + 0.5;
}
float fractal_noise(vec2 p, float octaves, float roughness)
{
  float fscale = 1.0;
  float amp = 1.0;
  float maxamp = 0.0;
  float sum = 0.0;
  octaves = clamp(octaves, 0.0, 16.0);
  int n = int(octaves);
  for (int i = 0; i <= n; i++) {
    float t = noise(fscale * p);
    sum += t * amp;
    maxamp += amp;
    amp *= clamp(roughness, 0.0, 1.0);
    fscale *= 2.0;
  }
  float rmd = octaves - floor(octaves);
  if (rmd != 0.0) {
    float t = noise(fscale * p);
    float sum2 = sum + t * amp;
    sum /= maxamp;
    sum2 /= maxamp + amp;
    return (1.0 - rmd) * sum + rmd * sum2;
  }
  else {
    return sum / maxamp;
  }
}


float node_noise_texture_2d(vec3 co, float w, float scale, float detail, float roughness, float distortion)
{
  vec2 p = co.xy * scale;
  if (distortion != 0.0) {
    p += vec2(snoise(p + random_vec2_offset(0.0)) * distortion,
              snoise(p + random_vec2_offset(1.0)) * distortion);
  }
  
  return fractal_noise(p, detail, roughness);
}

vec4 mix_linear(float fac, vec4 col1, vec4 col2)
{
  fac = clamp(fac, 0.0, 1.0);

  return col1 + fac * (2.0 * (col2 - vec4(0.5)));
}

float math_compare(float a, float b, float c)
{
  return (abs(a - b) <= max(c, 1e-5)) ? 1.0 : 0.0;
}

void main() {
	vec3 vertex = v_VertexPosition * 2;

    
    vec3 map = mapping_point(vertex, vec3(0, -yPos, 0), vec3(0), vec3(1));
    float noise = node_noise_texture_2d(map,0, 1, 3, .5, 3.49);

    const float v = .34;
    float s = (noise < v + .10 ? 1 : 0) - (noise < v ? 1 : 0);

    float fResult = s * Brightness;

    color = vec4(fResult, fResult, fResult, (.86 - vertex.y) * (fResult)) * Color;
}
uniform vec2 lightPos;
uniform vec3 lightColor;
uniform float screenHeight;
uniform float brightness;

uniform sampler2D texture;

void main()
{	

	vec3 C = vec3(1, 3 , 1);

	vec2 lightPos2 = vec2(lightPos.x, screenHeight - lightPos.y);

	float L = length(lightPos2 - gl_FragCoord.xy) / 2202;

	float attenuation = 1.0 / (C.x + C.y * L + C.z * L * L);

	vec4 color2 = vec4(lightColor, 1 ) * attenuation;
	
	vec4 color = texture2D(texture,gl_TexCoord[0].st);

	if(color != vec4(0,0,0,1))
		gl_FragColor = color2;

}
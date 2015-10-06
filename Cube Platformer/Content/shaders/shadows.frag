uniform sampler2D lightTexture;
uniform sampler2D scene;

void main()
{	
	vec2 coord = gl_TexCoord[0].xy;
	vec4 lightColor = texture2D(lightTexture, coord);
	float a = (lightColor.x + lightColor.y + lightColor.z);

	vec4 color = texture2D(scene, coord);

	if(color == vec4(0,0,0,1))
		gl_FragColor = color + lightColor; //vec4(color.x * attenuation, color.y * attenuation, color.z * attenuation, 1);
	else
		gl_FragColor = vec4(color.x * a, color.y * a, color.z * a, 1);
}
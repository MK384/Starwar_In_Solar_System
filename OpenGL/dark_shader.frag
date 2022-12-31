#version 330 core
//The material is a collection of some values that we talked about in the last tutorial,
//some crucial elements to the phong model.
struct Material {
    float ambientStrength;
    float diffuseStrength;
    float specularStrength;

    float shininess; //Shininess is the power the specular light is raised to
};
//The light contains all the values from the light source, how the ambient diffuse and specular values are from the light source.
//This is technically what we were using in the last episode as we were only applying the phong model directly to the light.
struct Light {
    vec3 position;
    //vec3 direction;
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;

    float constant;
    float linear;
    float quadratic;
};
//We create the light and the material struct as uniforms.
uniform Light light;
uniform Material material;
//We still need the view position.
uniform vec3 viewPos;
uniform sampler2D texture0;

out vec4 FragColor;

in vec2 texCoord;
in vec3 Normal;
in vec3 FragPos;

void main()
{
    vec4 texColor = texture(texture0, texCoord);
    vec4 material_ambient = material.ambientStrength * texColor;
    vec4 material_diffuse = material.diffuseStrength * texColor;
    vec4 material_specular = material.specularStrength * texColor;
    
    float distance = length(light.position - FragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));
    
//    vec3 light_ambient   = light.ambient * attenuation;
//    vec3 light_diffuse   = light.diffuse * attenuation;
//    vec3 light_specular = light.specular * attenuation;
    
    //ambient
    vec4 ambient = (light.ambient, 1) * material_ambient; //Remember to use the material here.

    //diffuse 
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(light.position - FragPos);
    //vec3 lightDir = normalize( - light.direction);
    float diff = max(dot(norm, lightDir), 0.0);
    vec4 diffuse = (light.diffuse, 1)* (diff * material_diffuse); //Remember to use the material here.

    //specular
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 halfwayDir = normalize(lightDir + viewDir);
    float spec = pow(max(dot(norm, halfwayDir), 0.0), material.shininess);
    // vec3 reflectDir = reflect(-lightDir, norm);
    // float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    vec4 specular = (light.specular, 1) * (spec * material_specular); //Remember to use the material here.

    //Now the result sum has changed a bit, since we now set the objects color in each element, we now dont have to
    //multiply the light with the object here, instead we do it for each element seperatly. This allows much better control
    //over how each element is applied to different objects.
    ambient  *= attenuation;
    diffuse  *= attenuation;
    specular *= attenuation;

    vec4 result = ambient + diffuse + specular;
    FragColor = result;
    //vec3 result = vec3( ambient + diffuse + specular );
    //FragColor = vec4( result, 1 );
}
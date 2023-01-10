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
    vec3 direction;
    
    float cutOff;
    float outerCutOff;
    
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;

    float constant;
    float linear;
    float quadratic;

};



//We create the light and the material structs as uniforms.
//uniform PointLight light;
uniform Light light;
uniform Material material;
//We still need the view position.
uniform vec3 viewPos;
uniform sampler2D texture0;

out vec4 FragColor;

in vec2 texCoord;
in vec3 Normal;
in vec3 FragPos;

vec3 CalcPointLight (Light light, vec3 normal, vec3 fragPos, vec3 viewPos , Material material);
vec3 CalcDirLight   (Light light, vec3 normal, vec3 fragPos, vec3 viewPos , Material material);
vec3 CalcFlashLight (Light light, vec3 normal, vec3 fragPos, vec3 viewPos , Material material);

void main()
{
    vec3 texColor = vec3 ( texture(texture0, texCoord) );
    
    // the output is the phong model of lightening  
    vec3 phong_model = CalcPointLight(light, Normal, FragPos, viewPos, material)  * texColor;
    // applying the attenuation that is due to the distance
    FragColor = vec4 ( phong_model, 1);
}

vec3 CalcPointLight(Light light, vec3 normal, vec3 fragPos, vec3 viewPos , Material material){
    
    // dirction vectors
    vec3 lightDir = normalize(light.position - fragPos);
    vec3 viewDir = normalize(viewPos - fragPos);
    vec3 reflectDir = normalize(reflect(-lightDir, normal));

    //ambient
    vec3 ambient = material.ambientStrength * light.ambient; //Remember to use the material here.

    //diffuse
    float diff = max(dot(normal, lightDir), 0.0);
    vec3 diffuse =  (diff * material.diffuseStrength) * light.diffuse ; //Remember to use the material here.

    //specular
    float spec = pow( max(dot(viewDir, reflectDir) , 0.0) , material.shininess);
    vec3 specular = (spec * material.specularStrength) * light.specular; //Remember to use the material here.

    // attenuation factor due to distance of the light source from the object
    float distance = length(light.position - fragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));
    
    return (ambient + diffuse + specular) *  attenuation;
    
}

vec3 CalcDirLight(Light light, vec3 normal, vec3 fragPos, vec3 viewPos , Material material){
    //TODO: TEST THE FUNCTION
    // dirction vectors
    vec3 viewDir = normalize(viewPos - fragPos);
    vec3 reflectDir = normalize(reflect(-light.direction, normal));

    //ambient
    vec3 ambient = material.ambientStrength * light.ambient; //Remember to use the material here.

    //diffuse
    float diff = max(dot(normal, light.direction), 0.0);
    vec3 diffuse =  (diff * material.diffuseStrength) * light.diffuse ; //Remember to use the material here.

    //specular
    float spec = pow( max(dot(viewDir, reflectDir) , 0.0) , material.shininess);
    vec3 specular = (spec * material.specularStrength) * light.specular; //Remember to use the material here.
    
    return (ambient + diffuse + specular);
}

vec3 CalcFlashLight(Light light, vec3 normal, vec3 fragPos, vec3 viewPos , Material material){
    
    // dirction vectors
    vec3 lightDir = normalize(light.position - fragPos);
    vec3 viewDir = normalize(viewPos - fragPos);
    vec3 reflectDir = normalize(reflect(-lightDir, normal));

    //ambient
    vec3 ambient = material.ambientStrength * light.ambient; //Remember to use the material here.

    //diffuse
    float diff = max(dot(normal, lightDir), 0.0);
    vec3 diffuse =  (diff * material.diffuseStrength) * light.diffuse ; //Remember to use the material here.

    //specular
    float spec = pow( max(dot(viewDir, reflectDir) , 0.0) , material.shininess);
    vec3 specular = (spec * material.specularStrength) * light.specular; //Remember to use the material here.

    // attenuation factor due to distance of the light source from the object
    float distance = length(light.position - fragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));

    
    float theta     = dot(lightDir, normalize(-light.direction));
    float epsilon   = light.cutOff - light.outerCutOff;
    float intensity = clamp((theta - light.outerCutOff) / epsilon, 0.0, 1.0);
    
    diffuse *= intensity;
    specular*= intensity;

    return (ambient + diffuse + specular) *  attenuation;
    
}
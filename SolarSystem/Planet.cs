using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.IO;

namespace ComputerGraphics.GraphObjects
{
    public class Planet : GraphObject
    {
        
        public enum Planets
        {
            Mercury,
            Venus,
            Earth,
            Mars,
            Jupiter,
            Saturn,
            Uranus,
            Neptune
        }

        public Planets planetName;
        public float _rotaionSpeed { get; set; }
        public float _orbitSpeed { get; set; } 
        
        public List<Moon> _moons { get; set; }
        public Vector4 _material { get; set; }
        
        public Planet( Planets name,float raduis, Vector3 position, String texture ): base(position , raduis , false)
        {
            planetName = name;
            setTexture(texture);
            _moons = new List<Moon>();
        }
        
        public override void OnRenderFrame(Shader shader , float time)
        {
      
            
            GL.BindVertexArray(VertexArrayObject);
             shader.Use(); // Calling the shader -> Gl.useProgram(Handler);
            _texture.Use();

            var trans = _worldReferencePoint;
            
            model = Matrix4.Identity;
            model *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(time * _rotaionSpeed * 50.0f));
            model *= Matrix4.CreateScale(_scale);
            trans.X = -trans.Z * (float)Math.Cos(MathHelper.DegreesToRadians(time * _orbitSpeed));
            trans.Z = -trans.Z * (float)Math.Sin(MathHelper.DegreesToRadians(time * _orbitSpeed));
            model *= Matrix4.CreateTranslation(trans);

            shader.SetMatrix4("model",  model );
            
            shader.SetFloat("material.ambientStrength", _material.X);
            shader.SetFloat("material.diffuseStrength", _material.Y);
            shader.SetFloat("material.specularStrength", _material.Z);
            shader.SetFloat("material.shininess", _material.W);
            
            GL.DrawArrays(PrimitiveType.Triangles, 0, _vertices.Length/8);

            foreach (var moon in _moons)
            {
                moon.OnRenderFrame(shader , time);
            }

        }
        
        protected override void ImportStandardShapeData()
        {
            
            _vertices = new SphereFactory().GetVertices();
            
        }
    }
}


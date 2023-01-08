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
    
    
    public class Moon : GraphObject
    {
        public Stopwatch _timer { set; get; }
        
        public float _rotaionSpeed { get; set; }
        public float _orbitSpeed { get; set; }
        public Planet _planet;
        public Vector4 _material { get; set; }
        

        public Moon(float radius , Vector3 position , string texture):base(position,radius,false )
        {
            setTexture(texture);
        }
        public override void OnRenderFrame(Shader shader , float time)
        {
            //TODO: Fix the moon orbit rotation

            GL.BindVertexArray(VertexArrayObject);
            shader.Use();
            _texture.Use();

            var trans = _worldReferencePoint;
            
            var model = Matrix4.Identity;
            model *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(time * _rotaionSpeed * 50.0f));
            model *= Matrix4.CreateScale(_scale);
            trans.X = (-trans.Z * (float)Math.Cos(MathHelper.DegreesToRadians(-time * _orbitSpeed)) + _planet._worldReferencePoint.X);
            trans.Z = (-trans.Z * (float)Math.Sin(MathHelper.DegreesToRadians(-time * _orbitSpeed)) + _planet._worldReferencePoint.Z);
            model *= Matrix4.CreateTranslation(trans);

            shader.SetMatrix4("model", model);
            
            shader.SetFloat("material.ambientStrength", _material.X);
            shader.SetFloat("material.diffuseStrength", _material.Y);
            shader.SetFloat("material.specularStrength", _material.Z);
            shader.SetFloat("material.shininess", _material.W);

            GL.DrawArrays(PrimitiveType.Triangles, 0, _vertices.Length/8);
            
        }
        
        protected override void ImportStandardShapeData()
        {
            _vertices = new SphereFactory().GetVertices();
            base.ImportStandardShapeData();
        }
    }
}


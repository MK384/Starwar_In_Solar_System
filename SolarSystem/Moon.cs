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
        public float _rotaionSpeed { get; set; }
        public float _orbitSpeed { get; set; }
        public Planet _planet;
        public Vector4 _material { get; set; }
        

        public Moon(float radius , Vector3 position , string texture):base(position,radius,false )
        {
            setTexture(texture);
            OnLoadObject();
        }
        public override void OnRenderFrame(Shader shader , float time)
        {
            
            GL.BindVertexArray(VertexArrayObject);
            shader.Use();
            _texture.Use();
            
            UpdateModel(time);
            shader.SetMatrix4("model", model);
            shader.SetMaterial(_material);
            
            GL.DrawArrays(PrimitiveType.Triangles, 0, _vertices.Length/8);
            
        }

        protected override void UpdateModel(float time)
        {
            var trans = _worldReferencePoint;
            var planetTrans = _planet.model.ExtractTranslation();
            
            model = Matrix4.Identity;
            model *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(time * _rotaionSpeed * 50.0f));
            model *= Matrix4.CreateScale(_scale);
            trans.X = (-trans.Z * (float)Math.Cos(MathHelper.DegreesToRadians(-time * _orbitSpeed)) + planetTrans.X);
            trans.Z = (-trans.Z * (float)Math.Sin(MathHelper.DegreesToRadians(-time * _orbitSpeed)) + planetTrans.Z);
            model *= Matrix4.CreateTranslation(trans);
        }

        protected override void ImportStandardShapeData()
        {
            _vertices = new SphereFactory().GetVertices();

        }
    }
}


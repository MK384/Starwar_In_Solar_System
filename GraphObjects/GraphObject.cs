/********************************************************************************************
 * Copyright (c) Computer Graphics Course by Fayoum University 
 * Prof. Amr M. Gody, amg00@fayoum.edu.eg
 * License: free for use and distribution for Educational purposes. It is required to keep this header comments on your code. 
 * Purpose:             Parent Graph Object. This object has been developed for providing all basic functionalities for graph object. This object should be used as parent of any graph object. 
 *
 * Ver  Date         By     Purpose
 * ---  ----------- -----   --------------------------------------------------------------------
 * 01   2020-12-05  AMG     Created the initial version.
 *************************************************************************************************/

using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using OpenGL.Constructs;


using System.IO;
using OpenGL;
using Boolean = System.Boolean;
using BufferTarget = OpenTK.Graphics.OpenGL.BufferTarget;
using BufferUsageHint = OpenTK.Graphics.OpenGL.BufferUsageHint;
using Matrix4 = OpenTK.Mathematics.Matrix4;
using Vector3 = OpenTK.Mathematics.Vector3;
using Vector2 = OpenTK.Mathematics.Vector2;
using VertexAttribPointerType = OpenTK.Graphics.OpenGL.VertexAttribPointerType;

namespace ComputerGraphics.GraphObjects
{
    public class GraphObject 
    {
        
        public List<Vector3> LocalVertices { get; set; }
        public List<Vector2> VerticesTexture { get; set; }
        public int VertexArrayObject { get; set; }
        public int VertexBufferObject { get;  set; }
        public int ElementBufferObject { get; set; }


        public Texture _texture;
        public Vector3 _worldReferencePoint;
        
        protected float[] _vertices;
        protected uint[] _indices;

        public Matrix4 model;
        
        protected bool _useElements;

        public bool isLumin;

        public float _scale;
 

 

        public GraphObject()
        {
            LocalVertices = new List<Vector3>();
            VerticesTexture = new List<Vector2>();

            _scale = 1.0f;
            _worldReferencePoint = Vector3.Zero;

            isLumin = true;
            
            ImportStandardShapeData();

        }
        public GraphObject(Vector3 refpoint,float scale , bool isLumin)
        {
            LocalVertices = new List<Vector3>();
            VerticesTexture = new List<Vector2>();

            _worldReferencePoint = refpoint;
            
            this.isLumin = isLumin;
            _scale = scale;
            
            ImportStandardShapeData();

        }
        protected virtual void UpdateModel(float arg)
        {
            
            model   = Matrix4.CreateScale(_scale);
            model  *= Matrix4.CreateTranslation(_worldReferencePoint) ;
            
        }

        public void setTexture(string path)
        {
            _texture = new Texture(path);
        }
        

        
        public void OnLoadObject()
        {

            //1- Identify Vertex array Object of this Graph object
            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);
            
            //2- Load Vertex Array Buffer of this Object
            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);
            
            
            //    Attib0          Attib1        Attib2
            //    Vertex          Normal       Texture
            // (Vx , Vy, Vz) , (Nx , Ny, Nz) , (u , v)
            
            //set attribute 0 for vertecies
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            
            // set attribute 2 for normal 
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));


            //set attribute 1 for texture
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));

            
        }
        public virtual void OnRenderFrame(Shader shader , float times)
        {
            
            GL.BindVertexArray(VertexArrayObject);
            shader.Use(); // Calling the shader -> Gl.useProgram(Handler);
            _texture.Use();
            
            shader.SetMatrix4("model",  model );
            
            
            
        }
       public virtual void OnUnload()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            

            // Delete all the resources.
            GL.DeleteBuffer(VertexBufferObject);
            GL.DeleteVertexArray(VertexArrayObject);
      
        }
       protected virtual void ImportStandardShapeData()
        {
            
            var buffer = new List<float>();
             for(var i=0; i < LocalVertices.Count; i++)
             {
                 
                 buffer.Add(LocalVertices[i].X);
                 buffer.Add(LocalVertices[i].Y);
                 buffer.Add(LocalVertices[i].Z);
                 
                 buffer.Add(VerticesTexture[i].X);
                 buffer.Add(VerticesTexture[i].Y);
             }
             _vertices = buffer.ToArray();
        }
       
       
       protected virtual bool ConfigureElementsBuffer()
       {

           if (_indices != null && _indices.Length > 0)
           {
               ElementBufferObject = GL.GenBuffer();
               GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
               GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices,
                   BufferUsageHint.StaticDraw);
        
               return true;
           }
           return false;
       }

    }
}

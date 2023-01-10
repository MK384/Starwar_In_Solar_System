/********************************************************************************************
 * Copyright (c) Computer Graphics Course by Fayoum University 
 * Prof. Amr M. Gody, amg00@fayoum.edu.eg
 * License: free for use and distribution for Educational purposes. It is required to keep this header comments on your code. 
 * Purpose:             Public methods and Properties have been developed here
 *
 * Ver  Date         By     Purpose
 * ---  ----------- -----   --------------------------------------------------------------------
 * 01   2020-12-05  AMG     Created the initial version.
 *************************************************************************************************/

using ComputerGraphics.GraphObjects;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Windows.Media.Media3D;


namespace ComputerGraphics
{
    public partial class OpenGLWindow : GameWindow
    {
        
        #region Properties
        protected Shader luminObjShader;
        protected Shader darkObjShader;
        protected List<GraphObject> _graphObjects = new List<GraphObject>();
        
        protected Camera _camera;
        protected Stopwatch timer;
        
        #endregion

        #region Methods
        public OpenGLWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings )
            : base(gameWindowSettings, nativeWindowSettings)
        {
            // Construct the camera and set the view Matrix
            _camera = new Camera(new Vector3(0.0f, 1.0f, 0.5f) * 320 , Size.X / (float)Size.Y);
            timer = new Stopwatch();
            
            // Used to initialize and enables navigation functionality.
            LoadNavigationFunctions();
        }
        public void AddGraph(GraphObject obj)
        {
            _graphObjects.Add(obj);
        } 
        
        
        /*
         * Called on unloading the scene.
         */
        protected override void OnUnload()
        {
            base.OnUnload();
            // let every object to cleared its data
            foreach (var obj in _graphObjects)
            {
                obj.OnUnload();
            }
            GL.UseProgram(0);
            GL.DeleteProgram(darkObjShader.Handle);
            GL.DeleteProgram(luminObjShader.Handle);
        }
        protected override void OnClosed()
        {
            
            OnUnload();
            base.OnClosed();
            
        }
        /*
         * This method is called when the GL Window starting for the first time.
         * It's suitable to set the background color within it and initialize the shader.
         */
        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f); // Set the background color
            // Enable depth testing so that pixels doesn't overlap 
            GL.Enable(EnableCap.DepthTest);
            
            // Initialize the shaders for dark and luminous bodies
            luminObjShader = new Shader("OpenGL\\vert_shader.vert", "OpenGL\\lumin_shader.frag"); 
            darkObjShader = new Shader("OpenGL\\vert_shader.vert", "OpenGL\\dark_shader.frag");
            
            // set Shaders to use with openGL
            luminObjShader.Use();
            darkObjShader.Use();
            
            // start the timer used for rotation speed 
            timer.Start();
            
            GL.Viewport(0, 0, Size.X, Size.Y);
            // Every object has to Onload itself where it links the Vertex array object and vertex buffer object.
            foreach (var obj in _graphObjects)
            {
                obj.OnLoadObject();
            }


        }
        /*
         * This method is called each time the user resize the window.
         */
        protected override void OnResize(ResizeEventArgs e)
        {
            
            base.OnResize(e);
            // get the new height and width of the window
            GL.Viewport(0, 0, Size.X, Size.Y); // edit the view port to the new values.
            
            // calculate the Aspect Ratio. and set the projection Matrix
            _camera.AspectRatio = Size.X/(float) Size.Y;
            
        }
        /*
         * Called repeatedly by number of the frameRate in the second.
         * It's suitable to draw all the shape in the scene here.
         */
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            // first clear the window with the defined color
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            var timeValue = (float)timer.Elapsed.TotalSeconds;
            // configure the shadowing and illumine
            ConfigurateShadowing(darkObjShader, _camera);
             // let all object to draw itself
             foreach (var obj in _graphObjects)
             {
                 if (obj.isLumin)
                 {
                     
                     luminObjShader.SetMatrix4("view",_camera.GetViewMatrix());
                     luminObjShader.SetMatrix4("projection",_camera.GetProjectionMatrix());
                     obj.OnRenderFrame(luminObjShader , timeValue);
                 }
                 else
                 {
                    
                     
                     darkObjShader.SetMatrix4("view",_camera.GetViewMatrix());
                     darkObjShader.SetMatrix4("projection",_camera.GetProjectionMatrix());
                     obj.OnRenderFrame(darkObjShader, timeValue);
                 }
                 
                
             }
            GL.Flush(); // forcing the engine to execute 
            //Swaps the front and back buffers of the current GraphicsContext, presenting the rendered scene to the user.
            Context.SwapBuffers(); 
            base.OnRenderFrame(args);


        }
        
        private void ConfigurateShadowing(Shader objShader , Camera camera)
        {
            
            objShader.Use();
            objShader.SetVector3("viewPos", camera.Position);

            Vector3 ambientColor = new Vector3(1.0f);
            Vector3 diffuseColor = new Vector3(1.0f);
            Vector3 specularColor = new Vector3(1.0f);
            
            objShader.SetVector3("light.position", new Vector3(0.0f, 0.0f, 0.0f));
            
            // for testing the FlashLight
            // objShader.SetVector3("light.position", _camera.Position);
            // objShader.SetVector3("light.direction",_camera.Front);
            // objShader.SetFloat("light.cutOff", (float)Math.Cos(MathHelper.DegreesToRadians(12.5)));
            // objShader.SetFloat("light.outerCutOff", (float)Math.Cos(MathHelper.DegreesToRadians(17.5)));
            
            // light strength
            objShader.SetVector3("light.ambient", ambientColor);
            objShader.SetVector3("light.diffuse", diffuseColor);
            objShader.SetVector3("light.specular", specularColor);
            
            // light attenuation
            objShader.SetFloat("light.constant", 1.0f);
            objShader.SetFloat("light.linear", 0.000002f);
            objShader.SetFloat("light.quadratic", 0.0000001f);

        }
        #endregion
    }
}

/********************************************************************************************
 * Copyright (c) Computer Graphics Course by Fayoum University 
 * Prof. Amr M. Gody, amg00@fayoum.edu.eg
 * License: free for use and distribution for Educational purposes. It is required to keep this header comments on your code. 
 * Purpose:             Navigation functions have been developed here
 *
 * Ver  Date         By     Purpose
 * ---  ----------- -----   --------------------------------------------------------------------
 * 01   2020-12-05  AMG     Created the initial version.
 *************************************************************************************************/

using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Windows.Input;
using ComputerGraphics.GraphObjects;
using Boolean = System.Boolean;
using MouseWheelEventArgs = OpenTK.Windowing.Common.MouseWheelEventArgs;


namespace ComputerGraphics
{
    public partial class OpenGLWindow : GameWindow
    {
     

        protected float cameraSpeed = 100f;
        protected float zoomFactor = 3.0f;
        protected float sensitivity = 0.2f;
        
        
        
        delegate  void NavigationFunction(FrameEventArgs args);
        enum Navigations
        {
            Forward,
            Backward,
            Left,
            Right,
            Up,
            Down,
            AimRight,
            AimLift,
            Reload,
            None
        }
        Dictionary<Navigations, NavigationFunction> _navigationFunction = new Dictionary<Navigations, NavigationFunction>();
        private void LoadNavigationFunctions()
        {
            _navigationFunction.Add(Navigations.Forward, MoveForward);
            _navigationFunction.Add(Navigations.Backward, MoveBackward);
            _navigationFunction.Add(Navigations.Left, MoveLeft);
            _navigationFunction.Add(Navigations.Right, MoveRight);
            _navigationFunction.Add(Navigations.Up, MoveUp);
            _navigationFunction.Add(Navigations.Down, MoveDown);
            
            _navigationFunction.Add(Navigations.AimRight, AimRight);
            _navigationFunction.Add(Navigations.AimLift, AimLeft);
            _navigationFunction.Add(Navigations.Reload, Reload);
            
            _navigationFunction.Add(Navigations.None, NoNavigation);

            
            CursorVisible = false;
            CursorGrabbed = true;
        }
        private Navigations GetNavigation()
        {
            return  KeyboardState.IsKeyDown(Keys.W) ? Navigations.Forward :
                    KeyboardState.IsKeyDown(Keys.S) ? Navigations.Backward :
                    KeyboardState.IsKeyDown(Keys.A) ? Navigations.Left :
                    KeyboardState.IsKeyDown(Keys.D) ? Navigations.Right :
                    KeyboardState.IsKeyDown(Keys.Space) ? Navigations.Up :
                    KeyboardState.IsKeyDown(Keys.LeftShift) ? Navigations.Down :
                    KeyboardState.IsKeyDown(Keys.E) ? Navigations.AimRight :
                    KeyboardState.IsKeyDown(Keys.Q) ? Navigations.AimLift :
                    KeyboardState.IsKeyDown(Keys.R) ? Navigations.Reload :
                     Navigations.None;
        }
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            
            if (KeyboardState.IsKeyDown(Keys.Escape))
                Close();
            else
                _navigationFunction[GetNavigation()](args);
            
            _camera.Yaw += MouseState.Delta.X * sensitivity;
            _camera.Pitch -= MouseState.Delta.Y * sensitivity;
            
        }
        
        /***********************************************************
         *             Navigations Functions Goes Here             *
         ***********************************************************/
        public virtual void MoveUp(FrameEventArgs args)
        {
            _camera.Position += _camera.Up * cameraSpeed * (float)args.Time; //Up 
         
        }
        public virtual void MoveDown(FrameEventArgs args)
        {
            _camera.Position -= _camera.Up * cameraSpeed * (float)args.Time; //Down
        
        }
        public virtual void MoveBackward(FrameEventArgs args)
        {
            
            _camera.Position -= _camera.Front * cameraSpeed * (float)args.Time; 
    
        }

        
        public virtual void MoveForward(FrameEventArgs args)
        {
            _camera.Position += _camera.Front * cameraSpeed * (float)args.Time; 
            
        }
        
        public virtual void MoveRight(FrameEventArgs args)
        {
            _camera.Position += Vector3.Normalize(Vector3.Cross(_camera.Front, _camera.Up)) * cameraSpeed * (float)args.Time;
        
        }

        public virtual void MoveLeft(FrameEventArgs args)
        {
            _camera.Position -= Vector3.Normalize(Vector3.Cross(_camera.Front, _camera.Up)) * cameraSpeed * (float)args.Time;
         
        }
        
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);
            if (IsFocused) // check to see if the window is focused  
            {
                // MousePosition = new Vector2( e.X + Size.X/2f, e.Y + Size.Y/2f);
            }
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            _camera.Fov -= MouseState.ScrollDelta.Y * zoomFactor ;
        }
        
        public virtual void AimRight(FrameEventArgs args)
        {
            
        }
        public virtual void AimLeft(FrameEventArgs args)
        {
            
        }
        public virtual void Reload(FrameEventArgs args)
        {
            
        }

        private void NoNavigation(FrameEventArgs args)
        {
            
        }

        private Boolean canCameraMove()
        {
            Boolean decision = true;

            // _graphObjects
            foreach (var obj in _graphObjects)
            {
                
            }
            return decision;
        }
    }
}

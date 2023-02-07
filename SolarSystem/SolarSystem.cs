using System.Collections.Generic;
using System.Diagnostics;
using ComputerGraphics.GraphObjects;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace ComputerGraphics
{
    public class SolarSystem : OpenGLWindow
    {
         private int _right = 0, _left = 0, _top = 0, _down = 0;
         
         public SpaceCraft spaceCraft{set; get;}
         
  
         
         
    
    
        private Vector4[] _planetspositions =
            {
            
                new Vector4(0.0f, 0.0f, -190.0f, (1.0f / 59.0f))
                ,
                new Vector4(0.0f, 0.0f, -361.5f, (1.0f / 241)),
    
                new Vector4(0.0f, 0.0f, -500.0f, 1.0f),
    
                new Vector4( 0.0f, 0.0f, -762.0f, (24.0f / 25)),
    
                new Vector4(0.0f, 0.0f, -2601.5f, (24.0f/9)),
    
                new Vector4(0.0f, 0.0f, -4769.0f, (24.0f / 11)),
    
                new Vector4(0.0f, 0.0f, -9590.0f, (24.0f / 18)),
    
                new Vector4(0.0f, 0.0f, -15000.0f, (24.0f / 19.0f))
    
            }; 
            
            private Vector4[] _planetMaterials =
            {
                new Vector4(0.5f, 0.5f, .2f, 2.0f),
                new Vector4(0.5f, 0.5f, .25f, 2.0f),
                new Vector4(0.5f, 0.5f, .7f, 2.0f),
                new Vector4(0.25f, 0.25f, .25f, 2.0f),
                new Vector4(0.25f, 0.25f, .25f, 2.0f),
                new Vector4(0.25f, 0.25f, .25f, 2.0f),
                new Vector4(0.25f, 0.25f, .25f, 2.0f),
                new Vector4(0.25f, 0.25f, .25f, 2.0f),
            };
            
            
            
    
        public SolarSystem(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            timer = new Stopwatch();
            spaceCraft = new SpaceCraft(ref _camera, new Vector4(5.0f, 5.0f, 2.0f, 2.0f) ,new Vector4(0.5f, 0.5f, 1.0f, 32.0f) );
            
            AddGraph(spaceCraft);
            AddGraph(new Sun(109.0f));
            AddGraph(new Planet(Planet.Planets.Mercury, 3.8f, _planetspositions[0].Xyz, "resources\\Mercury.jpg")
            {
            
             
                _material = _planetMaterials[0],
                _rotaionSpeed = _planetspositions[0].W,
                _orbitSpeed = _planetspositions[0].Z/(88f)
            
            });
            AddGraph(new Planet(Planet.Planets.Venus, 9.5f,_planetspositions[1].Xyz, "resources\\Venus.jpg")
            {
            
         
                _material = _planetMaterials[1],
                _rotaionSpeed = _planetspositions[1].W,
                _orbitSpeed = _planetspositions[1].Z/(225f)
            
            });
            Planet earth = new Planet(Planet.Planets.Earth, 10f, _planetspositions[2].Xyz, "resources\\Earth.jpg")
            {

                _material = _planetMaterials[2],
                _rotaionSpeed = _planetspositions[2].W,
                _orbitSpeed = _planetspositions[2].Z / (365f),


            };
            
            Moon earthmoon = new Moon(2.7f, new Vector3(0.0f, 0.0f, -25.0f), "resources\\Moon1.jpg")
            {
                _planet = earth,
                _material = new Vector4(0.25f, 0.25f, .25f, 2.0f),
                _rotaionSpeed = (1.0f / 30.0f),
                _orbitSpeed = 30f

            };
            AddGraph(earth);
            AddGraph(new Planet(Planet.Planets.Mars, 5.3f, _planetspositions[3].Xyz, "resources\\Mars.jpg")
            {
            
           
                _material = _planetMaterials[3],
                _rotaionSpeed = _planetspositions[3].W,
                _orbitSpeed = _planetspositions[3].Z/(697f)
            
            });            
            AddGraph(new Planet(Planet.Planets.Jupiter, 85f, _planetspositions[4].Xyz, "resources\\Jupiter.jpg")
            {
            
           
                _material = _planetMaterials[4],
                _rotaionSpeed = _planetspositions[4].W,
                _orbitSpeed = _planetspositions[4].Z/(4880f)
            
            });        
            AddGraph(new Planet(Planet.Planets.Saturn, 94f, _planetspositions[5].Xyz, "resources\\i8k_saturn.jpg")
            {
            
     
                _material = _planetMaterials[5],
                _rotaionSpeed = _planetspositions[5].W,
                _orbitSpeed = _planetspositions[5].Z/(10950f)
            
            }); 
            AddGraph(new Planet(Planet.Planets.Uranus, 40f, _planetspositions[6].Xyz, "resources\\Uranus.jpg")
            {
            
         
                _material = _planetMaterials[6],
                _rotaionSpeed = _planetspositions[6].W,
                _orbitSpeed = _planetspositions[6].Z/(30660f)
            
            });            
            AddGraph(new Planet(Planet.Planets.Neptune, 38f, _planetspositions[7].Xyz, "resources\\Neptune.jpg")
            {
            
  
                _material = _planetMaterials[7],
                _rotaionSpeed = _planetspositions[7].W,
                _orbitSpeed = _planetspositions[7].Z/(60225f)
            
            });                   
        }

        public override void MoveForward (FrameEventArgs e)
        {
           if (_top + _down > 0)
           {
               _camera.Position += _camera.Up * cameraSpeed * (float)e.Time;
           }
           else if (_top + _down < 0)
           {
               _camera.Position -= _camera.Up * cameraSpeed * (float)e.Time;
           }
           if (_left + _right > 0)
           {
               _camera.Position -= _camera.Right * cameraSpeed * (float)e.Time;
           }
           else if (_left + _right < 0)
           {
               _camera.Position += _camera.Right * cameraSpeed * (float)e.Time;
           }
           _camera.Position += _camera.Front * cameraSpeed * (float)e.Time; // Forward
        }

        public override void MoveBackward(FrameEventArgs e)
        {
             _camera.Position -= _camera.Front * cameraSpeed * (float)e.Time;
        }

        public override void MoveLeft(FrameEventArgs e)
        {
                if (_left >= 0 && _right >= 0)
                {
                    _left += _left == 45? 0 : 1;
                    spaceCraft.model = Matrix4.Identity;
                    spaceCraft.model *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(_left));
                    spaceCraft.model *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(_top + _down));
                    spaceCraft.model *= Matrix4.CreateScale(0.25f) * Matrix4.CreateTranslation(new Vector3(0.0f, -.5f, -0.7f));
                    _camera.Position -= _camera.Right * cameraSpeed * (float)e.Time; // Left
                    if (_top + _down > 0)
                    {
                        _camera.Position += _camera.Up * cameraSpeed * (float)e.Time;
                    }
                    else if (_top + _down < 0) 
                    {
                        _camera.Position -=  _camera.Up * cameraSpeed * (float)e.Time;
                    }
                }
                else
                {
                    _right++;
                    spaceCraft.model = Matrix4.Identity;
                    spaceCraft.model *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(_left + _right));
                    spaceCraft.model *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(_top + _down));
                    spaceCraft.model *= Matrix4.CreateScale(0.25f) * Matrix4.CreateTranslation(new Vector3(0.0f, -.5f, -0.7f));
                }
        }

        public override void MoveRight(FrameEventArgs e)
        {
            
                if (_right <= 0 && _left <= 0)
                {
                    _right -= _right == -45? 0 : 1;
                    spaceCraft.model = Matrix4.Identity;
                    spaceCraft.model *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(_right));
                    spaceCraft.model *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(_top + _down));
                    spaceCraft.model *= Matrix4.CreateScale(0.25f) * Matrix4.CreateTranslation(new Vector3(0.0f, -.5f, -0.7f));
                    _camera.Position += _camera.Right * cameraSpeed * (float)e.Time; // Right
                    if (_top + _down > 0)
                    {
                        _camera.Position += _camera.Up * cameraSpeed * (float)e.Time;
                    }
                    else if (_top + _down < 0)
                    {
                        _camera.Position -= _camera.Up * cameraSpeed * (float)e.Time;
                    }
                }
                else
                {
                    _left--;
                    spaceCraft.model = Matrix4.Identity;
                    spaceCraft.model *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(_left + _right));
                    spaceCraft.model *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(_top + _down));
                    spaceCraft.model *= Matrix4.CreateScale(0.25f) * Matrix4.CreateTranslation(new Vector3(0.0f, -.5f, -0.7f));
                }
        }

        public override void MoveUp(FrameEventArgs e)
        {
            
                if (_top >= 0 && _down >= 0)
                {
                    _top += _top == 45? 0 : 1;
                      spaceCraft.model = Matrix4.Identity;
                      spaceCraft.model *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(_left + _right));
                      spaceCraft.model *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(_top));
                      spaceCraft.model *= Matrix4.CreateScale(0.25f) * Matrix4.CreateTranslation(new Vector3(0.0f, -.5f, -0.7f));
                    _camera.Position += _camera.Up * cameraSpeed * (float)e.Time; // Up
                    if (_left + _right > 0)
                    {
                        _camera.Position -= _camera.Right * cameraSpeed * (float)e.Time;
                    }
                    else if (_left + _right < 0)
                    {
                        _camera.Position += _camera.Right * cameraSpeed * (float)e.Time;
                    }
                }
                else
                {
                    _down++;
                      spaceCraft.model = Matrix4.Identity;
                      spaceCraft.model *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(_left + _right));
                      spaceCraft.model *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(_top + _down));
                      spaceCraft.model *= Matrix4.CreateScale(0.25f) * Matrix4.CreateTranslation(new Vector3(0.0f, -.5f, -0.7f));
                }
        }

        public override void MoveDown(FrameEventArgs e)
        {
           
                if (_down <= 0 && _top <= 0)
                {
                    _down -= _down == -45 ? 0 : 1;
                     spaceCraft.model = Matrix4.Identity;
                     spaceCraft.model *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(_left + _right));
                     spaceCraft.model *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(_down));
                     spaceCraft.model *= Matrix4.CreateScale(0.25f) * Matrix4.CreateTranslation(new Vector3(0.0f, -.5f, -0.7f));
                    _camera.Position -= _camera.Up * cameraSpeed * (float)e.Time; // down
                    if (_left + _right > 0)
                    {
                        _camera.Position -= _camera.Right * cameraSpeed * (float)e.Time;
                    }
                    else if (_left + _right < 0)
                    {
                        _camera.Position += _camera.Right * cameraSpeed * (float)e.Time;
                    }
                }
                else
                {
                    _top--;
                     spaceCraft.model = Matrix4.Identity;
                     spaceCraft.model *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(_left + _right));
                     spaceCraft.model *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(_top + _down));
                     spaceCraft.model *= Matrix4.CreateScale(0.25f) * Matrix4.CreateTranslation(new Vector3(0.0f, -.5f, -0.7f));
                }
        }

        public override void AimRight(FrameEventArgs args)
        {
           cameraSpeed += cameraSpeed > 70? 0 :  5f;
        }
        public override void AimLeft(FrameEventArgs args)
        {
           cameraSpeed -= cameraSpeed < 10f ? 0 : 5f;
        }
        public override void Reload(FrameEventArgs args)
        {
             cameraSpeed = 25.0f;
        }
    }
}
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
    public class SpaceCraft : GraphObject
    {
     private Texture spaceTexture;
     private Texture bodyTexture;
     private Texture wingTexture;

     private Vector4 shipMaterial;
     private Vector4 spaceMaterial;

     private  Camera _camera;
     
     public SpaceCraft(ref Camera camera, Vector4 spaceMaterial ,Vector4 shipMaterial ) : base(new Vector3(0.0f, -.5f, 0.7f),0.25f, false)
     { 

      this.shipMaterial = shipMaterial;
      this.spaceMaterial = spaceMaterial;

      _camera = camera;

      spaceTexture = new Texture("resources\\8k_stars.jpg");
      bodyTexture = new Texture("resources\\SpaceShip.jpg");
      wingTexture = new Texture("resources\\solarPanel.jpg");
      
     }

     public override void OnRenderFrame(Shader shader , float time)
     {
      GL.BindVertexArray(VertexArrayObject);
      
      shader.Use();
      
      //body
      bodyTexture.Use();
      
      shader.SetMatrix4("view", Matrix4.Identity);
      shader.SetMatrix4("model", model);
      shader.SetMatrix4("projection", _camera.GetProjectionMatrix());
      
      shader.SetFloat("material.ambientStrength", shipMaterial.X);
      shader.SetFloat("material.diffuseStrength", shipMaterial.Y);
      shader.SetFloat("material.specularStrength", shipMaterial.Z);
      shader.SetFloat("material.shininess", shipMaterial.W);
      GL.DrawArrays(PrimitiveType.Triangles, 0, (_vertices.Length / 8) - 48);
      
      // wings
      wingTexture.Use();
      
      GL.DrawArrays(PrimitiveType.Triangles, (_vertices.Length / 8) - 48, 12);

      // space
      shader.SetFloat("material.ambientStrength", spaceMaterial.X);
      shader.SetFloat("material.diffuseStrength", spaceMaterial.Y);
      shader.SetFloat("material.specularStrength", spaceMaterial.Z);
      shader.SetFloat("material.shininess", spaceMaterial.W);
      shader.SetMatrix4("model", Matrix4.CreateScale(18000.0f));
      
      spaceTexture.Use();
      GL.DrawArrays(PrimitiveType.Triangles, (_vertices.Length / 8) - 36 , 36);
      
     }

     protected override void ImportStandardShapeData()
        {
            _vertices = new []
             {

            //0.5, 0.5, 0.5     -0.5, 0.5, 0.5   0.5, 0.0, 0.5     -0.5, 0.0, 0.5f
            0.5f, 0.5f, 0.5f,   5f, 5f, 5f,   1.0f, 1.0f,             //
            -0.5f, 0.5f, 0.5f,  5f, 5f, 5f,   0.0f, 1.0f,             //    IT IS THE BODY
            0.5f, 0.0f, 0.5f,   5f, 5f, 5f,   1.0f, 0.0f,             //    
            -0.5f, 0.5f, 0.5f,  5f, 5f, 5f,   0.0f, 1.0f,             //     BACK FACE
            0.5f, 0.0f, 0.5f,   5f, 5f, 5f,   1.0f, 0.0f,             //
            -0.5f, 0.0f, 0.5f,  5f, 5f, 5f,   0.0f, 0.0f,             //

            //  0.5f, 0.5f, 0.5f   -0.5f, 0.5f, 0.5f    0.5f, 0.5f, -0.5f       -0.5f, 0.5f, -0.5f
             0.5f, 0.5f, 0.5f,  5f, 5f, 5f,   1.0f, 1.0f,              //
            -0.5f, 0.5f, 0.5f,  5f, 5f, 5f,   0.0f, 1.0f,              //    IT IS THE BODY 
            0.5f, 0.5f, -0.5f,  5f, 5f, 5f,   1.0f, 0.0f,              //  
            0.5f, 0.5f, -0.5f,  5f, 5f, 5f,   1.0f, 0.0f,              //     TOP FACE
            -0.5f, 0.5f, 0.5f,  5f, 5f, 5f,   0.0f, 1.0f,              //
            -0.5f, 0.5f, -0.5f, 5f, 5f, 5f,   0.0f, 0.0f,              //

            //0.5f, 0.5f, 0.5f   0.5f, 0.0f, 0.5f    0.5f, 0.5f, -0.5f      0.5f, 0.0f, -0.5f
            0.5f, 0.5f, 0.5f,   5f, 5f, 5f,   1.0f, 1.0f,                //
            0.5f, 0.0f, 0.5f,   5f, 5f, 5f,   0.0f, 1.0f,                //    IT IS THE BODY
            0.5f, 0.5f, -0.5f,  5f, 5f, 5f,   1.0f, 0.0f,                //  
            0.5f, 0.5f, -0.5f,  5f, 5f, 5f,   1.0f, 0.0f,                //    RIGHT SIDE FACE
            0.5f, 0.0f, 0.5f,   5f, 5f, 5f,   0.0f, 1.0f,                //
            0.5f, 0.0f, -0.5f,  5f, 5f, 5f,   0.0f, 0.0f,                //

            // -0.5f, 0.0f, 0.5f    -0.5f,  0.5f,  0.5f    -0.5f, 0.0f, -0.5f   -0.5f,  0.5f,  -0.5f
            -0.5f, 0.0f, 0.5f,    5f, 5f, 5f,   0.0f, 1.0f,               //
            -0.5f, 0.5f,  0.5f,   5f, 5f, 5f,   1.0f, 1.0f,               //    IT IS THE BODY 
            -0.5f, 0.0f, -0.5f,   5f, 5f, 5f,   0.0f, 0.0f,               //  
            -0.5f, 0.0f, -0.5f,   5f, 5f, 5f,   0.0f, 0.0f,               //    LEFT SIDE FACE
            -0.5f, 0.5f, 0.5f,    5f, 5f, 5f,   1.0f, 1.0f,               //
            -0.5f,  0.5f, -0.5f,  5f, 5f, 5f,   1.0f, 0.0f,               //

            //0.5f, 0.0f, 0.5f,      -0.5f, 0.0f, 0.5f,     -0.5f, 0.0, -0.5f       0.5f, 0.0f, -0.5f
            0.5f, 0.0f, 0.5f,    5f, 5f, 5f,  1.0f, 1.0f,                 //
            -0.5f, 0.0f, 0.5f,   5f, 5f, 5f,  0.0f, 1.0f,                //    IT IS THE  BODY
            -0.5f, 0.0f, -0.5f,  5f, 5f, 5f,  0.0f, 0.0f,                 //  
            -0.5f, 0.0f, -0.5f,  5f, 5f, 5f,  0.0f, 0.0f,                 //     BOTTOM FACE
            0.5f, 0.0f, 0.5f,    5f, 5f, 5f,  1.0f, 1.0f,                 //
            0.5f, 0.0f, -0.5f,   5f, 5f, 5f,  1.0f, 0.0f,                //

             
             //// 0.5f, 0.5f, -0.5f,  -0.5f, 0.5f, -0.5f,    0.25f, 0.3125f,  -1.0f,     -0.25f, 0.3125f, -1.0f              0.0f, 0.5f, 0.0f
             0.5f, 0.5f, -0.5f,       5f, 5f, 5f,  1.0f, 0.0f,                 //
             -0.5f, 0.5f, -0.5f,      5f, 5f, 5f,  0.0f, 0.0f,                 //    IT IS THE FRONT 
             0.25f, 0.3125f, -1.0f,   5f, 5f, 5f,  .75f, 1.0f,                 //    
             -0.5f, 0.5f, -0.5f,      5f, 5f, 5f,  0.0f, 0.0f,                 //    TOP GOING DOWN
             0.25f, 0.3125f, -1.0f,   5f, 5f, 5f,  0.75f, 1.0f,                 //
             -0.25f, 0.3125f, -1.0f,  5f, 5f, 5f,  0.25f, 1.0f,                 //

             //0.5f, 0.0f, -0.5f,      -0.5f, 0.0f, -0.5f,    0.5f, -0.1875f, -1.0f,
             0.5f, 0.0f, -0.5f,       5f, 5f, 5f,  1.0f, 1.0f,               //
             -0.5f, 0.0f, -0.5f,      5f, 5f, 5f,  0.0f, 1.0f,               //    IT IS THE FRONT
             0.25f, 0.1875f, -1.0f,   5f, 5f, 5f,  0.75f, 0.0f,              //  
             -0.5f, 0.0f, -0.5f,      5f, 5f, 5f,  0.0f, 1.0f,               //    BOTTOM GOING UPP  
             0.25f, 0.1875f, -1.0f,   5f, 5f, 5f,  0.75f, 0.0f,              //
             -0.25f, 0.1875f, -1.0f,  5f, 5f, 5f,  0.25f, 0.0f,              //

             //  0.5f, 0.5f, -0.5f,   0.5f, 0.0f, -0.5f,  0.25f, 0.3125f, -1.0f,  0.25f, 0.1875f, -1.0f,
             0.5f, 0.5f, -0.5f,        5f, 5f, 5f,   1.0f, 1.0f,            //
             0.5f, 0.0f, -0.5f,        5f, 5f, 5f,   1.0f, 0.0f,            //    IT IS THE FRONT
             0.25f, 0.3125f, -1.0f,    5f, 5f, 5f,   0.0f, 0.6f,            //  
             0.5f, 0.0f, -0.5f,        5f, 5f, 5f,   1.0f, 0.0f,            //    RIGHT SIDE GION LEFT
             0.25f, 0.3125f, -1.0f,    5f, 5f, 5f,   0.0f, 0.6f,            //    
             0.25f, 0.1875f, -1.0f,    5f, 5f, 5f,   0.0f, 0.35f,           //

             // -0.5f, 0.5f, -0.5f,   -0.5f, 0.0f, -0.5f,  -0.25f, 0.3125f, -1.0f,    -0.25f, 0.1875f, -1.0f,
             -0.5f, 0.5f, -0.5f,      5f, 5f, 5f,  1.0f, 1.0f,              //
             -0.5f, 0.0f, -0.5f,      5f, 5f, 5f,  1.0f, 0.0f,              //    IT IS THE FRONT
             -0.25f, 0.3125f, -1.0f,  5f, 5f, 5f,  0.0f, 0.6f,              //  
             -0.5f, 0.0f, -0.5f,      5f, 5f, 5f,  1.0f, 0.0f,              //    LEFT SIDE GOING RIGHT
             -0.25f, 0.3125f, -1.0f,  5f, 5f, 5f,  0.0f, 0.6f,              //     
             -0.25f, 0.1875f, -1.0f,  5f, 5f, 5f,  0.0f, 0.35f,              //

             // 0.25f, 0.3125f, -1.0f,       -0.25f, 0.3125f, -1.0f,    -0.25f, 0.1875f, -1.0f,     0.25f, 0.1875f, -1.0f  
             0.25f, 0.3125f, -1.0f,   5f, 5f, 5f,  0.75f, 0.75f,               //
             -0.25f, 0.3125f, -1.0f,  5f, 5f, 5f,  0.25f, 0.75f,               //    IT IS THE FRONT
             -0.25f, 0.1875f, -1.0f,  5f, 5f, 5f,  0.25f, 0.25f,               //  
             0.25f, 0.3125f, -1.0f,   5f, 5f, 5f,  0.75f, 0.75f,               //    LAST FRONT PECIE
             -0.25f, 0.1875f, -1.0f,  5f, 5f, 5f,  0.25f, 0.25f,               //    CONECTING THE TOP, BOTTOM, AND SIDES 
             0.25f, 0.1875f, -1.0f,   5f, 5f, 5f,  0.75f, 0.25f,               //

            //0.5f, 0.25f, 0.20f     0.5f, 0.25f, -0.20f   1.5f, 0.25f, 0.20f  1.5f, 0.25f, -.20f  
             0.5f, 0.25f, 0.20f,  5f, 5f, 5f,  0.0f, 1.0f,                  //
             0.5f, 0.25f, -0.20f, 5f, 5f, 5f,  0.0f, 0.0f,                  //    IT IS THE RIGHT WING
             1.5f, 0.25f, 0.20f,  5f, 5f, 5f,  1.0f, 1.0f,                  //  
             1.5f, 0.25f, 0.20f,  5f, 5f, 5f,  1.0f, 1.0f,                  //
             0.5f, 0.25f, -0.20f, 5f, 5f, 5f,  0.0f, 0.0f,                  //
             1.5f, 0.25f, -0.20f, 5f, 5f, 5f,  1.0f, 0.0f,                  //

             //-0.5f, 0.25f, 0.20f     -0.5f, 0.25f, -0.20f   -1.5f, 0.25f, 0.20f  -1.5f, 0.25f, -.20f    
             -0.5f, 0.25f, 0.20f,  5f, 5f, 5f,   1.0f, 1.0f,               //
             -0.5f, 0.25f, -0.20f, 5f, 5f, 5f,   1.0f, 0.0f,               //    IT IS THE LEFT WING
             -1.5f, 0.25f, 0.20f,  5f, 5f, 5f,   0.0f, 1.0f,               //  
             -1.5f, 0.25f, 0.20f,  5f, 5f, 5f,   0.0f, 1.0f,               //
             -0.5f, 0.25f, -0.20f, 5f, 5f, 5f,   1.0f, 0.0f,               //
             -1.5f, 0.25f, -0.20f, 5f, 5f, 5f,   0.0f, 0.0f,               //

                                // space
             -0.5f, -0.5f, -0.5f,  0.5f, 0.5f, 0.5f,   0.0f, 0.0f,
             0.5f, -0.5f, -0.5f,   0.5f, 0.5f, 0.5f,   1.0f, 0.0f,
             0.5f,  0.5f, -0.5f,   0.5f, 0.5f, 0.5f,   1.0f, 1.0f,
             0.5f,  0.5f, -0.5f,   0.5f, 0.5f, 0.5f,   1.0f, 1.0f,
            -0.5f,  0.5f, -0.5f,   0.5f, 0.5f, 0.5f,   0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,   0.5f, 0.5f, 0.5f,   0.0f, 0.0f,

            -0.5f, -0.5f,  0.5f,   0.5f, 0.5f, 0.5f,   0.0f, 0.0f,
             0.5f, -0.5f,  0.5f,   0.5f, 0.5f, 0.5f,   1.0f, 0.0f,
             0.5f,  0.5f,  0.5f,   0.5f, 0.5f, 0.5f,   1.0f, 1.0f,
             0.5f,  0.5f,  0.5f,   0.5f, 0.5f, 0.5f,   1.0f, 1.0f,
            -0.5f,  0.5f,  0.5f,   0.5f, 0.5f, 0.5f,   0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,   0.5f, 0.5f, 0.5f,   0.0f, 0.0f,

            -0.5f,  0.5f,  0.5f,   0.5f, 0.5f, 0.5f,   1.0f, 0.0f,
            -0.5f,  0.5f, -0.5f,   0.5f, 0.5f, 0.5f,   1.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,   0.5f, 0.5f, 0.5f,   0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,   0.5f, 0.5f, 0.5f,   0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,   0.5f, 0.5f, 0.5f,   0.0f, 0.0f,
            -0.5f,  0.5f,  0.5f,   0.5f, 0.5f, 0.5f,   1.0f, 0.0f,

             0.5f,  0.5f,  0.5f,   0.5f, 0.5f, 0.5f,   1.0f, 0.0f,
             0.5f,  0.5f, -0.5f,   0.5f, 0.5f, 0.5f,   1.0f, 1.0f,
             0.5f, -0.5f, -0.5f,   0.5f, 0.5f, 0.5f,   0.0f, 1.0f,
             0.5f, -0.5f, -0.5f,   0.5f, 0.5f, 0.5f,   0.0f, 1.0f,
             0.5f, -0.5f,  0.5f,   0.5f, 0.5f, 0.5f,   0.0f, 0.0f,
             0.5f,  0.5f,  0.5f,   0.5f, 0.5f, 0.5f,   1.0f, 0.0f,

            -0.5f, -0.5f, -0.5f,   0.5f, 0.5f, 0.5f,   0.0f, 1.0f,
             0.5f, -0.5f, -0.5f,   0.5f, 0.5f, 0.5f,   1.0f, 1.0f,
             0.5f, -0.5f,  0.5f,   0.5f, 0.5f, 0.5f,   1.0f, 0.0f,
             0.5f, -0.5f,  0.5f,   0.5f, 0.5f, 0.5f,   1.0f, 0.0f,
            -0.5f, -0.5f,  0.5f,   0.5f, 0.5f, 0.5f,   0.0f, 0.0f,
            -0.5f, -0.5f, -0.5f,   0.5f, 0.5f, 0.5f,   0.0f, 1.0f,

            -0.5f,  0.5f, -0.5f,   0.5f, 0.5f, 0.5f,   0.0f, 1.0f,
             0.5f,  0.5f, -0.5f,   0.5f, 0.5f, 0.5f,   1.0f, 1.0f,
             0.5f,  0.5f,  0.5f,   0.5f, 0.5f, 0.5f,   1.0f, 0.0f,
             0.5f,  0.5f,  0.5f,   0.5f, 0.5f, 0.5f,   1.0f, 0.0f,
            -0.5f,  0.5f,  0.5f,   0.5f, 0.5f, 0.5f,   0.0f, 0.0f,
            -0.5f,  0.5f, -0.5f,   0.5f, 0.5f, 0.5f,   0.0f, 1.0f,
        };
            CalcSpaceCraftNormal(_vertices);
        }
        private void CalcSpaceCraftNormal(float[] vertecies )
        {
         Vector3 normal;
         for (int i = 0; i < (vertecies.Length / 48); i += 48)
         {
          Vector3 a = new Vector3(vertecies[i], vertecies[i + 1], vertecies[i + 2]);
          Vector3 b = new Vector3(vertecies[i + 8], vertecies[i + 9], vertecies[i + 10]);
          Vector3 c = new Vector3(vertecies[i + 16], vertecies[i + 17], vertecies[i + 18]);
       
          normal = Vector3.Cross(b - a, c - a).Normalized();
          for (int j = i; j < 48; j+= 8)
          {
           vertecies[i + j + 3] = normal.X;
           vertecies[i + j + 4] = normal.Y;
           vertecies[i + j + 5] = normal.Z;
          }
         }
        }
        
    }
}
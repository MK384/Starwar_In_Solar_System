using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;


namespace ComputerGraphics.GraphObjects
{
    public class Sun : GraphObject
    {
        
        
        public Sun(float radius):base(new Vector3(0f,0f,0f), radius , true)
        {
            setTexture("resources\\Sun.jpg");
        }

        public override void OnRenderFrame(Shader shader, float time)
        {
            base.OnRenderFrame(shader , time);
            GL.DrawArrays(PrimitiveType.Triangles, 0, _vertices.Length/8);
            GL.BindVertexArray(0); // set the binded vertex array to null
        }

        protected override void ImportStandardShapeData()
        {
            _vertices = new SphereFactory().GetVertices();
        }
    }
}
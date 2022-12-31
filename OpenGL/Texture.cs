using System;
using OpenTK.Graphics.OpenGL;


using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ComputerGraphics
{
    public class Texture
    {
        private int _handle;

        public Texture(String path)
        {
            GL.Enable(EnableCap.Texture2D);
            _handle = GL.GenTexture();
            this.Use();
            // GL.BindSampler();
            
            //Load the image
            Image<Rgba32> image = Image.Load<Rgba32>(path);

            //ImageSharp loads from the top-left pixel, whereas OpenGL loads from the bottom-left, causing the texture to be flipped vertically.
            //This will correct that, making the texture display properly.
            image.Mutate(x => x.Flip(FlipMode.Vertical));
            
            //Use the CopyPixelDataTo function from ImageSharp to copy all of the bytes from the image into an array that we can give to OpenGL.
            var pixels = new byte[4 * image.Width * image.Height];
            image.CopyPixelDataTo(pixels);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

        }

        public void Use(TextureUnit unit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, _handle);
            
        }


    }
}
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using OpenTK.GLControl;
using OpenTK.Graphics.OpenGL4;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using PixelFormatGL = OpenTK.Graphics.OpenGL4.PixelFormat;

namespace Model;

public static class Window
{
    public static int widthScreen = Screen.PrimaryScreen?.Bounds.Width ?? 0;
    public static int heightScreen = Screen.PrimaryScreen?.Bounds.Height ?? 0;

    private static int textureId;
    private static int vaoId;
    private static int vboId;
    private static int programId;
    private static byte[] screenBuffer = new byte[widthScreen * heightScreen * 4];
    

    public static unsafe void InitGL()
    {
        GL.ClearColor(Color.White);

        // Создаем и привязываем текстуру
        textureId = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, textureId);
        
        // Настраиваем аппаратную фильтрацию Mipmap для сжатия силами GPU
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

        // Координаты полигона (двойной треугольник) на весь экран
        float[] vertices = {
            -1.0f,  1.0f,       0.0f, 0.0f,
            -1.0f, -1.0f,       0.0f, 1.0f,
             1.0f, -1.0f,       1.0f, 1.0f,

            -1.0f,  1.0f,       0.0f, 0.0f,
             1.0f, -1.0f,       1.0f, 1.0f,
             1.0f,  1.0f,       1.0f, 0.0f 
        };

        vaoId = GL.GenVertexArray();
        vboId = GL.GenBuffer();

        GL.BindVertexArray(vaoId);
        GL.BindBuffer(BufferTarget.ArrayBuffer, vboId);
        
        // В OpenTK 4 аргумент размера должен быть типа nint (nuint)
        fixed (float* v = vertices)
        {
            GL.BufferData(BufferTarget.ArrayBuffer, (nint)(vertices.Length * sizeof(float)), (IntPtr)v, BufferUsageHint.StaticDraw);
        }

        // Настройка указателей шейдера
        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 2 * sizeof(float));
        GL.EnableVertexAttribArray(1);

        CompileShaders();
    }

    private static void CompileShaders()
    {
        string vertexShaderSource = @"
            #version 400
            layout (location = 0) in vec2 aPos;
            layout (location = 1) in vec2 aTexCoord;
            out vec2 TexCoord;
            void main() {
                gl_Position = vec2(aPos.x, aPos.y);
                TexCoord = aTexCoord;
            }";

        string fragmentShaderSource = @"
            #version 400
            out vec4 FragColor;
            in vec2 TexCoord;
            uniform sampler2D texture1;
            void main() {
                // Аппаратная выборка сжатого изображения
                FragColor = texture(texture1, TexCoord); 
            }";

        int vs = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vs, vertexShaderSource);
        GL.CompileShader(vs);

        int fs = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fs, fragmentShaderSource);
        GL.CompileShader(fs);

        programId = GL.CreateProgram();
        GL.AttachShader(programId, vs);
        GL.AttachShader(programId, fs);
        GL.LinkProgram(programId);

        GL.DeleteShader(vs);
        GL.DeleteShader(fs);
    }

    public static unsafe void PaintGL(GLControl control)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit);

        // Захват экрана на CPU
        CaptureScreenRaw(screenBuffer);

        GL.BindTexture(TextureTarget.Texture2D, textureId);
        fixed (byte* p = screenBuffer)
        {
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, 
                widthScreen, heightScreen, 
                0, PixelFormatGL.Bgra, PixelType.UnsignedByte, (IntPtr)p);
        }

        // Заставляем GPU мгновенно построить уменьшенные копии кадра (генерация мипмапов)
        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

        GL.UseProgram(programId);
        GL.BindVertexArray(vaoId);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 6);

        // Буферный обмен кадра
        control.SwapBuffers();
    }

    private static void CaptureScreenRaw(byte[] buffer)
    {
        Rectangle bounds = new Rectangle(0, 0, widthScreen, heightScreen);
        using var bmp = new Bitmap(widthScreen, heightScreen, PixelFormat.Format32bppArgb);
        
        using (Graphics g = Graphics.FromImage(bmp))
        {
            g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
        }

        var bitmapData = bmp.LockBits(bounds, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
        System.Runtime.InteropServices.Marshal.Copy(bitmapData.Scan0, buffer, 0, buffer.Length);
        bmp.UnlockBits(bitmapData);
    }

    public static void ShutdownGL()
    {
        GL.DeleteVertexArray(vaoId);
        GL.DeleteBuffer(vboId);
        GL.DeleteTexture(textureId);
        GL.DeleteProgram(programId);
    }
}
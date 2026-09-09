using System;
using System.Drawing;
using System.Windows.Forms;
using OpenTK;
using OpenTK.GLControl;
using OpenTK.Graphics.OpenGL4;

using static Model.Write;
using static Model.CanvasManage;
using static Model.Global;

namespace Model;

public partial class Canvas : Form
{  
    public static int widthForm => widthForm;
    public static int heightForm => heightForm;
    public static int widthScreen => Window.widthScreen;
    public static int heightScreen => Window.heightScreen;
    private GLControl glControl;

    public Canvas()
    {
        glControl = new GLControl
        {
            Dock = DockStyle.Fill
        };

        glControl.Load += OnGLLoad;
        glControl.Paint += OnGLPaint;

        Controls.Add(glControl);
    }

    private void OnGLLoad(object? sender, EventArgs e)
    {
        // Делаем контекст созданного окна текущим
        glControl.MakeCurrent();

        InitializeComponent();

        //CheckIt("Form are: ", heightForm+"_"+widthForm+"_"+Text+"_"+Size+"_"+ClientSize);

        // Инициализируем привязки OpenGL 4 для OpenTK 4.x
        GL.LoadBindings(new OpenTK.Windowing.GraphicsLibraryFramework.GLFWBindingsContext());

        // Запуск инициализации ресурсов в Window.cs
        Window.InitGL();
    }

    private void OnGLPaint(object? sender, PaintEventArgs e)
    {
        if (!updateFrame) return;

        // Делегируем прорисовку кадра и аппаратное сжатие в Window.cs
        Window.PaintGL(glControl);

        updateFrame = false;
    }

    public void UpdateFrame()
    {
        if (InvokeRequired)
        {
            BeginInvoke(new Action(UpdateFrame));
            return;
        }

        updateFrame = true;
        glControl.Invalidate(); 
    }

    /*private System.ComponentModel.IContainer? components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.StartPosition = FormStartPosition.CenterScreen;
        this.ClientSize = new Size(widthForm, heightForm);
        this.Text = "Canvas";
    }*/

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        Window.ShutdownGL();
        base.OnFormClosing(e);
    }
}
using System;
using System.Windows.Forms;
using System.Drawing;

namespace Model;

partial class Canvas
{
    private System.ComponentModel.IContainer components = null;

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
    }
    
}
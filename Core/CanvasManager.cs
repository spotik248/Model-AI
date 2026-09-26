//using System;

using static Model.Write;
using static Model.Global;

namespace Model;

public static class CanvasManage
{
    public static Canvas canvas = new();
    public static bool startForm;
    public static bool startUpdate;
    public static int widthScreen => Window.widthScreen;
    public static int heightScreen => Window.heightScreen;
    public static int widthForm;
    public static int heightForm;
    public static bool updateFrame;

    static CanvasManage() => Start();

    public static void Start() // Форма
    {
        canvas ??= new Canvas();

        startForm = false;
        startUpdate = false;

        widthForm = 1920;
        heightForm = 1080;

        if (widthScreen <= 0 || heightScreen <= 0)
            Throw($"An alone screen size are zero: {widthScreen}x{heightScreen}");
    }

    public async static void StartForm()
    {
        OpenTK.Windowing.Desktop.GLFWProvider.CheckForMainThread = false;

        IsBegin();

        Thread threadForm = new(() =>
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ApplicationConfiguration.Initialize();
            canvas = new();
            Application.Run(canvas);
        });

        threadForm.SetApartmentState(ApartmentState.STA); // Без этого Canvas.Designer.cs не сработает
        threadForm.Start();

        await WaitLoadThreadForm(threadForm);
        
        Line("Окно запущено.");
    }
}
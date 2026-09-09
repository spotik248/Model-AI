//using System;

using static Model.Write;

//dotnet build -c Release

namespace Model;

public static class FileSystem
{
    public static string root => Util.root;
    public static string[][] reFolder = [];
    public static Dictionary<string, string> folders { get; private set; } = [];
    public static string[][] reFiles = [];
    public static Dictionary<string, string> files { get; private set; } = [];
    public static string[] getFiles = [];
    public static string[][] sortedFiles = [];

    static FileSystem() => Start();

    public static void Offset()
    {
        reFolder = [
            ["data", @"data"],
            ["nn_data", @"data\NNdata"],
            ["logs", @"logs"],
            ["sounds", @"sounds"],
            ["learnPocket", @"learnPocket"]
        ];

        reFiles = [
            ["setting", @"Setting.json"],
            ["data", @"data\Data.json"],
            ["model_data", @"data\ModelData.json"],
            ["words_data", @"data\WordsData.json"],
            ["lib_data", @"data\LibData.json"],
            ["learn_data", @"data\LearnData.json"],
            ["lnn_data", @"data\NNdata\LNNData.json"],
            ["fnn_data", @"data\NNdata\FNNData.json"],
            ["vnn_data", @"data\NNdata\VNNData.json"],
            ["nn_data", @"data\NNdata\NNData.json"],
            ["tnn_data", @"data\NNdata\TNNData.json"],
            ["memory_data", @"data\NNdata\MemoryData.json"],
            ["line_write", @"logs\LineWrite.txt"],
            ["line_color", @"logs\LineColor.txt"],
            ["read_line", @"logs\ReadLine.bat"],
            ["read_color", @"logs\ReadColor.bat"]
        ];
    }

    public static void Start()
    {
        Offset();
        
        WriteCombines();

        CreateFolders();

        CreateFiles();

        getFiles = Util.GetFiles(Util.root, "*", SearchOption.AllDirectories);
        sortedFiles = Util.SortExtensionFiles(getFiles);
    }

    public static void WriteCombines()
    {
        // запись начальных папок в folders
        foreach (var f in reFolder)
            folders.Add(f[0], Util.RootCombine(f[1]));

        // запись начальных файлов в files
        foreach (var f in reFiles) 
            files.Add(f[0], Util.RootCombine(f[1]));
        
    }

    public static void CreateFolders()
    {
        // Создание папок из "значений" folders
        Util.CreateFolders(folders.Values.ToArray());
    }

    public static void CreateFiles()
    {
        // Создание файлов из "значений" files
        Util.CreateFiles(files.Values.ToArray());
    }

    public static string GetPathFolder(string shortName)
    {
        return folders[shortName];
    }

    public static string GetPathFile(string shortName)
    {
        return files[shortName];
    }

}
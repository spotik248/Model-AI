using System;
using System.IO;
using System.Diagnostics;
using Newtonsoft;
using Newtonsoft.Json;

using static Model.Write;

namespace Model;

public static class Util
{
    //TODO: DirectoryInfo
    //TODO: Enviroment.SpecialFolder
    public static string root = Directory.GetCurrentDirectory();
    public static string[] GetFiles(string path) => Directory.GetFiles(path);
    public static string[] GetFiles(string path, string type) => Directory.GetFiles(path, type);
    public static string[] GetFiles(string path, string type, SearchOption searchOption) => Directory.GetFiles(path, type, searchOption);
    
    public static string[] Combine(string path, string[] files) => files.Select(file => Path.Combine(path, file)).ToArray();
    public static string Combine(string path, string file) => Path.Combine(path, file);

    public static string[] RootCombine(string[] files) => files.Select(file => Path.Combine(root, file)).ToArray();
    public static string RootCombine(string file) => Path.Combine(root, file);

    public static void CreateFiles(params string[] paths)
    {
        foreach (var path in paths)
            if (!File.Exists(path)) File.Create(path);
    }

    public static void CreateFolders(params string[] paths)
    {
        foreach (var path in paths)
            Directory.CreateDirectory(path);
    }
    
    public static string[] ReadLines(string path) // В основном для NeuroPocketAnalis
    {
        try
        {
            if (!File.Exists(path))
            {
                Debug($"Файл {path} не найден.");
            }

            string[] content = File.ReadAllLines(path);

            return content;
        }
        catch (Exception exc)
        {
            Exc($"Ошибка чтения файла. ", exc);
        }
        return [];
    }
    
    public static string UnExcConsoleRead() => Console.ReadLine()?.Trim().ToLower() ?? ""; // Никогда не null

    public static string ReadFileConsole(string pathRead) // Использовать при поломке основной консоли, запись в которую идет обычно через текстовый файл input.txt
    {
        try
        {
            if (!File.Exists(pathRead))
            {
                MsgLine($"Файл {pathRead} не найден.");
                return "";
            }

            string content = File.ReadAllText(pathRead);

            File.WriteAllText(pathRead, "");

            MsgLine("Данные успешно считаны и очищены.");

            return content;
        }
        catch (IOException exc)
        {
            Exc($"Ошибка ввода-вывода.", exc);
        }
        return "";
    }

    public static void Clear(params string[] paths)
    {
        foreach (var path in paths)
            File.WriteAllText(path, "");
    }

    public static void ExecuteFile(string filePath) // Выполняет файл в стандартной программе для операционный системы
    {
        try
        {
            Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
        }
        catch (Exception exc)
        {
            Exc("Ошибка при попытке запустить файл: ", exc);
        }
    }

    public static string[][] SortExtensionFiles(string[] files)
    {
        List<string>[] filesLists = new List<string>[5];
        for (int i = 0; i < filesLists.Length; i++)
            filesLists[i] = new List<string>();

        string[][] categories = [
            ["cs", "exe", "bat", "pdb", "deps", "runtimeconfig"],
            ["json"],
            ["dll"],
            ["txt", "md"]
        ];

        // Для обрезания начала и замены его на ./fileOrFolder/...
        int rootLength = root.Length;

        foreach (string fileExt in files)
        {
            string ext = Path.GetExtension(fileExt)?.ToLowerInvariant() ?? ""; //вернет ".extension", например для "name.Audio.dll"
            
            ext = ext.StartsWith('.') ? ext.Substring(1) : ext;

            // Для обрезания начала и замены его на ./fileOrFolder/...
            string file = "." + MyString.Remove(fileExt, rootLength);

            bool added = false;

            for (int j = 0; j < categories.Length; j++)
            {
                if (categories[j].Contains(ext))
                {
                    filesLists[j].Add(file);
                    added = true;
                    break; // Файл найден, выходим из цикла категорий
                }
            }

            // Если категория не нашлась, отправляем в последний список
            if (!added) 
            {
                filesLists[filesLists.Length - 1].Add(file);
            }
        }

        // 4. Конвертируем в зубчатый массив (string[][])
        string[][] filesArray = new string[filesLists.Length][];
        for (int i = 0; i < filesLists.Length; i++)
            filesArray[i] = filesLists[i].ToArray();

        return filesArray;
    }

#pragma warning disable CS8602

    public static void Save<T>(string saveFilePath, T obj)
    {
        try
        {
            using (StreamWriter file = File.CreateText(saveFilePath))
            {
                JsonSerializer serializer = new();
                serializer.Serialize(file, obj);
            }
            MsgLine($"Данные сохранены. {obj.GetType()} {obj}");
        }
        catch (Exception ex)
        {
            Exc($"Ошибка при сохранении данных. В файл: {saveFilePath}. Данные: {obj.GetType()} {obj}.", ex);
        }
    }

    public static T? Load<T>(string loadFilePath)
    {
        if (File.Exists(loadFilePath))
        {
            try
            {
                var data = File.ReadAllText(loadFilePath);
                if (data == null || data == "" || data == " ")
                {
                    Debug($"Данные пусты. Файл: {loadFilePath}, данные: {data.GetType()} {data}");
                    return default;
                }

                var settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    Formatting = Formatting.None
                };
                var loadedData = JsonConvert.DeserializeObject<T>(data, settings);

                Line($"Данные загружены. {loadedData.GetType()} {loadedData}");

                return loadedData;
            }
            catch (Exception ex)
            {
                Exc($"Ошибка при загрузке данных.", ex);
                return default;
            }
        }
        else Err($"Файл '{loadFilePath}' не найден.");
        return default;
    }

    public static T Load<T>(string loadFilePath, T beforeData)
    {
        if (File.Exists(loadFilePath))
        {
            try
            {
                var data = File.ReadAllText(loadFilePath);
                if (data == null || data == "" || data == " ")
                {
                    Debug($"Данные пусты. Файл: {loadFilePath}, данные: {data.GetType()} {data}");
                    return beforeData;
                }

                var settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    Formatting = Formatting.None
                };
                var loadedData = JsonConvert.DeserializeObject<T>(data, settings);

                Line($"Данные загружены. {loadedData.GetType()} {loadedData}");

                return loadedData;
            }
            catch (Exception ex)
            {
                Exc($"Ошибка при загрузке данных.", ex);
                return beforeData;
            }
        }
        else Err($"Файл '{loadFilePath}' не найден."); return beforeData;
    }

}

#pragma warning restore CS8602
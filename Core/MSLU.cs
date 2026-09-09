//using System;

using static Model.Write;
using static Model.FileSystem;
using static Model.Library;
using static Model.NManage;
using static Model.Learn;
using static Model.Global;

namespace Model;

class MSLU // Model Save Load Update
{
    
    public static void Save()
    {
        Util.Save(GetPathFile("model_data"), Model.info);

        Util.Save(GetPathFile("words_data"), words);

        Util.Save(GetPathFile("learn_data"), learn); // CheckIt

        Util.Save(GetPathFile("lnn_data"), (LNN)nn[0]);
    }

    // ["setting", @"Setting.json"],
    // ["data", @"data\Data.json"],
    // ["model_data", @"data\ModelData.json"],
    // ["words_data", @"data\WordsData.json"],
    // ["library_data", @"data\LibraryData.json"],
    // ["learn_data", @"data\LearnData.json"],
    // ["enn_data", @"data\NNdata\ENNData.json"],
    // ["fnn_data", @"data\NNdata\FNNData.json"],
    // ["vnn_data", @"data\NNdata\VNNData.json"],
    // ["rnn_data", @"data\NNdata\RNNData.json"],
    // ["tnn_data", @"data\NNdata\TNNData.json"],
    // ["memory_data", @"data\NNdata\MemoryData.json"],

    public static void Load()
    {

        // ?? new string[md.Length]; //TODO: Если впринципе данные инициируются, то зачем нужно это?
        string[] md = Util.Load(GetPathFile("model_data"), Model.info);

        Model.model = md[2]; Model.user = md[3];
        
        words = Util.Load(GetPathFile("words_data"), words);

        //lib = Util.Load(files["lib_data"], lib);

        //learn = Util.Load(files["learn_data"], learn);

        nn[0] = Util.Load(files["lnn_data"], (LNN)nn[0]);
    }

    //ModelData, WordsData, RNNData, TNNData, Setting, Data

    public static void Update(bool unload)
    {
        /*
        if (!unload) //Загрузка
        {
            md = [version, description, nameUser, nameModel, startDateTime];
            nn = [];

            variable = new Dictionary<string, object> {
                {"model", md}, 
                {"root", root},
                {"requiredFiles", requiredFiles},
                {"filesPath", filesPath},
                {"files", files},
                {"sortedFiles", sortedFiles},
                {"dimension", dimension},
                {"", },
                {"size", size},
                {"learningRate", learningRate},
                {"minLR", minLR},
                {"cnn", cnn},
                {"epoch", epoch},
                {"epoches", epoches},
                {"words", words},
                {"writeLog", writeLog},
                {"writeLine", writeLine},
            };
        }
        else // Разгрузка
        {
            md = (object[])variable["model"];
            root = (string)variable["root"];
            requiredFiles = (string[])variable["requiredFiles"];
            filesPath = (string[])variable["filesPath"];//
            files = (string[])variable["files"];
            sortedFiles = (string[][])variable["sortedFiles"];//
            dimension = (int)variable["dimension"];  = (bool)variable[""]; size = (int)variable["size"];//
            learningRate = (double)variable["learningRate"]; minLR = (double)variable["minLR"];//
            epoch = (int)variable["epoch"]; epoches = (int)variable["epoches"];//
            words = (Word[])variable["words"];
            gru = (GRU)variable["gru"]; rnn = (MNN)variable["rnn"]; tnn = (TNN)variable["tnn"];//
            writeLog = (bool)variable["writeLog"]; writeLine = (bool)variable["writeLine"]; checkIt = (bool)variable["checkIt"];
        }*/
    }

}
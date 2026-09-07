//using System;
//using System.Collections.Generic;

namespace Model;

// string[][] choose, params Action[] methods

// Quest("Какую нейронную сеть использовать? (TODO)",
//      [["е", "эмоциональная"], ["ф", "функциональная"], ["м", "матричная"], ["р", "рекуррентную"], ["т", "трансформер"]],
//      [
//          () => NN.ChangeNN(0),
//          () => NN.ChangeNN(1),
//          () => NN.ChangeNN(2),
//          () => NN.ChangeNN(3),
//          () => NN.ChangeNN(4)
//      ]
// );

// Args [char, string, Action]
//                                                                        without arg | /n, Desc
// "Help", "Get your list of commands", [["", "standart function", () => {GetHelp()}], ["n", "Set time"], ["k", "Get a location of work"]], ""

public struct Cmd(string Name, string Desc, int Type, params object[][] Args) : IStructFormable
{
    public string name = Name;
    public string desc = Desc;
    public int type = Type;
    public object[][] args = Args; // [ [char, string, Action], [char, string, Action] ] -- Первый всегда должен быть

    public readonly bool IsFormatted()
    {
        return args[0][0] is char
            && args[0][1] is string
            && args[0][2] is Delegate;
    }

    public readonly bool IsLengthFine()
    {
        return args[0].Length == 3;
    }

    public readonly bool IsArray()
    {
        return args.IsArray();
    }

    public static bool IsNull() => IsNull();

   /*
   public Dictionary<string, object> ToKeyValue()
   {
      return new Dictionary<string, object>() {
         {"token", token},
         {"vector", vector},
         {"dim", dim}
      };
   }

   public static Word ToStandart(Dictionary<string, object> keyValue) {
      string token = (string)keyValue["token"];
      double[] vector = (double[])keyValue["vector"];
      int dim = (int)keyValue["dim"];

      return new Word(token, vector, dim);
   }
   */

}
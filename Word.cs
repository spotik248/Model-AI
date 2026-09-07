//using System;
//using System.Collections.Generic;

namespace Model;

public struct Word(string Token, double[] Vector) : IStructFormable
{
   public string token = Token;
   public double[] vector = Vector;

   public readonly Word Randomized(int dim) => new(token, Init.Randomized(dim));

   public readonly Word Zeros(int dim) => new(token, Init.Zeros(dim));

   public readonly Word Full(int dim, double num) => new(token, Init.Full(num, dim));

   public override readonly string ToString()
   {
      return $"Token: {token}. Vector: {MyString.ToString(vector)}";
   }

   public readonly string ToRound()
   {
      return $"Token: {token}. Object: {MyString.ToString(Math.Round(vector))}";
   }

   public readonly bool IsArray()
   {
      return vector.IsArray();
   }

   public static bool IsNull() => IsNull();

   /*
    public Dictionary<string, object> ToKeyValue()
    {
       return new Dictionary<string, object>() {
          {"token", token},
          {"vector", vector}
       };
    }

    public static Word ToStandart(Dictionary<string, object> keyValue) {
       string token = (string)keyValue["token"];
       double[] vector = (double[])keyValue["vector"]

       return new Word(token, vector, dim);
    }
    */

}
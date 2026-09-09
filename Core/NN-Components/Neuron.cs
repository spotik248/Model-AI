//using System;
//using System.Collections.Generic;

namespace Model;

public struct Neuron(sbyte Power, byte[] Width, sbyte Bias) : IStructFormable
{
    public sbyte power = Power; // -127 до 128
    public byte[] width = Width; // 0 до 255
    public sbyte bias = Bias; // -127 до 128

    public readonly Neuron Randomized(int dim) => new(power, Init.RandomizedBytes(dim), bias);

    public readonly Neuron Zeros(int dim) => new(power, Init.Zeros<byte>(dim), 0);

    public readonly Neuron Full(int dim, byte num) => new(power, Init.Full(num, dim), bias);

    public override readonly string ToString()
    {
        return $"Power: {power}. Width: {MyString.ToString(width)}. Bias: {bias}";
    }

    public readonly string ToRound()
    {
      return $"Power: {power}. Object: {MyString.ToString(width)}. Bias: {bias}";
    }

    public readonly bool IsArray() => width.IsArray();

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
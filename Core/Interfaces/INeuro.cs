//using System;

namespace Model;

public interface INeuro : INames, ISizeNN, IPocketLearn
{
    ushort countLayout { get; set; }
    double[][] Predict(double[][] input); // input
    double[] Study(double[][] input, double[][] output); // input, output
}
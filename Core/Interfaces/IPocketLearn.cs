//using System;

namespace Model;

public interface IPocketLearn
{
    int batches { get; set; }
    bool onlyPocket { get; set; }
    double[][][] PredictPocket(double[][][] input); // input
    double[][] StudyPocket(double[][][] input, double[][][] output); // input, output
}
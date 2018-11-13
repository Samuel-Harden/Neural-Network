using System;
using System.Collections.Generic;

public static class NeuralLayer
{
    /*private static Random randomizer = new Random();

    public int neuronCount;
    public int outputCount;

    public float[,] weights;

    public NeuralLayer(int _nodeCount, int _outputCount)
    {
        neuronCount = _nodeCount;
        outputCount = _outputCount;

        weights = new float[_nodeCount + 1, _outputCount]; // +1 for bias
    }

    public List<float> CalculateInputs(List<float> _inputs)
    {
        return _inputs;
    }


    public void SetRandomWeights(float _min, float _max)
    {
        float range = Math.Abs(_min - _max);

        for (int i = 0; i < weights.GetLength(0); i++)
            for (int j = 0; j < weights.GetLength(1); j++)
                weights[i, j] = _min + ((float)randomizer.NextDouble() * range);
    }*/
}

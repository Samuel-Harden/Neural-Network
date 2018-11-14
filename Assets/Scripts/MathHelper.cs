using System;
using System.Collections.Generic;


public static class MathHelper
{
    // NN Activation Function!
    public static double SigmoidFunction(double _value)
    {
        if (_value > 10) return 1.0;

        else if (_value < -10) return 0.0;

        else return 1.0 / (1.0 + Math.Exp(-_value));
    }


    public static void SetRandomWeights(List<List<Neuron>> _neurons, float _min, float _max)
    {
        for (int i = 0; i < _neurons.Count - 1; i++)
        {
            for (int j = 0; j < _neurons[i].Count; j++)
            {
                for (int k = 0; k < _neurons[i][j].weightCount; k++)
                {
                    _neurons[i][j].weights.Add(UnityEngine.Random.Range(_min, _max));
                }
            }
        }
    }
}

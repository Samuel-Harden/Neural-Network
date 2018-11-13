using System;
using System.Collections.Generic;
using UnityEngine;

public static class MathHelper
{
    public static double SigmoidFunction(double _value)
    {
        if (_value > 10)
            return 1.0;

        else if (_value < -10)
            return 0.0;

        else
            return 1.0 / (1.0 + Math.Exp(-_value));
    }


    public static float TanHFunction(float _value)
    {
        if (_value > 10)
            return 1.0f;

        if (_value < -10)
            return -1.0f;

        else
            return (float)Math.Tanh(_value);
    }


    public static float SoftSigmoidFunction(float _value)
    {
        return _value / (1 + Math.Abs(_value));
    }


    public static void SetRandomWeights(List<List<Neuron>> _neurons)
    {
        for (int i = 0; i < _neurons.Count - 1; i++)
        {
            for (int j = 0; j < _neurons[i].Count; j++)
            {
                for (int k = 0; k < _neurons[i][j].weightCount; k++)
                {
                    _neurons[i][j].weights.Add(UnityEngine.Random.Range(-10.0f, 10.0f));
                }
            }
        }
    }
}

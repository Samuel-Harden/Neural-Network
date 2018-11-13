using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neuron
{
    public List<double> weights;
    public int weightCount;
    public double value;

    public Neuron(int _weightCount)
    {
        weightCount = _weightCount;
        weights = new List<double>();
    }
}

using System.Collections.Generic;


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

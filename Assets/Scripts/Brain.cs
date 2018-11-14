using System.Collections.Generic;


public class Brain
{
    private float fitness; // How good is this brain?
    private List<List<Neuron>> neurons; // Neurons for the NN


    public void Initialize(int[] _topology)
    {
        // Neurons get stored into each layer
        neurons = new List<List<Neuron>>();

        for (int i = 0; i < _topology.Length; i++)
        {
            neurons.Add(new List<Neuron>());

            for (int j = 0; j < _topology[i]; j++)
            {
                if (i != _topology.Length - 1)
                {
                    Neuron neuron = new Neuron(_topology[i + 1]);
                    neurons[i].Add(neuron);
                }

                else
                {
                    Neuron neuron = new Neuron(0);
                    neurons[i].Add(neuron);
                }
            }
        }
    }


    public void UpdateBrain(Brain _newBrain)
    {
        fitness = 0;

        // loop through neurons and update to new weights!
        for (int i = 0; i < neurons.Count; i++) // Layers
        {
            for (int j = 0; j < neurons[i].Count; j++) // Neurons in layer
            {
                for (int k = 0; k < neurons[i][j].weightCount; k++) // no of weights in neuron
                {
                    neurons[i][j].weights[k] = _newBrain.neurons[i][j].weights[k];
                }
            }
        }
    }


    public Neuron Neuron(int _layer, int _neuron)
    {
        return neurons[_layer][_neuron];
    }


    public List<List<Neuron>> Neurons()
    {
        return neurons;
    }


    public float Fitness()
    {
        return fitness;
    }


    public void SetFitness(float _fitness)
    {
        fitness = _fitness;
    }
}

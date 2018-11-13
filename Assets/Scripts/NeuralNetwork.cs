using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NeuralNetwork
{
    public static void ProcessNeuralNetwork(VehicleControl _vehicle)
    {
            CalculateLayers(_vehicle);
    }


    public static void CalculateLayers(VehicleControl _vehicle)
    {
        // loop through each layer (neurons.Count)
        for (int i = 0; i < _vehicle.neurons.Count; i++) // i = layer
        {
            // loop through each neuron in current layer
            for (int j = 0; j < _vehicle.neurons[i].Count; j++) // j = neuron
            {
                for (int k = 0; k < _vehicle.neurons[i][j].weightCount; k++) // k = weight of neuron
                {
                    
                    _vehicle.neurons[i + 1][k].value += (_vehicle.neurons[i][j].value * _vehicle.neurons[i][j].weights[k]);
                }
            }

            // If we are on the output layer, we dont need to activate
            // the next layer,as there isnt one!
            if (i < _vehicle.neurons.Count - 1)
            {
                // Activate the next layers neurons!
                for (int l = 0; l < _vehicle.neurons[i + 1].Count; l++)
                {
                    _vehicle.neurons[i + 1][l].value =
                        MathHelper.SigmoidFunction(_vehicle.neurons[i + 1][l].value);
                }
            }
        }
    }
}

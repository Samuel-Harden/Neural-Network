

public static class NeuralNetwork
{
    public static void ProcessNeuralNetwork(Brain _brain)
    {
                // loop through each layer (neurons.Count)
        for (int i = 0; i < _brain.Neurons().Count; i++) // i = layer
        {
            // loop through each neuron in current layer
            for (int j = 0; j < _brain.Neurons()[i].Count; j++) // j = neuron
            {
                for (int k = 0; k < _brain.Neurons()[i][j].weightCount; k++) // k = weight of neuron
                {
                    _brain.Neurons()[i + 1][k].value +=
                        (_brain.Neurons()[i][j].value * _brain.Neurons()[i][j].weights[k]);
                }
            }

            // If we are on the output layer, we dont need to activate
            // the next layer,as there isnt one!
            if (i < _brain.Neurons().Count - 1)
            {
                // Activate the next layers neurons!
                for (int l = 0; l < _brain.Neurons()[i + 1].Count; l++)
                {
                    _brain.Neurons()[i + 1][l].value =
                        MathHelper.SigmoidFunction(_brain.Neurons()[i + 1][l].value);
                }
            }
        }
    }
}

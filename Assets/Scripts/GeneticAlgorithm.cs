using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class GeneticAlgorithm : MonoBehaviour
{
    public void RegeratePopulation(ref List<Brain> _currentPop, ref List<Brain> _newPop)
    {
        // Sort list based on highest fitness...
        _currentPop = _currentPop.OrderByDescending(brain => brain.Fitness()).ToList();

        // Breed the best x no of pop to create next generation
        for (int i = 0; i < _currentPop.Count; i++)
        {
            int parentA = Random.Range(0, (_currentPop.Count / 5));

            int parentB = Random.Range(0, ((_currentPop.Count / 5) * 4));

            for (int j = 0; j < _currentPop[parentA].Neurons().Count; j++)
            {
                for (int k = 0; k < _currentPop[parentA].Neurons()[j].Count; k++)
                {
                    for (int l = 0; l < _currentPop[parentA].Neurons()[j][k].weightCount; l++)
                    {
                        // Take average point
                        //_newPop[i].Neurons()[j][k].weights.Add
                          //  ((_currentPop[parentA].Neurons()[j][k].weights[l] +
                            //_currentPop[parentB].Neurons()[j][k].weights[l]) / 2);

                        double min, max;

                        // Take a random point bwtween the two parents weights!
                        if ((_currentPop[parentA].Neurons()[j][k].weights[l] > _currentPop[parentB].Neurons()[j][k].weights[l]))
                        {
                            min = _currentPop[parentB].Neurons()[j][k].weights[l];
                            max = _currentPop[parentA].Neurons()[j][k].weights[l];
                        }

                        else
                        {
                            min = _currentPop[parentA].Neurons()[j][k].weights[l];
                            max = _currentPop[parentB].Neurons()[j][k].weights[l];
                        }

                        // take a random point between the parents weights
                        _newPop[i].Neurons()[j][k].weights.Add(Random.Range((float)min, (float)max));
                    }
                }
            }
        }
    }
}

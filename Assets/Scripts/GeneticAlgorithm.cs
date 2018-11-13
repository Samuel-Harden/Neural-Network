using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GeneticAlgorithm : MonoBehaviour
{
    public void RegeratePopulation(ref List<VehicleControl> _population, int[] _topology, GameObject _vehiclePrefab, Vector3 _spawnPos, Transform _objectContainer)
    {
        List<VehicleControl> newGeneration = new List<VehicleControl>();

        // Sort list based on highest fitness...
        _population = _population.OrderByDescending(vehicle => vehicle.fitness).ToList();

        // Breed the best x no of pop to create next generation
        for (int i = 0; i < _population.Count; i++)
        {
            GameObject vehicle = Instantiate(_vehiclePrefab, _spawnPos, Quaternion.identity);

            vehicle.GetComponent<VehicleControl>().InitializeVehicle(_topology);

            int parentA = Random.Range(0, 10);

            int parentB = Random.Range(0, _population.Count);

            for (int j = 0; j < _population[parentA].neurons.Count; j++)
            {
                for (int k = 0; k < _population[parentA].neurons[j].Count; k++)
                {
                    for (int l = 0; l < _population[parentA].neurons[j][k].weightCount; l++)
                    {
                        vehicle.GetComponent<VehicleControl>().neurons[j][k].weights.Add((_population[parentA].neurons[j][k].weights[l] + _population[parentB].neurons[j][k].weights[l]) / 2);
                    }
                }
            }

            vehicle.transform.parent = _objectContainer;

            newGeneration.Add(vehicle.GetComponent<VehicleControl>());
        }

        foreach (VehicleControl vehicle in _population)
            Destroy(vehicle.gameObject);

        _population.Clear();

        _population = newGeneration;
    }
}

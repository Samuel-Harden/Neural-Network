using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkyNet : MonoBehaviour
{
    [SerializeField] int population;
    [SerializeField] float initialWeightMin;
    [SerializeField] float initialWeightMax;

    [SerializeField] GameObject vehiclePrefab;
    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform objectContainer;
    [SerializeField] Text genCounter;

    private GeneticAlgorithm geneticAlgorithm;
    private List<VehicleControl> vehicles;

    // The Layout of the Neural Network
    // layer 0 corresponds to the number of sensors
    // The last layer corresponds a vehicles steer and torque
    private int[] topology = { 3, 5, 4, 2 };

    private int generation;

	// Use this for initialization
	void Start ()
    {
        generation = 0;

        SetGenerationText();

        geneticAlgorithm = GetComponent<GeneticAlgorithm>();

        InitializePopulation();
	}
	

	// Update is called once per frame
	void Update ()
    {
        CheckPopActive();

        if (CheckPopActive())
        {
            // update all the vehicles!
            // Set Value for each neuron based on raycast values!
            // loop them through the neural network
            // Set Values for input nodes at begining of each cycle
            SetInputs();

            foreach (VehicleControl vehicle in vehicles)
                NeuralNetwork.ProcessNeuralNetwork(vehicle.GetBrain());

            // pass Output Nodes back to Vehicle
            UpdateVehicles();
        }

        else
            NewGeneration();
	}


    // Loop through each vehicle, update the input values for the NN
    void SetInputs()
    {
        for (int i = 0; i < vehicles.Count; i++)
        {
            vehicles[i].UpdateNN();
        }
    }


    void UpdateVehicles()
    {
        for (int i = 0; i < vehicles.Count; i++)
        {
            vehicles[i].UpdateVehicle();
        }
    }


    bool CheckPopActive()
    {
        // Loop through to see if any vehicle is active
        for (int i = 0; i < vehicles.Count; i++)
        {
            if (vehicles[i].IsActive())
                return true;
        }

        // if not return false!
        return false;
    }


    public void ForceNextGeneration()
    {
        foreach (VehicleControl brain in vehicles)
            brain.SetActive(false);
    }


    private void NewGeneration()
    {
        // Abstract brains from current generation
        List<Brain> currentPopBrains = new List<Brain>();
        List<Brain> newPopBrains = new List<Brain>();

        for (int i = 0; i < vehicles.Count; i++)
        {
            // Add brains to lists
            currentPopBrains.Add(vehicles[i].GetBrain());

            Brain brain = new Brain();

            brain.Initialize(topology);

            newPopBrains.Add(brain);
        }

        // Time to generate new Population!
        geneticAlgorithm.RegeratePopulation(ref currentPopBrains, ref newPopBrains);

        // replace old Brains with new brains

        for (int j = 0; j < vehicles.Count; j++)
        {
            vehicles[j].GetBrain().UpdateBrain(newPopBrains[j]);

            vehicles[j].transform.position = spawnPoint.position;

            vehicles[j].transform.rotation = Quaternion.identity;

            vehicles[j].SetActive(true);
        }

        generation++;

        SetGenerationText();
    }


    private void InitializePopulation()
    {
        vehicles = new List<VehicleControl>();

        for (int i = 0; i < population; i++)
        {
            GameObject vehicle = Instantiate(vehiclePrefab, spawnPoint.position,
                Quaternion.identity);

            vehicle.GetComponent<VehicleControl>().InitializeVehicle(topology);

            vehicle.transform.parent = objectContainer;

            // Set Weights to start
            MathHelper.SetRandomWeights(vehicle.GetComponent<VehicleControl>()
                .GetBrain().Neurons(), initialWeightMin, initialWeightMax);

            vehicles.Add(vehicle.GetComponent<VehicleControl>());
        }
    }


    void SetGenerationText()
    {
         genCounter.text = "Generation: " + generation.ToString();
    }
}

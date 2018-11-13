using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkyNet : MonoBehaviour
{
    [SerializeField] int population;
    [SerializeField] GameObject vehicleAIPrefab;
    [SerializeField] Transform spawnPoint;

    //private NeuralNetwork neuralNetwork;
    private GeneticAlgorithm geneticAlgorithm;

    private List<VehicleControl> vehicleAI;

    [SerializeField] Transform objectContainer;

    [SerializeField] Text genCounter;

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

            foreach (VehicleControl vehicle in vehicleAI)
                NeuralNetwork.ProcessNeuralNetwork(vehicle);

            // pass Output Nodes back to Vehicle
            UpdateVehicles();
        }

        else
        {
            // Time to generate new Population!
            geneticAlgorithm.RegeratePopulation(ref vehicleAI, topology,
                vehicleAIPrefab, spawnPoint.position, objectContainer);

            //Debug.Log("Time to Repopulate!");

            generation++;

            SetGenerationText();
        }
	}


    // Loop through each vehicle, update the input values for the NN
    void SetInputs()
    {
        for (int i = 0; i < vehicleAI.Count; i++)
        {
            vehicleAI[i].UpdateNN();
        }
    }


    void UpdateVehicles()
    {
        for (int i = 0; i < vehicleAI.Count; i++)
        {
            vehicleAI[i].UpdateVehicle();
        }
    }


    bool CheckPopActive()
    {
        // Loop through to see if any vehicle is active
        for (int i = 0; i < vehicleAI.Count; i++)
        {
            if (vehicleAI[i].active)
                return true;
        }

        // if not return false!
        return false;
    }


    public void ForceNextGeneration()
    {
        foreach (VehicleControl vehicle in vehicleAI)
            vehicle.active = false;
    }


    private void InitializePopulation()
    {
        vehicleAI = new List<VehicleControl>();

        for (int i = 0; i < population; i++)
        {
            GameObject vehicle = Instantiate(vehicleAIPrefab, spawnPoint.position, Quaternion.identity);

            vehicle.GetComponent<VehicleControl>().InitializeVehicle(topology);

            vehicle.transform.parent = objectContainer;

            // Set Weights to start
            MathHelper.SetRandomWeights(vehicle.GetComponent<VehicleControl>().neurons);

            vehicleAI.Add(vehicle.GetComponent<VehicleControl>());
        }
    }


    void SetGenerationText()
    {
         genCounter.text = "Generation: " + generation.ToString();
    }
}

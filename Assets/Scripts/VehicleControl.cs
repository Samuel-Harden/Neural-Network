using System.Collections.Generic;
using UnityEngine;

public class VehicleControl : MonoBehaviour
{
    public float fitness;

    public float steerAngle = 35.0f;
    public float maxTorque = 35.0f;

    public WheelCollider wheelFL;
    public WheelCollider wheelFR;

    public List<float> sensors;

    public float left;
    public float right;
    public float forward;

    public bool active;

    public float angle;

    [SerializeField] string trackLayer;
    [SerializeField] LayerMask checkLayer;

    [SerializeField] Transform sensorForward;
    [SerializeField] Transform sensorLeft;
    [SerializeField] Transform sensorRight;

    [SerializeField] int sensorDistance;

    [SerializeField] MeshRenderer vehicleBody;


    // Brain Data
    public List<List<Neuron>> neurons;

    public void InitializeVehicle(int[] _topology)
    {
        vehicleBody.material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

        // Neurons get stored into each layer
        neurons = new List<List<Neuron>>();

        sensors = new List<float>();

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

        for (int i = 0; i < _topology[0]; i++)
        {
            sensors.Add(0);
        }
    }


    private void Start()
    {
        active = true;
    }

    public void UpdateNN()
    {
        UpdateSensors();

        for (int i = 0; i < sensors.Count; i++)
        {
            neurons[0][i].value = sensors[i];
        }
    }


    public void UpdateSensors()
    {
        UpdateSensor(sensorLeft, ref left);
        UpdateSensor(sensorRight, ref right);
        UpdateSensor(sensorForward, ref forward);

        sensors[0] = left;
        sensors[1] = right;
        sensors[2] = forward;
    }


    private void UpdateSensor(Transform _sensor, ref float _distance)
    {
        RaycastHit hit;

        if (Physics.Raycast(_sensor.position, _sensor.forward, out hit, sensorDistance, checkLayer))
        {
            if (hit.distance < sensorDistance)
            {
                //Color color = Color.red;
                //Debug.DrawLine(_sensor.transform.position, hit.point, color);

                _distance = hit.distance;
                return;
            }
        }

        _distance = sensorDistance;
    }


    public void UpdateVehicle()
    {
        if (active)
        {
            ApplySteer();
            ApplyForce();

            if (fitness < transform.position.z)
                fitness = transform.position.z;
        }
    }


    private void ApplySteer()
    {
        angle = steerAngle * (float)neurons[neurons.Count - 1][0].value;

        //if ((float)neurons[neurons.Count - 1][0].value >= 0.5f)
        angle -= (steerAngle / 2);

        wheelFL.steerAngle = angle;
        wheelFR.steerAngle = angle;
    }


    private void ApplyForce()
    {
        float torque = maxTorque * (float)neurons[neurons.Count - 1][1].value;

        wheelFL.motorTorque = torque;
        wheelFR.motorTorque = torque;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(trackLayer))
        {
            active = false;

            wheelFL.motorTorque = 0.0f;
            wheelFR.motorTorque = 0.0f;
        }
    }
}
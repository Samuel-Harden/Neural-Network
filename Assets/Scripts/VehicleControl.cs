using System.Collections.Generic;
using UnityEngine;

public class VehicleControl : MonoBehaviour
{
    [SerializeField] float steerAngle = 35.0f;
    [SerializeField] float maxTorque = 35.0f;
    [SerializeField] float maxBreakTorque = 10.0f;
    [SerializeField] int sensorDistance;

    [SerializeField] WheelCollider wheelFL;
    [SerializeField] WheelCollider wheelFR;
    [SerializeField] MeshRenderer vehicleBody;

    [SerializeField] Transform sensorForward;
    [SerializeField] Transform sensorLeft;
    [SerializeField] Transform sensorRight;

    [SerializeField] string trackLayer;
    [SerializeField] LayerMask checkLayer;

    [SerializeField] List<float> sensorValues;

    private Brain brain;

    private float left;
    private float right;
    private float forward;
    private bool active;
    private float angle;


    public void InitializeVehicle(int[] _topology)
    {
        vehicleBody.material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

        brain = new Brain();

        brain.Initialize(_topology);

        sensorValues = new List<float>();

        for (int i = 0; i < _topology[0]; i++)
            sensorValues.Add(0);
    }


    private void Start()
    {
        active = true;
    }


    public void UpdateNN()
    {
        UpdateSensors();

        for (int i = 0; i < sensorValues.Count; i++)
            brain.Neuron(0, i).value = sensorValues[i];
    }


    public void UpdateSensors()
    {
        UpdateSensor(sensorLeft, ref left);
        UpdateSensor(sensorRight, ref right);
        UpdateSensor(sensorForward, ref forward);

        sensorValues[0] = left;
        sensorValues[1] = right;
        sensorValues[2] = forward;
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
            ApplyTorque();
            //ApplyBrakeTorque();

            if (brain.Fitness() < transform.position.z)
                brain.SetFitness(transform.position.z);
        }
    }


    private void ApplySteer()
    {
        angle = steerAngle * (float)brain.Neurons()[brain.Neurons().Count - 1][0].value;

        angle -= (steerAngle / 2);

        wheelFL.steerAngle = angle;
        wheelFR.steerAngle = angle;
    }


    private void ApplyTorque()
    {
        float torque = maxTorque * (float)brain.Neurons()[brain.Neurons().Count - 1][1].value;

        // If value is greater that 0.25 lets apply some power!
        //if ((float)brain.Neurons()[brain.Neurons().Count - 1][1].value > 0.25f)
        //{
            wheelFL.motorTorque = torque;
            wheelFR.motorTorque = torque;
            return;
        //}

        // if its less we should use the brakes!
        //torque = maxBreakTorque * (float)brain.Neurons()[brain.Neurons().Count - 1][1].value;

        //wheelFL.brakeTorque = torque;
        //wheelFR.brakeTorque = torque;
    }


    private void ApplyBrakeTorque()
    {
        float torque = maxBreakTorque * (float)brain.Neurons()[brain.Neurons().Count - 1][2].value;

        wheelFL.brakeTorque = torque;
        wheelFR.brakeTorque = torque;
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


    public Brain GetBrain()
    {
        return brain;
    }


    public bool IsActive()
    {
        return active;
    }


    public void SetActive(bool _active)
    {
        active = _active;
    }
}
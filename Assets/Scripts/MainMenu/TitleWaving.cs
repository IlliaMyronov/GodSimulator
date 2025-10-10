using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TitleWaving : MonoBehaviour
{
    [SerializeField] private float rotationLimit;
    [SerializeField] private float maxRotationSpeed;

    private float currentRotation;
    private float rotationDirection;
    private float rotationSpeed;

    void Awake()
    {
        currentRotation = this.gameObject.transform.rotation.z;

        // select a random direction of rotation (-1 counter-clockwise, 1 clockwise) 
        rotationDirection = (Random.Range(0, 2) > 0 ? 1 : -1);
        rotationSpeed = Random.Range(-1 * maxRotationSpeed, maxRotationSpeed) / 360;
    }
    private void Update()
    {
        currentRotation += rotationSpeed * Time.deltaTime;
        rotationSpeed += Random.Range(-1 * rotationLimit - currentRotation, rotationLimit - currentRotation) / 360;

        if(Mathf.Abs(rotationSpeed) > maxRotationSpeed)
        {
            rotationSpeed = maxRotationSpeed * (rotationSpeed / Mathf.Abs(rotationSpeed)) / 360;
        }

        if(Mathf.Abs(currentRotation) > rotationLimit)
        {
            rotationSpeed = maxRotationSpeed * -1 * (currentRotation / Mathf.Abs(currentRotation)) / 360;
        }

        this.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, currentRotation));
    }
}

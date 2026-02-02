using UnityEngine;

public class Rotation : MonoBehaviour
{

    [SerializeField] private float rotationSpeed;
    [SerializeField] private Vector3 rotationDirection = new Vector3();


    void Update()
    {
        transform.Rotate(rotationSpeed * rotationDirection * Time.deltaTime);
    }
}

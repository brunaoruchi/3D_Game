using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptCamera : MonoBehaviour
{
    private Quaternion rotOriginal;
    public float velRot = 100;
    private float contRot = 0;
    // Start is called before the first frame update
    void Start()
    {
        rotOriginal = transform.localRotation;

    }

    // Update is called once per frame
    void Update()
    {
        contRot += Input.GetAxis("Mouse Y");
        contRot = Mathf.Clamp(contRot, -50, 50);
        Quaternion cimabaixo = Quaternion.AngleAxis(contRot, Vector3.left);
        transform.localRotation = rotOriginal * cimabaixo;
    }
}

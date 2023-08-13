using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField]
    private Transform targetTransform;
    [SerializeField]
    private float fRotationSpeed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (targetTransform)
        {
            Vector3 eulerAngles = targetTransform.eulerAngles;
            targetTransform.eulerAngles = new Vector3(eulerAngles.x, eulerAngles.y, eulerAngles.z - fRotationSpeed * Time.deltaTime);
        }
    }
}

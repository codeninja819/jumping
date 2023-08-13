using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public static SmoothFollow instance = null;
    public Transform targetTransform;
    public float f_Distance = 3;
    public float f_Height = 3;
    public Vector3 offset;
    public float f_CameraTargetOffset = .2f;
    public float f_DistanceDumping = 0.1f;
    public float f_RotationDumping = 30f;
    Vector3 velocity;
    Vector3 initPos;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        initPos = transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!JumpPlayer.instance.isPlaying())
        {
            transform.position = initPos;
            transform.rotation = Quaternion.identity;
            return;
        }
            

        Vector3 position = targetTransform.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, position, ref velocity, f_DistanceDumping);
        
        Vector3 direction = targetTransform.position + Vector3.down * f_CameraTargetOffset - position;
        direction.Normalize();
        Quaternion to = Quaternion.LookRotation(direction);
        transform.rotation = to;
        //transform.rotation = Quaternion.Lerp(transform.rotation, to, f_RotationDumping * Time.deltaTime); 
    }
}

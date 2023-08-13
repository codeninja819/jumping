using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlayer : MonoBehaviour
{
    public static JumpPlayer instance = null;
    public bool testMode = false;
    public float f_InitTrust;
    public float f_Trust;
    
    public float f_DetectDuration = .1f;    
    public float f_MinRotateSpeed = 120f;
    public float f_MaxRotateSpeed = 360f;    

    public Material trampolineActiveMaterial;
    public Material trampolineNormalMaterial;
    public Material pitActiveMaterial;
    public Material pitNormalMaterial;

    public Transform camTarget;
    float f_DetectTime = 0;
    float f_DetectDistanceTime = 1f;
    float f_CurRotate = 0;
    float f_RotateSpeed = 10f;

    [SerializeField]
    float f_JumpDelayLimit = 1;
    float f_JumpDelay = 0;
    bool jumpState = false;

    Rigidbody rigidBody;
    Vector3 jumpDirection;
    Vector3 initPos;
    Vector3 rotateAxis;
    Vector3 preHeroPosition;
    IEnumerator enmerator = null;

    float preDistance = 0;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();        
        initPos = transform.position;
        rotateAxis = Quaternion.Euler(0, 0, Random.Range(0, -120)) * Vector3.up;
        f_RotateSpeed = Random.Range(f_MinRotateSpeed, f_MaxRotateSpeed);
    }

    public void Init(float trust)
    {
        f_InitTrust = f_Trust = trust;
        rigidBody.isKinematic = false;
        preDistance = 0;
        f_DetectDistanceTime = 1;
        jumpDirection = Quaternion.Euler(-HeightScreen.f_JumpAngle, 0, 0) * Vector3.forward;

        Camera.main.clearFlags = CameraClearFlags.Color;
    }

    public void Reset()
    {
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
        gameObject.SetActive(false);
        gameObject.SetActive(true);
        rigidBody.isKinematic = true;
        transform.position = initPos;
        f_DetectTime = 0;
        transform.rotation = Quaternion.identity;
        preHeroPosition = transform.position;
        jumpState = false;
        f_JumpDelay = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (rigidBody.isKinematic)
            return;

        if (f_DetectTime > 0)
            f_DetectTime -= Time.deltaTime;

        if (f_Trust < 150)
            Time.timeScale = 1;
        /*else if (preHeroPosition.y < transform.position.y)
            Time.timeScale = .7f;
        else
            Time.timeScale = 1.3f;*/

        if (MainScreen.instance)
        {
            float distance = Vector3.Distance(transform.position, initPos);
            MainScreen.instance.RefreshDistance(distance);

            if (f_DetectDistanceTime > 0)
                f_DetectDistanceTime -= Time.deltaTime;
            else
            {
                if (preDistance == distance)
                {
                    rigidBody.isKinematic = true;                    
                    Network.instance.JumpEndRequest(distance);
                    Camera.main.clearFlags = CameraClearFlags.Skybox;
                }
                preDistance = distance;
                f_DetectDistanceTime = .2f;
            }

            if (jumpState)
            {
                f_JumpDelay += Time.deltaTime;
                if(f_JumpDelay > f_JumpDelayLimit)
                {
                    rigidBody.isKinematic = true;
                    Network.instance.JumpEndRequest(distance);
                    Camera.main.clearFlags = CameraClearFlags.Skybox;
                }
            }
            else
                f_JumpDelay = 0;
        }
            

        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, float.PositiveInfinity, 1 << LayerMask.NameToLayer("Entrance")))
        //if (Physics.Raycast(transform.position, Vector3.down, out hit, float.PositiveInfinity))
        {
            SubRoadEntrance entrance = hit.collider.gameObject.GetComponent<SubRoadEntrance>();
            entrance.ProcessEndlessRoad();
            if (hit.collider.tag == "Entrance")
            {
                
            }else if(hit.collider.tag == "Ground")
            {
                //float height = Vector3.Distance(transform.position, hit.point);
                //camTarget.position = transform.position + new Vector3(0, height / 1;
            }
            
        }
        preHeroPosition = transform.position;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (f_DetectTime > 0)
            return;

        string colliderTag = collision.collider.tag;

        if (colliderTag == "Ground")
            f_Trust /= Mathf.Sqrt(2);
        else if (colliderTag == "Trampoline")
        {
            f_Trust *= Mathf.Sqrt(2);
            MeshRenderer renderer = collision.collider.gameObject.GetComponent<MeshRenderer>();
            renderer.sharedMaterial = trampolineActiveMaterial;
        }
        else if(colliderTag == "Pit")
        {
            MeshRenderer renderer = collision.collider.gameObject.GetComponent<MeshRenderer>();
            renderer.sharedMaterial = pitActiveMaterial;
            f_Trust /= 2;
        }
        else if (colliderTag == "Obstacle")
            f_Trust /= 4;


        //transform.position = new Vector3(initPos.x, transform.position.y, transform.position.z);
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
        if (f_Trust < 500)
        {
            //f_Trust = 0;
            GameManager.instance.rootEnvmanager.arenaRoad.SetActive(false);
            //rigidBody.isKinematic = true;
            //rigidBody.AddForce(Vector3.down * 1000);
        }
        else
        {
            //rigidBody.velocity = Vector3.zero;
            //rigidBody.angularVelocity = Vector3.zero;
            rigidBody.AddForce(jumpDirection * f_Trust);

            rotateAxis = Quaternion.Euler(0, 0, Random.Range(-90,0)) * Vector3.up;
            f_RotateSpeed = Random.Range(f_MinRotateSpeed, f_MaxRotateSpeed) * f_Trust / f_InitTrust;
            rigidBody.AddTorque(rotateAxis * f_RotateSpeed);
        }
        

        Debug.Log(colliderTag + " : " + Vector3.Distance(initPos, transform.position).ToString() + " : " + f_Trust.ToString());
        f_DetectTime = f_DetectDuration;
        jumpState = true;
        Time.timeScale = 1f;

    }

    public void OnCollisionExit(Collision collision)
    {
        //Time.timeScale = testMode ? 1.6f : .8f;
        if (collision.collider.tag == "Trampoline")
        {
            MeshRenderer renderer = collision.collider.gameObject.GetComponent<MeshRenderer>();
            if(enmerator != null)
                StopCoroutine(enmerator);
            enmerator = ResetColor(renderer, trampolineNormalMaterial);
            StartCoroutine(enmerator);
        }else if (collision.collider.tag == "Pit")
        {
            MeshRenderer renderer = collision.collider.gameObject.GetComponent<MeshRenderer>();
            if (enmerator != null)
                StopCoroutine(enmerator);
            enmerator = ResetColor(renderer, pitNormalMaterial);
            StartCoroutine(enmerator);
        }
        jumpState = false;
    }

    IEnumerator ResetColor(MeshRenderer renderer, Material nomalMaterial)
    {
        yield return new WaitForSeconds(.7f);
        renderer.sharedMaterial = nomalMaterial;
    }

    public bool isPlaying()
    { return !rigidBody.isKinematic; }
}

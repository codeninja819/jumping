using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public static EnvironmentManager instance = null;
    public Transform startTransform;
    public float minDistance = 20;
    public float maxDistance = 40;
    public float f_LimitDistance = 300;
    public SubRoadEntrance firestSubRoadEntrance;
    public GameObject arenaRoad;
    [HideInInspector]
    public List<GameObject> newSubEntranceList = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> trampolineList = new List<GameObject>();

    private void Awake()
    {
        instance = this;
    }

    public void Init()
    {
        for (int i = 0; i < newSubEntranceList.Count; i++)
            Destroy(newSubEntranceList[i].gameObject);

        newSubEntranceList.Clear();

        if(firestSubRoadEntrance)
            firestSubRoadEntrance.isPerformed = false;

        if(arenaRoad)
            arenaRoad.SetActive(true);

        InitiateTrampolines();
    }
    // Start is called before the first frame update
    void Start()
    {
        InitiateTrampolines();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitiateTrampolines()
    {
        foreach (GameObject trampoline in trampolineList)
            Destroy(trampoline);

        trampolineList.Clear();

        GameObject trampolineObj = Resources.Load("Prefabs/Trampoline") as GameObject;
        GameObject pitObj = Resources.Load("Prefabs/Pit") as GameObject;
        float addDistance = 0;
        for (; addDistance < f_LimitDistance;)
        {
            float zPosition = (float)startTransform.localPosition.z + addDistance;
            GameObject obj = Instantiate(Random.Range(1, 10000) % 2 == 0 || JumpPlayer.instance.testMode ? trampolineObj : pitObj) as GameObject;
            obj.transform.parent = transform;
            obj.transform.localPosition = new Vector3(startTransform.localPosition.x, startTransform.localPosition.y, zPosition);
            trampolineList.Add(obj);

            float distanceRate = f_LimitDistance / (f_LimitDistance - addDistance);
            distanceRate = 1;
            addDistance += Random.Range(minDistance * distanceRate, maxDistance * distanceRate);
        }
    }
}

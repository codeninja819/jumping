using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubRoadEntrance : MonoBehaviour
{
    public SubRoadEntrance preSubRoadEntrance = null;
    [HideInInspector]
    public bool isPerformed = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        return;
        if(other.tag == "Player")
        {
            GameObject subRoadPrefab = Resources.Load("Prefabs/SubRoad") as GameObject;
            GameObject subRoadObj = Instantiate(subRoadPrefab);
            subRoadObj.transform.parent = transform.parent.parent;
            Vector3 prePos = transform.parent.position;
            subRoadObj.transform.position = new Vector3(prePos.x, prePos.y, prePos.z + 500);
            subRoadObj.GetComponentInChildren<SubRoadEntrance>().preSubRoadEntrance = this;
            //if (preSubRoadEntrance != null && preSubRoadEntrance.preSubRoadEntrance != null)
            //    Destroy(preSubRoadEntrance.preSubRoadEntrance.transform.parent.gameObject);
        }
    }

    public void ProcessEndlessRoad()
    {
        if (isPerformed)
            return;

        isPerformed = true;
        GameObject subRoadPrefab = Resources.Load("Prefabs/SubRoad") as GameObject;
        GameObject subRoadObj = Instantiate(subRoadPrefab);
        subRoadObj.transform.parent = transform.parent.parent;
        Vector3 prePos = transform.parent.position;
        subRoadObj.transform.position = new Vector3(prePos.x, prePos.y, prePos.z + 500);
        subRoadObj.GetComponentInChildren<SubRoadEntrance>().preSubRoadEntrance = this;
        GameManager.instance.rootEnvmanager.newSubEntranceList.Add(subRoadObj);

        //if (preSubRoadEntrance != null && preSubRoadEntrance.preSubRoadEntrance != null)
        //    Destroy(preSubRoadEntrance.preSubRoadEntrance.transform.parent.gameObject);
    }
}

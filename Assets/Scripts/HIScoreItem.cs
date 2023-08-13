using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HIScoreItem : MonoBehaviour
{
    [SerializeField]
    UILabel gradeLabel;
    [SerializeField]
    UILabel nameLabel;
    [SerializeField]
    UILabel hiScoreLabel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(int index, string name, int highScore, bool highLight)
    {
        gradeLabel.text = string.Format("{0}.", index);
        nameLabel.text = name;
        hiScoreLabel.text = highScore.ToString();      
        
        if(highLight)
        {
            gradeLabel.color = Color.black;
            nameLabel.color = Color.black;
            hiScoreLabel.color = Color.black;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    public int hits = 0;
    public float nextFire = 0.0F;
    public float fireRate = 0.5F;
    public bool isHit = false;
    public Transform shootLoc;
    public GameObject scoreLabel;
    public TextMeshProUGUI playerScore;

    public int Hits
    {
        get
        {
            return this.hits;
        }
        set
        {
            this.hits = value; 
            playerScore.SetText(this.hits.ToString());
        }
    }
    private void Awake()
    {
        playerScore = scoreLabel.GetComponent<TextMeshProUGUI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //isHit == false &&
        if ( other.tag == "bullet") {
            other.gameObject.transform.position = new Vector3(-1000, -1000, 0);
            //isHit = true;
            Hits += 1;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //isHit = false;
    }
}

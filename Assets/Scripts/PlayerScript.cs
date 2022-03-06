using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private int hits = 0;
    public float nextFire = 0.0F;
    public float fireRate = 0.5F;
    public bool isHit = false;
    public Transform shootLoc;
    public GameObject scoreLabel;
    public Text playerScore;

    public int Hits
    {
        get
        {
            return this.hits;
        }
        set
        {
            this.hits = value; 
            playerScore.text = this.hits.ToString();
        }
    }
    private void Awake()
    {
        playerScore = scoreLabel.GetComponent<Text>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isHit == false && other.tag == "bullet") {
            other.gameObject.transform.position = new Vector3(-1000, -1000, 0);
            isHit = true;
            hits += 1;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isHit = false;
    }
}

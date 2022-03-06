using UnityEngine;

[System.Serializable]
public class Sendable
{
    public int id;
    public float x, y;
    public Vector3[] bulletPos;
    public Vector3[] bulletVel;
    public int hitsP1;
    public int hitsP2;
    public bool isHit;
    public long time;
    public int framenumber;
}

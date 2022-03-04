using UnityEngine;

[System.Serializable]
public class Sendable
{
    public int id;
    public float x, y;
    public Vector3[] bulletPos;
    public Vector3[] bulletVel;
    public long time;
    public int framenumber;
}

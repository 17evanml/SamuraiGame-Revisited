using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 avg = Vector2.zero;
        int count = 0;
        foreach(AttackMoves a in GameManager.instance.attackAnimators)
        {
            avg += (Vector2)a.gameObject.transform.position;
            count++;
        }
        avg /= count;
        Vector2.Lerp(Camera.main.transform.position, avg, Time.deltaTime);
    }
}

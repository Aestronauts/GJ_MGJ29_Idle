using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Dice_Projectile : MonoBehaviour
{
    public Transform Monster;
    public Vector3 Speed;

    public void Initiate(GameObject g, Transform t)
    {
        Instantiate(g, transform);
        Monster = t;
        transform.position += new Vector3(Random.Range(2, 2), Random.Range(0, 1), Random.Range(2, 2));
        Speed = (Monster.position+new Vector3(0,2,0) - transform.position)/150;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Speed;
        transform.Rotate(new Vector3(15, 30, 45) * 10 * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this);
    }
}

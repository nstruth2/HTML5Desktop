using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game6PlayerController : MonoBehaviour
{
    public float dodgeSpeed;
    float xInput;
    public float maxX;
    void Start()
    {
    }
    void Update()
    {
        xInput = Input.GetAxis("Horizontal");
        transform.Translate(xInput*dodgeSpeed*Time.deltaTime,0,0);
        float limitedX = Mathf.Clamp(transform.position.x,-maxX,maxX);
        transform.position = new Vector3(limitedX,transform.position.y,transform.position.z);
    }
}

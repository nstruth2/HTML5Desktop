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
        TouchInput();
        transform.Translate(xInput*dodgeSpeed*Time.deltaTime,0,0);
        float limitedX = Mathf.Clamp(transform.position.x,-maxX,maxX);
        transform.position = new Vector3(limitedX,transform.position.y,transform.position.z);
    }
    void TouchInput()
    {
        if(Input.GetMouseButton(0))
        {
            Vector3 touchPos = Input.mousePosition;;
            float middle = Screen.width / 2;
            if(touchPos.x<middle)
            {
                xInput = -1;
            }
            else if(touchPos.x>middle)
            {
                xInput = 1;

            }
        }
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            Game6GameManager.instance.Restart();
        }
    }
}

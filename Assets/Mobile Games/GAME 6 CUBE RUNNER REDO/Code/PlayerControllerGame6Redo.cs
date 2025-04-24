using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerGame6Redo : MonoBehaviour
{

    public float dodgeSpeed;
    public float maxX;

    float xInput;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        xInput = Input.GetAxis("Horizontal");

        transform.Translate(xInput * dodgeSpeed * Time.deltaTime, 0, 0);

        float limitedX = Mathf.Clamp(transform.position.x, -maxX, maxX);
        transform.position = new Vector3(limitedX, transform.position.y, transform.position.z);
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Enemy")
        {
            GameManagerGame6Redo.instance.Restart();
        }
    }

}

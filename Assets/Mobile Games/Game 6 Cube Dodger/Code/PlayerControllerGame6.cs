using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerGame6 : MonoBehaviour
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

        TouchInput();

        transform.Translate(xInput * dodgeSpeed * Time.deltaTime, 0, 0);

        float limitedX = Mathf.Clamp(transform.position.x, -maxX, maxX);
        transform.position = new Vector3(limitedX, transform.position.y, transform.position.z);
    }

    void TouchInput()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 touchPos = Input.mousePosition;
            touchPos.z = Camera.main.WorldToScreenPoint(transform.position).z; // Maintain Z depth

            Vector3 worldPos = Camera.main.ScreenToWorldPoint(touchPos);

            // Snap to the touch X position instantly, clamped within maxX range
            float clampedX = Mathf.Clamp(worldPos.x, -maxX, maxX);
            transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);

            // Prevent keyboard input from interfering
            xInput = 0;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Enemy")
        {
            GameManagerGame6.instance.Restart();
        }
    }

}

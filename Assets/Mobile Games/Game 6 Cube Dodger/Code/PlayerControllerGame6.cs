using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerGame6 : MonoBehaviour
{
    public float maxX;

    void Update()
    {
        FollowMouseX();
    }

    void FollowMouseX()
    {
        // Get mouse position in screen space and convert to world space
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z; // Maintain Z depth

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        // Clamp the X position within maxX bounds
        float clampedX = Mathf.Clamp(mouseWorldPos.x, -maxX, maxX);

        // Set the player's position (only X changes)
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            GameManagerGame6.instance.GameOver();
            Destroy(gameObject);
        }
    }
}

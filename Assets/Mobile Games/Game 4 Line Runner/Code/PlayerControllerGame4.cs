using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerControllerGame4 : MonoBehaviour
{
    public GameObject particle;
    float playerYPos;
    // Start is called before the first frame update
    void Start()
    {
        playerYPos = transform.position.y;
    }
    // Update is called once per frame
    void Update()
    {
        if(GameManagerGame4.instance.gameStarted)
        {
            if(!particle.activeInHierarchy)
            {
                particle.SetActive(true);
            }
            if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                PositionSwitch();
            }
        }
    }
    void PositionSwitch()
    {
        //player position siwtch
        playerYPos = -playerYPos;
        transform.position = new Vector3(transform.position.x, playerYPos, transform.position.z);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Obstacle")
        {
            //restart level
            //SceneManager.LoadScene("Line Runner");
            //GameManagerGame4.instance.GameOver();
            GameManagerGame4.instance.UpdateLives();
            GameManagerGame4.instance.Shake();
        }
    }
}

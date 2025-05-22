using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CarFrom3GamesPackGame2 : MonoBehaviour
{
    [SerializeField] private float speed = 30f;
    [SerializeField] private float speedGainPerSecond = 0.2f;
    [SerializeField] private float turnSpeed =200f;
    private int steerValue;
    private ScoreSystemFrom3GamesPackGame2 scoreSystem;
    // Update is called once per frame
    void Update()
    {
    // Keyboard steering
    if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
    {
        Steer(-1); // Steer left
    }
    else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
    {
        Steer(1); // Steer right
    }
    else
    {
        Steer(0); // Go straight
    }
        speed += speedGainPerSecond * Time.deltaTime;
        transform.Rotate(0f, steerValue * turnSpeed * Time.deltaTime, 0f);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    public void Steer(int value)
    {
        steerValue = value;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Obstacle"))
        {
            scoreSystem = FindObjectOfType<ScoreSystemFrom3GamesPackGame2>();
            if (scoreSystem != null)
            {
                scoreSystem.OnGameEnd();  // Trigger OnGameEnd in ScoreSystem
            }
        }
    }
}
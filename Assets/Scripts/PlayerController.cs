using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Values for acceleration
    private float thrust = 10f;
    private float torque = 0.5f;

    // Max speed at collisions
    private float maxSpeed = 4.0f;

    // Duration of Alert messages
    private float msgDuration = 2.0f;

    // Value of stop duration
    private float stopDuration = 1.0f;

    // reference to air and gas gauges
    private Slider airGauge;
    private Slider gasGauge;
    private bool outOfGas = false;

    // Reference to the Linear and Rotational speed displays
    private InputField linearSpeed;
    private InputField rotSpeed;

    // Air consumption per second
    public float breath = 1f;
    // Gas consumption per jet
    public float jet = 0.000001f;

    // speeds of the player (translation and rotation)
    public float currentVelocity;
    public float currentAngularVelocity;
    // relative speed of player to target
    public float projSpeed;

    // Reference to the player Rigidbody
    private Rigidbody playerRB;

    // Reference to the target Rigidbody
    public Rigidbody targetRB;

    public bool gameOver = false;
    public Text goodMessage, badMessage;

    private AudioSource playerSound;
    public AudioClip jetSound, stopSound;
    private string currentClip;

    void Start()
    {
        // Get reference to Player Rigidbody
        playerRB = GetComponent<Rigidbody>();
        // Get references to UI elements
        airGauge = GameObject.Find("Air Gauge").GetComponent<Slider>();
        gasGauge = GameObject.Find("Gas Gauge").GetComponent<Slider>();
        linearSpeed = GameObject.Find("Speed Field").GetComponent<InputField>();
        rotSpeed = GameObject.Find("Rot Field").GetComponent<InputField>();
        playerSound = GetComponent<AudioSource>();
        // Get reference to Target rigidbody
        targetRB = GameObject.Find("ISS").GetComponent<Rigidbody>();
        // Not needed currently but may be useful if this flag is used
        // instead of the gameOver flag in the scene
        PersistentSettings.Instance.playerLoose = false;
    }

    // Update is called once per frame
    void Update()
    {
        // if game is ended no more movement or air and gas consumption
        if (!gameOver)
        {
            // player breathes constantly when 
            consumeAir();

            // if there is no more gas no action is feasible
            if (!outOfGas)
            {
                // Move the Player forward
                if (Input.GetKey(PersistentSettings.Instance.forwardKey))
                {
                    playerRB.AddForce(transform.forward * thrust, ForceMode.Force);
                    consumeGas(1, jetSound);
                }

                // Move the Player backward
                if (Input.GetKey(PersistentSettings.Instance.backwardKey))
                {
                    playerRB.AddForce(-transform.forward * thrust, ForceMode.Force);
                    consumeGas(1, jetSound);
                }

                // Move the Player Left
                if (Input.GetKey(PersistentSettings.Instance.leftKey))
                {
                    playerRB.AddForce(-transform.right * thrust, ForceMode.Force);
                    consumeGas(1, jetSound);
                }

                // Move the Player Right
                if (Input.GetKey(PersistentSettings.Instance.rightKey))
                {
                    playerRB.AddForce(transform.right * thrust, ForceMode.Force);
                    consumeGas(1, jetSound);
                }

                // Rotate the Player to the left 
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    playerRB.AddTorque(-transform.up * torque, ForceMode.Force);
                    consumeGas(1, jetSound);
                }

                // Turn the Player to the right
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    playerRB.AddTorque(transform.up * torque, ForceMode.Force);
                    consumeGas(1, jetSound);
                }

                // Move the Player up
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    playerRB.AddForce(transform.up * thrust, ForceMode.Force);
                    consumeGas(1, jetSound);
                }

                // Move the Player down
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    playerRB.AddForce(-transform.up * thrust, ForceMode.Force);
                    consumeGas(1, jetSound);
                }

                // stores current speeds
                // (needed for gas consumption when stopping all movements)
                currentVelocity = playerRB.velocity.magnitude; 
                currentAngularVelocity = playerRB.angularVelocity.magnitude;
                // Computation of relative velocity to Target
                // First get the line between Target and Player
                Vector3 dir = (targetRB.position - playerRB.position).normalized;
                // Then project the Target Speed and the Player Speed Vectors
                // onto the line 'dir' and subtract them
                projSpeed = Vector3.Dot(dir, playerRB.velocity) - Vector3.Dot(dir, targetRB.velocity);

                // displays speeds
                linearSpeed.text = string.Format("{0:N1} m/s", projSpeed);
                rotSpeed.text = string.Format("{0:N1} °/s",
                    currentAngularVelocity / Mathf.PI * 180);

                // Stop all movements if Player is moving
                if (Input.GetKeyDown(KeyCode.Space)
                    && ((playerRB.velocity.magnitude > 0)
                    || (playerRB.angularVelocity.magnitude > 0)))
                {
                    // gas consumption depends on linear and angular speeds
                    consumeGas((currentVelocity +
                        currentAngularVelocity) * jet, stopSound);
                    StopPlayer();
                }
            }
        }
    }

    private void consumeAir()
    {
        // TODO: ** need to add panic breath, and/or air leaks **
        airGauge.value -= breath * Time.deltaTime;
        // check if there is still air in the extravehicular suit
        if (airGauge.value < 0.1)
        {
            PersistentSettings.Instance.playerLoose = true;
            PersistentSettings.Instance.endMessage = "You have run out of air! You are dead.";
            SceneManager.LoadScene("End");
        }
    }

    private void consumeGas(float qty, AudioClip clip)
    {
        gasGauge.value -= jet * qty / 5 * Time.deltaTime;

        // do not play sound if it is currently playing
        // except if it is a diffferent one
        if (!playerSound.isPlaying || currentClip != clip.name)
        {
            playerSound.PlayOneShot(clip, 1.0f);
            currentClip = clip.name;
        }
        // check if there is still gas in the extravehicular suit
        if (gasGauge.value < 0.1)
        {
            outOfGas = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Test collision with obstacles and Target
        // If speed too high --> crash and death
        // else if target --> win
        // else simple collision

        if (projSpeed > maxSpeed)
        {
            gameOver = true;

            // ** Need to add sound **
            badMessage.text = "CRASH";
            PersistentSettings.Instance.playerLoose = true;
            PersistentSettings.Instance.endMessage = "Your speed was too high and you have crashed! You are dead.";
            SceneManager.LoadScene("End");
        }
        else if (collision.gameObject.CompareTag("Target"))
        {
            // Player wins
            // ** Need to add sound **
            goodMessage.text = "WIN";
            PersistentSettings.Instance.playerLoose = false;

            // Check if high score performed
            if (PersistentSettings.Instance.winTime < Mathf.Max(PersistentSettings.Instance.hiScores.time))
            {
                // Case Yes : open scene Hi score to enter the name of the player
                SceneManager.LoadScene("Hi Score");
            }
            else
            {
                // Go directly to End Scene
                SceneManager.LoadScene("End");
            }
        }
        else  // simple collision
        {
            // TODO: ** Need to add random gas leak and/or air leak **
            // TODO: ** Need to add sound **
            badMessage.text = "Collision";
            Invoke("ClearMessage", msgDuration);
        }
    }

    private void StopPlayer()
    {
        StartCoroutine(StopPlayerGradually(stopDuration));
    }

    // Coroutine to gradually stop the player
    IEnumerator StopPlayerGradually(float duration)
    {
        Vector3 initialVelocity = playerRB.velocity;
        Vector3 initialAngularVelocity = playerRB.angularVelocity;
        float startTime = Time.time;

        // Reduce both speeds linearly with time
        while (Time.time < startTime + duration)
        {
            float ratio = 1 - ((Time.time - startTime) / duration);
            playerRB.velocity = initialVelocity * ratio;
            playerRB.angularVelocity = initialAngularVelocity * ratio;
            yield return null;
        }

        // Make sure speeds are strictly null, to avoid float calculus residues
        playerRB.velocity = new Vector3(0, 0, 0);
        playerRB.angularVelocity = new Vector3(0, 0, 0);
    }

    private void ClearMessage()
    {
        badMessage.text = "";
    }
}

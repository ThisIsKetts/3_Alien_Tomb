using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;

    Rigidbody rigidBody; // Naming of Rigidbody
    AudioSource audioSource; // Naming of Audiosource
    

	// Use this for initialization
	void Start ()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();
    }

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("OK"); //TODO remove/add working code
                break;

            case "Fuel":
                print("Fuel");//TODO remove/add working code
                break;
            default:
                print("Dead");//TODO remove/add working code
                //Kill player/reload level
                break;
                


        }
    }

    private void Thrust()
    {
        float addThrust = mainThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.W)) // Can thrust while rotating
        {
            rigidBody.AddRelativeForce(Vector3.up * addThrust);
            if (audioSource.isPlaying == false) // So audio doesn't play every frame
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true; // take manual control of rotation

        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; // resume physics control of rotation

    }

 

}

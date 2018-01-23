using UnityEngine;
using UnityEngine.SceneManagement;

//TODO Fix lighting bug

public class Rocket : MonoBehaviour {

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;

    Rigidbody rigidBody; // Naming of Rigidbody
    AudioSource audioSource; // Naming of Audiosource

    enum State { Alive, Dying, Transcending }
    State state = State.Alive;
    

	// Use this for initialization
	void Start ()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}

    // Update is called once per frame
    void Update()
    {
        // Somewhere stop sound on death
        if (state == State.Alive)
        {
            Thrust();
            Rotate();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) { return; } // Ignore collisions when dead

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                //Do Nothing
                break;
            case "Finish":
                state = State.Transcending;
                Invoke("LoadNextLevel", 1f); // Parameterise time
                break;
            default:
                print("Hit Something Deadly");
                state = State.Dying;
                Invoke( "LoadFirstLevel", 3f); // Parameterise time
                break;



        }
    }

   

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1); // Allow for more 2 levels
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void Thrust()
    {
        float addThrust = mainThrust * Time.deltaTime;


        if (Input.GetKey(KeyCode.W)) // Can thrust while rotating
        {
            rigidBody.AddRelativeForce(Vector3.up * addThrust);
            if (!audioSource.isPlaying) // So audio doesn't play every frame
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

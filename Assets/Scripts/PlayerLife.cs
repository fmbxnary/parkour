using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    public static int nextLevelIndex = 1;
    public static int points = 0;
    public AudioSource dieSound;
    bool dead = false;
    private void Update()
    {
        if (transform.position.y < -1f && !dead)
        {
            Die();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy Body"))
        {
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<PlayerMovement>().enabled = false;
            Die();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish Flag"))
        {
            LoadNextLevel();
        }
        else if (other.gameObject.CompareTag("Collectable"))
        {
            Destroy(other.gameObject);
            points += 100;
        }
        else if (other.gameObject.CompareTag("Axe"))
        {
            Die();
        }
    }

    void Die()
    {
        points = 0; // Reset the points here, when player dies
        Invoke(nameof(ReloadLevel), 1.3f);
        dead = true;
        dieSound.Play();
    }

    void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void LoadNextLevel()
    {
        if (nextLevelIndex < SceneManager.sceneCountInBuildSettings)
        {
            nextLevelIndex++; // Increase the nextLevelIndex for the next scene load
            SceneManager.LoadScene(nextLevelIndex);
        }
        else
        {
            Debug.LogError("Next level index is out of bounds!");
        }
    }
}

using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerLife : MonoBehaviour
{
    public static int nextLevelIndex = 1;
    public static int points = 0;
    public AudioSource dieSound;
    public AudioSource Collect;
    bool dead = false;
    public GameObject winScreen;
    public Text pointsText;
    private float colorChangeTime = 2f;
    private void Update()
    {
        if (transform.position.y < -1f && !dead)
        {
            Die();
        }
    }

    IEnumerator ChangeTextColor()
    {
        while (true)
        {
            float t = 0;
            while (t < colorChangeTime)
            {
                float hue = t / colorChangeTime;
                pointsText.color = Color.HSVToRGB(hue, 1f, 1f);

                yield return null;

                t += Time.deltaTime;
            }
        }
    }

    void StartColorChange()
    {
        StartCoroutine(ChangeTextColor());
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
            Collect.Play();
        }
        else if (other.gameObject.CompareTag("Axe"))
        {
            Die();
        }
        else if (other.CompareTag("GameOver"))
        {
            
                winScreen.SetActive(true);
                pointsText.text = "Points: " + points;
                StartColorChange();

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
        if (nextLevelIndex <= SceneManager.sceneCountInBuildSettings)
        {
            
            SceneManager.LoadScene(nextLevelIndex+1);
            nextLevelIndex++;
        }
    }
}

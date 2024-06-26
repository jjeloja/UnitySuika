using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollideCreateNew : MonoBehaviour
{
    public GameObject angel1, angel2, angel3, angel4, angel5, angel6, angel7, angel8, angel9, angel10;
    private GameObject[] angels;
    private string[] angelTags;
    private GameObject currentSonny;
    private int sonnyIndex;
    public static bool destroyed;
    public static bool createNew;
    private int[] angelScores;
    public static int gameScore;
    public static int highScore;
    public static Text scoreText, highScoreText;
    public AudioClip collideAudio;
    private AudioSource audioSource;


    void Start()
    {
        audioSource = GameObject.Find("WingsLauncher").GetComponent<AudioSource>();
        /** instantiates angels as an array of all the provided sonnys */
        /** instantiates angelTags as an array of all the sonny's tags */
        if (angel1 != null && angel2 != null && angel3 != null && angel4 != null & angel5 != null && angel6 != null && angel7 != null && angel8 != null && angel9 != null && angel10 != null)
        {
            angels = new GameObject[10] { angel1, angel2, angel3, angel4, angel5, angel6, angel7, angel8, angel9, angel10 };
            angelTags = new string[10] { angel1.gameObject.tag, angel2.gameObject.tag, angel3.gameObject.tag, angel4.gameObject.tag, angel5.gameObject.tag, angel6.gameObject.tag, angel7.gameObject.tag, angel8.gameObject.tag, angel9.gameObject.tag, angel10.gameObject.tag };
            angelScores = new int[10] { 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024 };
        }

        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        highScoreText = GameObject.Find("HighScoreText").GetComponent<Text>();
        currentSonny = this.gameObject;
        sonnyIndex = System.Array.IndexOf(angelTags, currentSonny.gameObject.tag);
        destroyed = false;
        createNew = false;
        scoreText.text = "Score\n" + gameScore;
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "High Score\n" + highScore;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        /** checks if currSonny has collided with sonny angel of the same type
            and destroys both currSonny and collidedSonny if so */
        if (angelTags[sonnyIndex] == other.gameObject.tag)
        {
            // ensures only one new sonny angel is created in place instead of two
            if (!createNew)
            {
                GameObject newSonny = angels[System.Array.IndexOf(angelTags, currentSonny.gameObject.tag) + 1];

                // updates position of new tier of Sonny to ensure no colliding with container walls
                Vector3 newPos = currentSonny.gameObject.transform.position;
                if (newSonny == null)
                {
                    if (newPos.x < 345)
                    {
                        newPos.x = 345;
                    }
                    else if (newPos.x > 605)
                    {
                        newPos.x = 605;
                    }
                }
                else if (newPos.x < (int)(newSonny.GetComponent<Renderer>().bounds.size.x / 2f) + GameObject.Find("LeftEdge").transform.position.x)
                {
                    newPos.x = (int)(newSonny.GetComponent<Renderer>().bounds.size.x / 2f) + GameObject.Find("LeftEdge").transform.position.x;
                }
                else if (newPos.x > GameObject.Find("RightEdge").transform.position.x - (int)(newSonny.GetComponent<Renderer>().bounds.size.x / 2f))
                {
                    newPos.x = (int)GameObject.Find("RightEdge").transform.position.x - (int)(newSonny.GetComponent<Renderer>().bounds.size.x / 2f);
                }

                newSonny = Instantiate(newSonny, newPos, transform.rotation);
                newSonny.transform.SetParent(GameObject.Find("Canvas").transform);
                newSonny.gameObject.GetComponent<Collider2D>().enabled = !newSonny.gameObject.GetComponent<Collider2D>().enabled;
                newSonny.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                createNew = true;
                audioSource.PlayOneShot(collideAudio, .7f);
                gameScore += angelScores[sonnyIndex + 1];
                if (gameScore > highScore)
                {
                    highScore = gameScore;
                    PlayerPrefs.SetInt("HighScore", gameScore);
                    PlayerPrefs.Save();
                    highScoreText.text = "High Score:\n" + highScore;

                }
                scoreText.text = "Score:\n" + gameScore;
            }
            else
            {
                createNew = false;
            }

            Destroy(this.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MidiPlayerTK;

public class GameManager : MonoBehaviour
{
    public AudioSource theMusic;
    public AudioSource HitSound;

    public bool startPlaying;

    public BeatScroller theBS;

    public static GameManager instance;

    public int currentScore;
    public int scorePerNote = 50;
    public int scorePerGoodNote = 100;
    public int scorePerPerfectNote = 300;

    public Text scoreText;
    public Text multiText;
    public Text currentPercentText;

    public int currentMultiplier;
    public int multiplierTracker;
    public int[] multiplierThresholds;

    public Texture2D cursorTexture;
    private Vector2 hotspot;

    public float totalNotes;
    public float currentPercent = 0;
    public float normalHits;
    public float goodHits;
    public float perfectHits;
    public float missedHits;
    public float currentNote;
    public float totalPercent;

    public GameObject resultsScreen;
    public Text percentHitText, normalsText, GoodsText, perfectsText, missesText, rankText, finalScoreText;

    public Image healthBar;
    public float healthSpeed;

    public GameObject failScreen;

    public bool fail;

    public GameObject midiplayer;
    public GameObject midireader;

    public static float speed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        scoreText.text = "Score 0";
        currentMultiplier = 1;

        totalNotes = FindObjectsOfType<NoteObject>().Length;

        hotspot.x = cursorTexture.width / 2;
        hotspot.y = cursorTexture.height / 2;

        Cursor.SetCursor(cursorTexture, hotspot, CursorMode.Auto);

        healthSpeed = 0.01f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!startPlaying)
        {
            if (Input.anyKeyDown)
            {
                startPlaying = true;
                theBS.hasStarted = true;

                theMusic.Play();
                fail = false;
            }
        }
        else
        {
            if(currentNote > 0)
            {
                currentPercent = totalPercent / currentNote;
                currentPercentText.text = currentPercent.ToString("F2") + "%";
            }

            if (theMusic.isPlaying && !resultsScreen.activeInHierarchy)
            {
                if(healthBar.transform.localScale.x >= 0f)
                {
                    healthBar.transform.localScale -= new Vector3(healthSpeed, 0, 0);
                }
                else
                {
                    fail = true;
                    theMusic.Stop();
                    failScreen.SetActive(true);
                    theBS.hasStarted = false;
                }
            }

            if ((!theMusic.isPlaying && !resultsScreen.activeInHierarchy)  && fail == false)
            {
                resultsScreen.SetActive(true);

                normalsText.text = normalHits.ToString();
                GoodsText.text = goodHits.ToString();
                perfectsText.text = perfectHits.ToString();
                missesText.text = missedHits.ToString();

                percentHitText.text = currentPercent.ToString("F2") +  "%";

                string rankVal = "F";

                if(currentPercent > 50)
                {
                    rankVal = "D";
                    if(currentPercent > 75)
                    {
                        rankVal = "C";
                        if(currentPercent > 85)
                        {
                            rankVal = "B";
                            if(currentPercent > 92.5)
                            {
                                rankVal = "A";
                                if(currentPercent >= 95)
                                {
                                    if(missedHits > 0)
                                    {
                                        rankVal = "A";
                                    }
                                    else
                                    {
                                        rankVal = "S";
                                        if (currentPercent == 100)
                                        {
                                            rankVal = "SS";
                                        }
                                    }                                    
                                }
                            }
                        }
                    }
                }

                rankText.text = rankVal;

                finalScoreText.text = currentScore.ToString();
            }
        }
    }

    public void NoteHit()
    {
        Debug.Log("Hit On Time");

        /*if(currentMultiplier - 1 < multiplierThresholds.Length)
        {
            multiplierTracker++;

            if (multiplierThresholds[currentMultiplier - 1] <= multiplierTracker)
            {
                multiplierTracker = 0;
                currentMultiplier++;
            }
        } */

        if (theMusic.isPlaying)
        {
            HitSound.Play();
        }

        currentMultiplier++;
        multiText.text = "Multiplier: x" + currentMultiplier;

        //currentScore += scorePerNote * currentMultiplier;
        scoreText.text = "Score: " + currentScore;

    }

    public void NormalHit()
    {
        currentScore += scorePerNote * currentMultiplier;
        NoteHit();

        normalHits++;
        currentNote++;
        totalPercent += 25;

        if(healthBar.transform.localScale.x + healthSpeed * 25f > 10)
        {
            healthBar.transform.localScale = new Vector3(10, 1, 1);
        }
        else
        {
            healthBar.transform.localScale += new Vector3(healthSpeed * 25, 0, 0);
        }
    }

    public void GoodHit()
    {
        currentScore += scorePerGoodNote * currentMultiplier;
        NoteHit();

        goodHits++;
        currentNote++;
        currentPercent++;
        totalPercent += 50;

        if (healthBar.transform.localScale.x + healthSpeed * 50f > 10)
        {
            healthBar.transform.localScale = new Vector3(10, 1, 1);
        }
        else
        {
            healthBar.transform.localScale += new Vector3(healthSpeed * 50, 0, 0);
        }
    }

    public void PerfectHit()
    {
        currentScore += scorePerPerfectNote * currentMultiplier;
        NoteHit();

        perfectHits++;
        currentNote++;
        totalPercent += 100;

        if (healthBar.transform.localScale.x + healthSpeed * 100f > 10)
        {
            healthBar.transform.localScale = new Vector3(10, 1, 1);
        }
        else
        {
            healthBar.transform.localScale += new Vector3(healthSpeed * 100f, 0, 0);
        }
    }

    public void NoteMissed()
    {
        Debug.Log("Missed Note");

        currentMultiplier = 1;
        multiplierTracker = 0;

        multiText.text = "Multiplier: x" + currentMultiplier;

        missedHits++;
        currentNote++;

        if(healthBar.transform.localScale.x - 1f < 0)
        {
            healthBar.transform.localScale = new Vector3(0, 1, 1);
        }
        else
        {
            healthBar.transform.localScale -= new Vector3(1f, 0, 0);
        }
    }
}

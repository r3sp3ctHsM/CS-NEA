using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiPlayerTK;

public class NoteView : MonoBehaviour
{
    public static bool FirstNotePlayed = false;
    public MPTKEvent note;
    public MidiStreamPlayer midiStreamPlayer;
    public bool played = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!played && transform.position.y < 1f)
        {
            played = true;
            midiStreamPlayer.MPTK_PlayEvent(note);
            FirstNotePlayed = true;
        }
    }

    private void FixedUpdate()
    {
        float translation = Time.fixedDeltaTime * GameManager.speed;
        transform.Translate(0, -translation, 0);
    }
}

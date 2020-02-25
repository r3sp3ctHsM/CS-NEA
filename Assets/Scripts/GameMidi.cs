using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiPlayerTK;
using System;

public class GameMidi : MonoBehaviour
{

    public static float Speed = 15f;
    public MidiFilePlayer midiFilePlayer;
    public MidiStreamPlayer midiStreamPlayer;
    public NoteView NoteDisplay;
    public float LastTimeCollider;
    public float DelayCollider = 5;
    public float FirstDelayCollider = 20;
    public GameObject Object;

    public Material MatNewNote;
    private float[] ButtonLocation = new float[] { -1.5f, -0.5f, 0.5f, 1.5f };

    // Start is called before the first frame update
    void Start()
    {
        if (midiFilePlayer != null)
        {
            // If call is already set from the inspector there is no need to set another listeneer
            if (!midiFilePlayer.OnEventNotesMidi.HasEvent())
            {
                // No listener defined, set now by script. NotesToPlay will be called for each new notes read from Midi file
                Debug.Log("MusicView: no OnEventNotesMidi defined, set by script");
                midiFilePlayer.OnEventNotesMidi.AddListener(NotesToPlay);
            }
        }
        else
        {
            Debug.Log("MusicView: no MidiFilePlayer detected");
        }
    }
    public void NotesToPlay(List<MPTKEvent> notes)
    {
        System.Random rng = new System.Random();

        //Debug.Log(midiFilePlayer.MPTK_PlayTime.ToString() + " count:" + notes.Count);
        foreach (MPTKEvent note in notes)
        {
            switch (note.Command)
            {
                case MPTKCommand.NoteOn:
                    if (note.Value > 40 && note.Value < 100)// && note.Channel==1)
                    {
                        // Axis Z for the note value

                        int random = rng.Next(0, 4);
                        Vector3 position = new Vector3(ButtonLocation[random - 1], 10f, 0);
                        NoteView n = Instantiate<NoteView>(NoteDisplay, position, Quaternion.identity);
                        n.gameObject.SetActive(true);
                        n.hideFlags = HideFlags.HideInHierarchy;
                        n.midiStreamPlayer = midiStreamPlayer;
                        n.note = note;
                        n.gameObject.GetComponent<SpriteRenderer>().material = MatNewNote;
                        // See noteview.cs: update() move the note along the plan until they fall out, then they are played

                        PlaySound();
                    }
                    break;
            }
        }
    }

    private void PlaySound()
    {
        // Some sound for waiting the notes ...
        if (!NoteView.FirstNotePlayed)
            //! [Example PlayNote]
            midiStreamPlayer.MPTK_PlayEvent
            (
                new MPTKEvent()
                {
                    Channel = 9,
                    Duration = 999999,
                    Value = 48,
                    Velocity = 100
                }
            );
        //! [Example PlayNote]
    }

    // Update is called once per frame
    void Update()
    {

    }
}

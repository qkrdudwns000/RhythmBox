using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public int bpm = 0;
    double currentTime = 0d;

    [SerializeField] Transform tfNoteAppear;

    TimingManager theTimingManger;
    EffectManager theEffectmanager;

    private void Start()
    {
        theTimingManger = GetComponent<TimingManager>();
        theEffectmanager = FindObjectOfType<EffectManager>();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= 60d / bpm)
        {
            GameObject t_note = ObjectPool.inst.noteQueue.Dequeue();
            t_note.transform.position = tfNoteAppear.position;
            t_note.SetActive(true);
            
            theTimingManger.boxNoteList.Add(t_note);
            currentTime -= 60d / bpm;
        }
    }
    // 노트가 화면밖으로나간경우
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Note"))
        {
            if(collision.GetComponent<Note>().GetNoteFlag())
                theEffectmanager.JudgementEffect(4);
            theTimingManger.boxNoteList.Remove(collision.gameObject);

            ObjectPool.inst.noteQueue.Enqueue(collision.gameObject);
            collision.gameObject.SetActive(false);
        }
    }
}

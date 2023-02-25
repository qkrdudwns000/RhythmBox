using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public int bpm = 0;
    double currentTime = 0d;

    [SerializeField] Transform tfNoteAppear;
    [SerializeField] GameObject goNote;

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
            GameObject t_note = Instantiate(goNote, tfNoteAppear.position, Quaternion.identity);
            t_note.transform.SetParent(this.transform); // note�������� canvas�ȿ� �־���� ���̹Ƿ� �θ���.
            theTimingManger.boxNoteList.Add(t_note);
            currentTime -= 60d / bpm;
        }
    }
    // ��Ʈ�� ȭ������γ������
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Note"))
        {
            if(collision.GetComponent<Note>().GetNoteFlag())
                theEffectmanager.JudgementEffect(4);
            theTimingManger.boxNoteList.Remove(collision.gameObject);
            Destroy(collision.gameObject);
        }
    }
}

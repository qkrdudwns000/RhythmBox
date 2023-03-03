using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingManager : MonoBehaviour
{
    public List<GameObject> boxNoteList = new List<GameObject>();

    int[] judgementRecord = new int[5];

    [SerializeField] Transform Center = null; 
    [SerializeField] RectTransform[] timingRect = null;
    Vector2[] timingBoxs = null;

    EffectManager theEffect;
    ScoreManager theScoreManager;
    ComboManager theComboManger;
    StageManager theStageManager;
    PlayerController thePlayer;
    StatusManager theStatusManager;
    AudioManager theAudioManager;

    // Start is called before the first frame update
    void Start()
    {
        theAudioManager = AudioManager.inst;
        theEffect = FindObjectOfType<EffectManager>();
        theScoreManager = FindObjectOfType<ScoreManager>();
        theComboManger = FindObjectOfType<ComboManager>();
        theStageManager = FindObjectOfType<StageManager>();
        thePlayer = FindObjectOfType<PlayerController>();
        theStatusManager = FindObjectOfType<StatusManager>();

        // Ÿ�̹� �ڽ� ����.
        timingBoxs = new Vector2[timingRect.Length];
        for(int i = 0; i < timingRect.Length; i++)
        {
            // -�������� ~ +�������� (�ּ�,�ִ����������� vector2������)
            timingBoxs[i].Set(Center.localPosition.x - timingRect[i].rect.width / 2, 
                              Center.localPosition.x + timingRect[i].rect.width / 2);
        }
    }

    public bool CheckTiming()
    {
        for(int i = 0; i < boxNoteList.Count; i++)
        {
            float t_notePosX = boxNoteList[i].transform.localPosition.x;
            
            for(int x = 0; x < timingBoxs.Length; x++)
            {
                if (timingBoxs[x].x <= t_notePosX && t_notePosX <= timingBoxs[x].y)
                {
                    // ��Ʈ����
                    boxNoteList[i].GetComponent<Note>().HideNote();
                    boxNoteList.RemoveAt(i);
                    // ����Ʈ ����
                    if (x < timingBoxs.Length - 1) // Bad �϶��� effect �ȳ����Բ�.
                        theEffect.noteHitEffect();
                    
                    
                    if (CheckCanNextPlate()) // ó�� ��ƺ��� ���� !
                    {
                        theScoreManager.IncreaseScore(x); // ���� ����
                        theStageManager.ShowNextPlates(); // �������� ����.
                        theEffect.JudgementEffect(x); // perfect ~ good ����
                        judgementRecord[x]++; // �������
                        theStatusManager.CheckShield(); // ����üũ
                    }
                    else // �̹� �ѹ� ��Ҵ� ���� !
                    {
                        theEffect.JudgementEffect(5); // normal ����
                    }

                   theAudioManager.PlaySFX("Clap");

                    return true;
                }
            }
        }
        // Miss ����.
        theComboManger.ResetCombo();
        theEffect.JudgementEffect(4);
        MissRecord();
        return false;
    }

    bool CheckCanNextPlate()
    {
        if(Physics.Raycast(thePlayer.destPos, Vector3.down, out RaycastHit t_hitInfo, 1.1f))
        {
            if(t_hitInfo.transform.CompareTag("BasicPlate"))
            {
                BasicPlate t_plate = t_hitInfo.transform.GetComponent<BasicPlate>();
                if (t_plate.flag)
                {
                    t_plate.flag = false; // �ѹ����� ������ ���ܽ�Ű������.
                    return true;
                }
            }
        }

        return false;
    }

    public int[] GetJudgementRecord()
    {
        return judgementRecord;
    }

    public void MissRecord()
    {
        judgementRecord[4]++; // miss �������.
        theStatusManager.ResetShieldCombo();
    }

    public void Initialized()
    {
        for(int i = 0; i < judgementRecord.Length; i++)
        {
            judgementRecord[i] = 0;
        }
    }
}

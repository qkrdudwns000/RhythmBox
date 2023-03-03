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

        // 타이밍 박스 설정.
        timingBoxs = new Vector2[timingRect.Length];
        for(int i = 0; i < timingRect.Length; i++)
        {
            // -판정넓이 ~ +판정넓이 (최소,최대판정범위를 vector2에저장)
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
                    // 노트제거
                    boxNoteList[i].GetComponent<Note>().HideNote();
                    boxNoteList.RemoveAt(i);
                    // 이펙트 연출
                    if (x < timingBoxs.Length - 1) // Bad 일때는 effect 안나오게끔.
                        theEffect.noteHitEffect();
                    
                    
                    if (CheckCanNextPlate()) // 처음 밟아보는 발판 !
                    {
                        theScoreManager.IncreaseScore(x); // 점수 증가
                        theStageManager.ShowNextPlates(); // 스테이지 생성.
                        theEffect.JudgementEffect(x); // perfect ~ good 판정
                        judgementRecord[x]++; // 판정기록
                        theStatusManager.CheckShield(); // 쉴드체크
                    }
                    else // 이미 한번 밟았던 발판 !
                    {
                        theEffect.JudgementEffect(5); // normal 판정
                    }

                   theAudioManager.PlaySFX("Clap");

                    return true;
                }
            }
        }
        // Miss 판정.
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
                    t_plate.flag = false; // 한번밟은 발판은 제외시키기위함.
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
        judgementRecord[4]++; // miss 판정기록.
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingManager : MonoBehaviour
{
    public List<GameObject> boxNoteList = new List<GameObject>();

    [SerializeField] Transform Center = null; 
    [SerializeField] RectTransform[] timingRect = null;
    Vector2[] timingBoxs = null;

    EffectManager theEffect;

    // Start is called before the first frame update
    void Start()
    {
        theEffect = FindObjectOfType<EffectManager>();

        // 타이밍 박스 설정.
        timingBoxs = new Vector2[timingRect.Length];
        for(int i = 0; i < timingRect.Length; i++)
        {
            // -판정넓이 ~ +판정넓이 (최소,최대판정범위를 vector2에저장)
            timingBoxs[i].Set(Center.localPosition.x - timingRect[i].rect.width / 2, 
                              Center.localPosition.x + timingRect[i].rect.width / 2);
        }
    }

    public void CheckTiming()
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
                    theEffect.JudgementEffect(x);
                    return;
                }
            }
        }
        theEffect.JudgementEffect(4);
    }
}

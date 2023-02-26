using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    [SerializeField] GameObject goComboImage = null;
    [SerializeField] TMPro.TMP_Text txtCombo = null;

    int currentCombo = 0;
    int maxCombo = 0;

    Animator myAnim;
    string animComboUp = "ComboUp";

    private void Start()
    {
        myAnim = GetComponent<Animator>();
        ResetCombo();
    }

    public void IncreaseCombo(int p_num = 1)
    {
        currentCombo += p_num;
        txtCombo.text = string.Format("{0:#,##0}", currentCombo); // 3ÀÚ¸´¼ö¸¶´Ù ÄÞ¸¶¸¦ Âï¾îÁÖ¼¼¿ä.

        if (maxCombo < currentCombo)
            maxCombo = currentCombo;

        if(currentCombo > 2)
        {
            txtCombo.gameObject.SetActive(true);
            goComboImage.SetActive(true);

            myAnim.SetTrigger(animComboUp);
        }
    }

    public int GetCurrentCombo()
    {
        return currentCombo;
    }

    public void ResetCombo()
    {
        currentCombo = 0;
        txtCombo.text = "0";
        txtCombo.gameObject.SetActive(false);
        goComboImage.SetActive(false);
    }

    public int GetMaxCombo()
    {
        return maxCombo;
    }
}

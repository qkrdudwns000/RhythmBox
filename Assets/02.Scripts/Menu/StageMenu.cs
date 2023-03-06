using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Song
{
    public string name;
    public string composer;
    public int bpm;
    public Sprite sprite;
}

public class StageMenu : MonoBehaviour
{
    [SerializeField] Song[] songList = null;

    [SerializeField] TMPro.TMP_Text txtSongName = null;
    [SerializeField] TMPro.TMP_Text txtSongComposer = null;
    [SerializeField] TMPro.TMP_Text txtSongScore = null;
    [SerializeField] Image imgDisk = null;

    [SerializeField] GameObject TitleMenu = null;

    DataBaseManager theDataBase;

    int currentSong = 0;

    private void OnEnable()
    {
        if(theDataBase == null)
            theDataBase = FindObjectOfType<DataBaseManager>();

        SettingSong();
    }
    public void BtnNext()
    {
        AudioManager.inst.PlaySFX("Touch");

        if (++currentSong > songList.Length - 1)
            currentSong = 0;
        SettingSong();
    }
    public void BtnPrior()
    {
        AudioManager.inst.PlaySFX("Touch");

        if (--currentSong < 0)
            currentSong = songList.Length - 1;
        SettingSong();
    }
    void SettingSong()
    {
        txtSongName.text = songList[currentSong].name;
        txtSongComposer.text = songList[currentSong].composer;
        txtSongScore.text = string.Format("{0:#,##0}", theDataBase.score[currentSong]);
        imgDisk.sprite = songList[currentSong].sprite;

        AudioManager.inst.PlayBGM("BGM" + currentSong);
    }

    public void BtnBack()
    {
        TitleMenu.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void BtnPlay()
    {
        int t_bpm = songList[currentSong].bpm;

        GameManager.inst.GameStart(currentSong, t_bpm);
        this.gameObject.SetActive(false);
    }
}

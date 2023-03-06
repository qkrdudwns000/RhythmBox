using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd; // 서버
using LitJson; // 제이슨파일.

public class DataBaseManager : MonoBehaviour
{
    public int[] score;

    private void Start()
    {
        LoadScore();
    }
    public void SaveScore()
    {
        Backend.BMember.GetUserInfo(UserDataBro =>
        {
            if(UserDataBro.IsSuccess())
            {
                //Param data = new Param();
                //data.Add("Scores", score);

                //if (UserDataBro.GetReturnValuetoJSON()["rows"].Count > 0)
                //{
                //    string t_Indate = UserDataBro.GetReturnValuetoJSON()["rows"][0]["inDate"]["S"].ToString();
                //    Backend.BMember.GetUserInfo(t_Indate, data, (t_callback) =>
                //    {

                //    });
                //}
                //else
                //{
                //    Backend.BMember.GetUserInfo(data, (t_callback) =>
                //    {

                //    });
                //}
            }
        });
    }
    public void LoadScore()
    {
        if(PlayerPrefs.HasKey("Score1"))
        {
            score[0] = PlayerPrefs.GetInt("Score1");
            score[1] = PlayerPrefs.GetInt("Score2");
            score[2] = PlayerPrefs.GetInt("Score3");
        }
    }
}

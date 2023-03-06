using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd; // 뒤끝서버 관련 유징.

public class Login : MonoBehaviour
{
    [SerializeField] TMPro.TMP_InputField id = null;
    [SerializeField] TMPro.TMP_InputField pw = null;

    // Start is called before the first frame update
    void Start()
    {
        Backend.Initialize(true); // 뒤끝서버 초기화(통신세팅)
        InitializeCallback();
    }

    void InitializeCallback()
    {
        if (Backend.IsInitialized)
        {
            Debug.Log(Backend.Utils.GetServerTime()); // 현재시간.
            Debug.Log(Backend.Utils.GetGoogleHash()); // 모바일 <-> 뒤끝서버 토신에 필요한 구글 해시키
        }
        else
            Debug.Log("초기화 실패 (인터넷 문제 등등");
    }

    public void BtnRegist()
    {
        string t_id = id.text;
        string t_pw = pw.text;

        BackendReturnObject bro = Backend.BMember.CustomSignUp(t_id, t_pw, "Test");// 뒤끝서버 함수 호출 시 BRO값으로 리턴됨 그걸 받아옴,

        if(bro.IsSuccess())
        {
            Debug.Log("회원가입 완료");
            this.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("회원가입 실패");
        }

    }

    public void BtnLogin()
    {
        string t_id = id.text;
        string t_pw = pw.text;

        BackendReturnObject bro = Backend.BMember.CustomLogin(t_id, t_pw);// 뒤끝서버 함수 호출 시 BRO값으로 리턴됨 그걸 받아옴,

        if (bro.IsSuccess())
        {
            Debug.Log("로그인 완료");
            this.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("로그인 실패"); 
        }
    }
}

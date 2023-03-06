using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd; // �ڳ����� ���� ��¡.

public class Login : MonoBehaviour
{
    [SerializeField] TMPro.TMP_InputField id = null;
    [SerializeField] TMPro.TMP_InputField pw = null;

    // Start is called before the first frame update
    void Start()
    {
        Backend.Initialize(true); // �ڳ����� �ʱ�ȭ(��ż���)
        InitializeCallback();
    }

    void InitializeCallback()
    {
        if (Backend.IsInitialized)
        {
            Debug.Log(Backend.Utils.GetServerTime()); // ����ð�.
            Debug.Log(Backend.Utils.GetGoogleHash()); // ����� <-> �ڳ����� ��ſ� �ʿ��� ���� �ؽ�Ű
        }
        else
            Debug.Log("�ʱ�ȭ ���� (���ͳ� ���� ���");
    }

    public void BtnRegist()
    {
        string t_id = id.text;
        string t_pw = pw.text;

        BackendReturnObject bro = Backend.BMember.CustomSignUp(t_id, t_pw, "Test");// �ڳ����� �Լ� ȣ�� �� BRO������ ���ϵ� �װ� �޾ƿ�,

        if(bro.IsSuccess())
        {
            Debug.Log("ȸ������ �Ϸ�");
            this.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("ȸ������ ����");
        }

    }

    public void BtnLogin()
    {
        string t_id = id.text;
        string t_pw = pw.text;

        BackendReturnObject bro = Backend.BMember.CustomLogin(t_id, t_pw);// �ڳ����� �Լ� ȣ�� �� BRO������ ���ϵ� �װ� �޾ƿ�,

        if (bro.IsSuccess())
        {
            Debug.Log("�α��� �Ϸ�");
            this.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("�α��� ����"); 
        }
    }
}

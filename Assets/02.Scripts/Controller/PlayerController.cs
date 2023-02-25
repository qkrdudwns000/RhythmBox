using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static bool s_canPresskey = true;

    // �̵�
    [SerializeField] float moveSpeed = 3;
    Vector3 dir = new Vector3();
    public Vector3 destPos = new Vector3();

    // ȸ��
    [SerializeField] float spinSpeed = 270;
    Vector3 rotDir = new Vector3();
    Quaternion destRot = new Quaternion();

    // ����̰Բ� �ݵ���ġ.
    [SerializeField] float recoilPosY = 0.25f;
    [SerializeField] float recoilSpeed = 1.5f;

    bool canMove = true;
    
    // ��Ÿ
    [SerializeField] Transform fakeCube = null;
    [SerializeField] Transform realCube = null;

    TimingManager theTimingManager;
    CameraController theCam;

    private void Start()
    {
        theTimingManager = FindObjectOfType<TimingManager>();
        theCam = FindObjectOfType<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W))
        {
            if (canMove && s_canPresskey)
            {
                Calc();

                if (theTimingManager.CheckTiming())
                {
                    StartAction();
                }
            }
        }
    }
    void Calc()
    {
        // ���� ���
        if(!(Input.GetAxisRaw("Vertical") != 0 && Input.GetAxisRaw("Horizontal") != 0))
            dir.Set(Input.GetAxisRaw("Vertical"), 0, Input.GetAxisRaw("Horizontal"));
        else
        {
            int rnd = Random.Range(0, 2);
            if(rnd == 0)
                dir.Set(Input.GetAxisRaw("Vertical"), 0, 0);
            else
                dir.Set(0, 0, Input.GetAxisRaw("Horizontal"));
        }

        // �̵� ��ǥ�� ���
        destPos = transform.position + new Vector3(-dir.x, 0, dir.z);

        // ȸ�� ��ǥ�� ���
        rotDir = new Vector3(-dir.z, 0, -dir.x);
        fakeCube.RotateAround(transform.position, rotDir, spinSpeed);
        destRot = fakeCube.rotation;
    }
    void StartAction()
    {
        StartCoroutine(MoveCo());
        StartCoroutine(SpinCo());
        StartCoroutine(RecoilCo());
        StartCoroutine(theCam.ZoomCamCo());
    }

    IEnumerator MoveCo()
    {
        canMove = false;

        while(Vector3.SqrMagnitude(transform.position - destPos) >= 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = destPos;
        canMove = true;
    }
    IEnumerator SpinCo()
    {
        while(Quaternion.Angle(realCube.rotation, destRot) > 0.5f)
        {
            realCube.rotation = Quaternion.RotateTowards(realCube.rotation, destRot, spinSpeed * Time.deltaTime);
            yield return null;
        }
        realCube.rotation = destRot;
    }

    IEnumerator RecoilCo()
    {
        while(realCube.position.y < recoilPosY)
        {
            realCube.position += new Vector3(0, recoilSpeed * Time.deltaTime, 0);
            yield return null;
        }
        while(realCube.position.y > 0)
        {
            realCube.position -= new Vector3(0, recoilSpeed * Time.deltaTime, 0);
            yield return null;
        }

        realCube.localPosition = new Vector3(0, 0, 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform thePlayer = null;
    [SerializeField] float followSpeed = 15;

    Vector3 playerDistance = new Vector3(); // 카메라와 플레이어간의 거리

    float hitDistance = 0;
    [SerializeField] float zoomDistance = -1.25f;

    // Start is called before the first frame update
    void Start()
    {
        playerDistance = transform.position - thePlayer.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 t_destPos = thePlayer.position + playerDistance + (transform.forward * hitDistance);
        transform.position = Vector3.Lerp(transform.position, t_destPos, followSpeed * Time.deltaTime);
    }
    public IEnumerator ZoomCamCo()
    {
        hitDistance = zoomDistance;

        yield return new WaitForSeconds(0.15f);

        hitDistance = 0;
    }
}

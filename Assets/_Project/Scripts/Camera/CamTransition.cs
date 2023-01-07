using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CamTransition : MonoBehaviour
{
    [SerializeField]
    CinemachineVirtualCamera cam1, cam2;

    public void ToCam1AfterSeconds(float after)
    {
        StartCoroutine(ToCam1Async(after));
    }

    public void ToCam2AfterSeconds(float after)
    {
        StartCoroutine(ToCam2Async(after));
    }

    IEnumerator ToCam1Async(float time)
    {
        yield return new WaitForSeconds(time);
        ToCam1();
    }

    IEnumerator ToCam2Async(float time)
    {
        yield return new WaitForSeconds(time);
        ToCam2();
    }

    public void ToCam1()
    {
        cam1.Priority = 1;
        cam2.Priority = 0;
    }

    public void ToCam2()
    {
        cam1.Priority = 0;
        cam2.Priority = 1;
    }
}

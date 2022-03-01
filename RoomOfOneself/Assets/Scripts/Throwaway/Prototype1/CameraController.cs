using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    CinemachineVirtualCamera[] virtualCameras;

    [SerializeField]
    Transform[] lookatPoints;

    int currentActiveVCam;

    bool canTrans;
    const float transSec = 2f;

    [SerializeField]
    GameObject[] enviroSuits;

    int currentActiveEnviro;

    float FieldOfViewTransTime = 2f;
    float minFOV = 20f;
    float maxFOV = 60f;

    bool isForward = true;


    private void Start()
    {
        InitEnviroSuits();
        InitVirtualCameras();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isForward)
            {
                if (currentActiveVCam < virtualCameras.Length - 1)
                    TransToCertainCamera(currentActiveVCam + 1);
                else
                    FocusOnItem();
            }
            else
            {
                if (currentActiveVCam > 0)
                    TransToCertainCamera(currentActiveVCam - 1);
            }
        }
            
    }

    void InitEnviroSuits()
    {
        currentActiveEnviro = 0; // to do: load the saved variable when there's a save file

        for (int i = 0; i < enviroSuits.Length; i++)
        {
            enviroSuits[i].SetActive(i == currentActiveEnviro);
        }
    }

    void InitVirtualCameras()
    {
        currentActiveVCam = 0; // to do: load the saved variable when there's a save file

        for (int i = 0; i < virtualCameras.Length; i++)
        {
            virtualCameras[i].gameObject.SetActive(i == currentActiveVCam);
        }

        canTrans = true;
    }

    void TransToCertainCamera(int newIndex)
    {
        if (newIndex == currentActiveVCam)
            return;

        if (!canTrans)
            return;

        currentActiveVCam = newIndex;
        virtualCameras[currentActiveVCam].gameObject.SetActive(true);
        canTrans = false;

        StartCoroutine(EnablePreviousCameraInSec(transSec));
    }

    IEnumerator EnablePreviousCameraInSec(float waitSec)
    {
        yield return new WaitForSeconds(waitSec);

        for (int i = 0; i < virtualCameras.Length; i++)
        {
            if (i != currentActiveVCam)
                virtualCameras[i].gameObject.SetActive(false);
        }

        canTrans = true;
    }

    void FocusOnItem()
    {
        canTrans = false;
        isForward = false;
        StartCoroutine(TransFieldOfView(maxFOV, minFOV, true, 0f));
    }

    IEnumerator TransFieldOfView(float startFOV, float targetFOV, bool changeEnviro, float waitSec)
    {
        yield return new WaitForSeconds(waitSec);

        float passedSec = 0f;

        while (passedSec < FieldOfViewTransTime)
        {
            passedSec += Time.deltaTime;
            virtualCameras[currentActiveVCam].m_Lens.FieldOfView = Mathf.Lerp(startFOV, targetFOV, passedSec / FieldOfViewTransTime);
            yield return null;
        }

        virtualCameras[currentActiveVCam].m_Lens.FieldOfView = targetFOV;

        if (changeEnviro)
            TransEnvironment(currentActiveEnviro + 1);
        else
            canTrans = true;

        yield break;

    }

    void TransEnvironment(int enviroIndex)
    {
        if (enviroIndex == currentActiveEnviro)
            return;

        int previousIndex = currentActiveEnviro;
        currentActiveEnviro = enviroIndex;
        enviroSuits[currentActiveEnviro].SetActive(true);
        enviroSuits[previousIndex].SetActive(false);

        StartCoroutine(TransFieldOfView(minFOV, maxFOV, false, 2f));
    }

}

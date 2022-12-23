using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnOnCamera : MonoBehaviour
{
    private bool camAvailable;
    private WebCamTexture backCam;
    private Texture defaultBackground;
    private RawImage background; // Used as a fallback in case camera fails
    private Quaternion baseRotation; // This is used to have the camera show properly.
    public AspectRatioFitter fitter; 

    // Start is called before the first frame update
    void Start()
    {
        background = GetComponent<RawImage>();
        if(background == null){
            Debug.Log("This did not work as expected");
        }else{
            Debug.Log("I might be on to something");
        }
        defaultBackground = background.texture;
        
        WebCamDevice[] devices = WebCamTexture.devices;

        if(devices.Length == 0)
        {
            Debug.Log("No camera detected");
            camAvailable = false;
            return;
        }

        for(int i = 0; i < devices.Length; i++){
            if(!devices[i].isFrontFacing)
            {
                backCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
                // backCam = new WebCamTexture(devices[i].name, Screen.width, Screen.width);
                // backCam = new WebCamTexture(devices[i].name);
            }
        }

        if(backCam == null){
            Debug.Log("Unable to find camera");
        }

        background.texture = backCam;
        baseRotation = transform.rotation; // From documentation. transform is the GameObject's transform, and here we store its initial state that we later update.
        Debug.Log("Base rotation: " + baseRotation);
        backCam.Play();
        camAvailable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!camAvailable)
            return;

        // change ratio to show camera properly
        float ratio = (float)backCam.width/(float)backCam.height;
        fitter.aspectRatio = ratio;
        Debug.Log("IN UPDATE");
        Debug.Log("Ratio: " + ratio);
        // Debug.Log("Video rotation angle: " + backCam.videoRotationAngle);
        // Debug.Log("Video vertically mirrored: " + backCam.videoVerticallyMirrored);

        // From documentation:
        // change transform of gameobject that shows camera,
        // so that it aligns properly.
        // But, had to change second parameter from Vector3.up to Vector3.back
        background.transform.rotation = baseRotation * Quaternion.AngleAxis(backCam.videoRotationAngle, Vector3.back);
        Debug.Log("New rotation: " + baseRotation * Quaternion.AngleAxis(backCam.videoRotationAngle, Vector3.back));

        // from device camera how to video
        // corrects potential video mirroring
        float scaleY = backCam.videoVerticallyMirrored ? -1f : 1f;
        transform.localScale = new Vector3(1f, scaleY, 1f);

    }
}

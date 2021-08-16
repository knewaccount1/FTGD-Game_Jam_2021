using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform MyCameraTransform;
    private Transform MyTransform;
    public bool alignNotLook = true;

    // Use this for initialization
    void Start()
    {
        MyTransform = this.transform;
        //MyCameraTransform = Camera.main.transform;

        MyCameraTransform = FindObjectOfType<GameManager>().MainCamera.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (alignNotLook)
            MyTransform.forward = MyCameraTransform.forward;
        else
        {
            MyTransform.LookAt(MyCameraTransform, Vector3.up);
            transform.rotation = Quaternion.Euler(-transform.rotation.eulerAngles.x, 0, 0);
        }
    }
}

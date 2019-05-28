using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OnlineTechniquesControl : MonoBehaviour {

    [SerializeField] private UploadOptions curOption;

    [EnumAction(typeof(UploadOptions))]
    public void SetCurUploadOption(int uploadOption) {
        curOption = (UploadOptions) uploadOption;
    }
}

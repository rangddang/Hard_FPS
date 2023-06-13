using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private GunSetting gunSetting;
    [SerializeField] private TextMeshProUGUI ammoText;

    private void Update()
    {
        ammoText.text = gunSetting.currentAmmo + "/" + gunSetting.ammos;
    }
}

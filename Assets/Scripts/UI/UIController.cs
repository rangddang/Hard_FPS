using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GunController gun;
    [SerializeField] private TextMeshProUGUI ammoText;

    private void Update()
    {
        ammoText.text = gun.bulletSetting.currentAmmo + "/" + gun.bulletSetting.hasAmmo;
    }
}

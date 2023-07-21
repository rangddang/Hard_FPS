using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType
{
    Nomal,
    Shop,
	Blessing
}

public class RoomSetting : MonoBehaviour
{
    [SerializeField] public GameObject[] doors;
    [SerializeField] public RoomType roomType;
    public bool isClear;

    public List<GameObject> enemys = new List<GameObject>();

}

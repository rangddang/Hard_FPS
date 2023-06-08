using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GunController gun;
    
    private Status status;
    private CharacterController character;

    private void Awake()
    {
        character = GetComponent<CharacterController>();
    }

    private void Update()
    {
        
    }
}

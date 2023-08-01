using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PersonalityName
{
	영웅,
	완벽주의자,
	전투광,
	결투의대가,
	분노조절장애,
	파티광
}

[System.Serializable]
public class PersonalityHeart
{
	public PersonalityName name;
	public string explanation;
}

public class PersonalitySetting : MonoBehaviour
{
    public PersonalityHeart personality;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PersonalityName
{
	����,
	�Ϻ�������,
	������,
	�����Ǵ밡,
	�г��������,
	��Ƽ��
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

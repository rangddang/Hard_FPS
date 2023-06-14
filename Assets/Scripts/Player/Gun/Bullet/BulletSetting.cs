using UnityEditor;

public enum BulletName
{
	Bullet,
}

[System.Serializable]
public struct BulletSetting
{
	public BulletName bulletName;
	public string bulletExplanation;
	public int currentAmmo;
	public int hasAmmo;
	public int maxAmmo;
	public float attackPower;
	public float attackDamage;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tuning
{
    public List<TuningType> tuningTypes = new List<TuningType>();
}

[System.Serializable]
public class TuningType
{
    public string name;
    public List<TuningItem> items = new List<TuningItem>();
}

[System.Serializable]
public class TuningItem
{
	public string name;
	public List<TuningItemUpgrade> items = new List<TuningItemUpgrade>();
}

[System.Serializable]
public class TuningItemUpgrade
{
	public string name;
	public string explanation;
	public bool active;
}

public class TuningManager : MonoBehaviour
{
    public Tuning tuning;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SwipPersonality : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI personalityText;
	[SerializeField] private TextMeshProUGUI personalityExplanationText;

	public GameObject m_scrollbar;
    private Scrollbar scrollbar;
    private float scrollPos = 0;
    private float[] pos;

    private void Awake()
    {
        scrollbar = m_scrollbar.GetComponent<Scrollbar>();
    }

    private void Update()
    {
        pos = new float[transform.childCount];
        float distance = 1f / (pos.Length - 1f);
        for(int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }
        if (Input.GetMouseButton(0))
        {
            scrollPos = scrollbar.value;
        }
        else
        {
            for(int i = 0; i < pos.Length; i++)
            {
                if(scrollPos < pos[i] + (distance / 2) && scrollPos > pos[i] - (distance / 2))
                {
					scrollbar.value = Mathf.Lerp(scrollbar.value, pos[i], Time.deltaTime * 10);
                }
            }
        }

        for(int i=0; i < pos.Length; i++)
        {
            if(scrollPos < pos[i] + (distance/2) && scrollPos > pos[i] - (distance / 2))
            {
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f, 1f), Time.deltaTime * 10);
                for(int j = 0; j < pos.Length; j++)
                {
                    if(j != i)
                    {
						transform.GetChild(j).localScale = Vector2.Lerp(transform.GetChild(j).localScale, new Vector2(0.6f, 0.6f), Time.deltaTime * 10);
					}
                }
            }
        }

		for (int i = 0; i < pos.Length; i++)
		{
			if (scrollPos < pos[i] + (distance / 2) && scrollPos > pos[i] - (distance / 2))
			{
                personalityText.text = transform.GetChild(i).GetComponent<PersonalitySetting>().personality.name.ToString();
				personalityExplanationText.text = transform.GetChild(i).GetComponent<PersonalitySetting>().personality.explanation.ToString();
			}
		}
    }

}

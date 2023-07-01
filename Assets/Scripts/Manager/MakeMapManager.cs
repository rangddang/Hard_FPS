using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeMapManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> rooms = new List<GameObject>();
    [SerializeField] private GameObject connection;
    [SerializeField] public List<GameObject> maps = new List<GameObject>();
    [SerializeField] private float roomOfDistance = 80;
    [SerializeField] private int mapCount = 20;

    private void Start()
    {
        Vector3[] dirs = { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };
        int randRoom = 0;
        int dirRand = 0;
        for(int i=0; i< mapCount; i++)
        {
            if(maps.Count > 0)
            // ���� 1�� �̻��϶�
            {
                while (true)
                // �� ���� 4�� �� �������� ���
                {
                    randRoom = Random.Range(0, maps.Count);
                    RoomSetting room = maps[randRoom].GetComponent<RoomSetting>();
                    bool[] saveDir = new bool[4];
                    int outCount = 0;
                    for (int k = 0; k < 4; k++)
                    //�� 4�� �� �����ִ��� üũ
                    {
                        if (room.doors[k].active)
                        {
                            outCount++;
                        }
                        else
                        {
                            saveDir[k] = true;
                        }
                    }
                    if (outCount > 0)
                    //�ϳ��� ���������� �����Ѱ� ã��
                    {
                        do
                        //�����ִ°Ÿ� �ٽ� ã��
                        {
                            dirRand = Random.Range(0, 4);
                        }
                        while (saveDir[dirRand]);

                        if (room.doors[dirRand].active)
                        //�� �����ִ��� üũ
                        {
                            Vector3 dir = dirs[dirRand];
                            int overlapCheck = 0;

							for (int n = 0; n < maps.Count; n++)
                            {
                                if (maps[n].transform.position == maps[randRoom].transform.position + (dir * roomOfDistance))
                                {
                                    overlapCheck++;
								}
                            }
                            if (overlapCheck <= 0)
                            {
                                room.doors[dirRand].SetActive(false);
                                maps.Add(Instantiate(rooms[Random.Range(0, rooms.Count)], maps[randRoom].transform.position + (dir * roomOfDistance), Quaternion.identity));
                                Instantiate(connection, maps[randRoom].transform.position + (dir * (roomOfDistance / 2)), Quaternion.Euler(0, 90 * dirRand, 0));
                                int dirBack = dirRand > 1 ? dirRand - 2 : dirRand + 2;
                                maps[maps.Count - 1].GetComponent<RoomSetting>().doors[dirBack].SetActive(false);
                                break;
                            }
                            else
                            //����� ����
                            {
								//room.doors[dirRand].SetActive(false);
								//Instantiate(connection, maps[randRoom].transform.position + (dir * (roomOfDistance / 2)), Quaternion.Euler(0, 90 * dirRand, 0));
								//int dirBack = dirRand > 1 ? dirRand - 2 : dirRand + 2;
								//maps[maps.Count - 1].GetComponent<RoomSetting>().doors[dirBack].SetActive(false);
								//break;
							}
                        }
                    }
                }
            }
            else
            {
				maps.Add(Instantiate(rooms[Random.Range(0, rooms.Count)], Vector3.zero, Quaternion.identity));
			}

        }
    }
}

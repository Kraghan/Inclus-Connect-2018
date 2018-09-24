using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public QTEType playerController;
    public bool enable = false;
    public bool isDisabledPlayer = false;

    public float skill = 0.5f;

    public float GetInitialSlowMotionFactor()
    {
        return 0.5f;
    }
}

public class PlayerDatas : MonoBehaviour
{
    public PlayerData[] datas = new PlayerData[4];


	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public PlayerData GetWeakestPlayer(QTEType[] types)
    {
        uint weakestIndex = 0;

        for (uint i = 0; i < datas.Length; ++i)
        {
            foreach (QTEType type in types)
            {
                if (datas[i].playerController == type
                    && datas[i].skill < datas[weakestIndex].skill)
                {
                    weakestIndex = i;
                    break;
                }
            }
        }

        return datas[weakestIndex];
    }


}

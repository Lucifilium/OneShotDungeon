using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetGameValues : MonoBehaviour
{
    [SerializeField]
    private FloatSO scoreSO;
    [SerializeField]
    private FloatSO timerSO;

    // Start is called before the first frame update
    void Start()
    {
        scoreSO.Value = 0;
        timerSO.Value = 180;
    }
}

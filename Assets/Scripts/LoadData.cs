using System;
using UnityEngine;

[CreateAssetMenu(fileName ="Load Data", menuName ="Load Data")]
public class LoadData : ScriptableObject
{
    public int score1;
    public int score2;

    private void OnEnable()
    {
        score1 = 0;
        score2 = 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "City/Rules")]
public class Rules : ScriptableObject
{
    public string letter;
    [SerializeField]
    private string[] results = null;

    [SerializeField]
    private bool random=false;

    public string getResult()
    {
        if (random)
        {
            return results[UnityEngine.Random.Range(0,results.Length-1)];
        }
        return results[0];
    }
}

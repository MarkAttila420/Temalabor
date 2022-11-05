using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "City/Rules")]
public class Rules : ScriptableObject
{
    public string letter;
    [SerializeField]
    private string[] results = null;

    public string getResult()
    {
        return results[0];
    }
}

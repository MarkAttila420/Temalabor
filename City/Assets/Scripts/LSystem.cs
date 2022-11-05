using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class LSystem : MonoBehaviour
{
    public Rules[] rules;
    public string root;
    [Range(1,10)]
    public int maxDepth = 1;

    private void Start()
    {
        Debug.Log(Generate());
    }
    public string Generate(string sentence=null)
    {
        if (sentence==null) 
        {
            sentence = root;
        }
        return Rekurzio(sentence);
    }

    private string Rekurzio(string sentence, int depth=0)
    {
        if (depth>=maxDepth)
        {
            return sentence;
        }
        StringBuilder newSentence=new StringBuilder();
        foreach (var c in sentence)
        {
            newSentence.Append(c);
            RulesRekurzio(newSentence,c,depth);

        }
        return newSentence.ToString();
    }
    private void RulesRekurzio(StringBuilder newSentence, char c, int depth)
    {
        foreach (var rule in rules)
        {
            if(rule.letter==c.ToString())
            {
                newSentence.Append(Rekurzio(rule.getResult(),depth+1));
            }

        }
    }
}

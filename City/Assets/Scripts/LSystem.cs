using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;
using Toggle = UnityEngine.UI.Toggle;

public class LSystem : MonoBehaviour
{
    public Rules[] rules;
    public string root;
    [Range(6,10)]
    public int maxDepth = 6;

    public bool IgnoreRandom = true;
    [Range(0, 1)]
    public float chanceToIgnore=0.3f;

    public int numberOfPedestrians = 50;

    public static float speed = 0.5f;

    [SerializeField] private Slider depthSlider;
    [SerializeField] private TextMeshProUGUI depthText;
    [SerializeField] private Slider randomSlider;
    [SerializeField] private TextMeshProUGUI randomText;
    [SerializeField] private Toggle randomToggle;
    [SerializeField] private Slider pedestrianNumberSlider;
    [SerializeField] private TextMeshProUGUI numberText;
    [SerializeField] private Slider speedSlider;
    [SerializeField] private TextMeshProUGUI speedText;
    void Start()
    {
        depthText.text = new StringBuilder($"LSystem depth: {maxDepth.ToString()}").ToString();
        depthSlider.value = maxDepth;

        numberText.text = new StringBuilder($"Number of Pedestrians: {numberOfPedestrians.ToString()}").ToString();
        pedestrianNumberSlider.value = numberOfPedestrians;

        speedText.text = new StringBuilder($"Max Speed: {speed.ToString()}").ToString();
        speedSlider.value = speed;

        randomText.text = new StringBuilder($"Ignore chance: {chanceToIgnore.ToString("0.00")}").ToString();
        randomSlider.value = chanceToIgnore;
        randomToggle.isOn = IgnoreRandom;
        depthSlider.onValueChanged.AddListener((value) =>
        {
            maxDepth = (int)value;
            depthText.text = new StringBuilder($"LSystem depth: {maxDepth.ToString()}").ToString();
        });
        randomSlider.onValueChanged.AddListener((value) =>
        {
            chanceToIgnore = value;
            randomText.text = new StringBuilder($"Ignore chance: {chanceToIgnore.ToString("0.00")}").ToString();
        });
        randomToggle.onValueChanged.AddListener((value) => { IgnoreRandom = value; });

        pedestrianNumberSlider.onValueChanged.AddListener((value) =>
        {
            numberOfPedestrians = (int)value;
            numberText.text = new StringBuilder($"Number Of Pedestrians: {numberOfPedestrians.ToString()}").ToString();
        });

        speedSlider.onValueChanged.AddListener((value) =>
        {
            speed = (float)value;
            speedText.text = new StringBuilder($"Max speed: {speed.ToString("F1")}").ToString();
        });

    }


    public string Generate(string sentence=null)
    {
        if (sentence==null) 
        {
            sentence = root;
        }
        return Rekurzio(sentence);
    }

    //Ez a fuggveny megy vegig a szon es minden beture meghivja a RulesRekurziot.
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
    //Ez a fuggveny vegigmegy az osszes szabalyon,
    //es amelyiknek az a betu van megadva, mint amelyiknel a Rekurzio fuggveny tart jelenleg, annak az egyik szabalyat hasznalva, annak a karakternek a helyere berakja a szabalyban leirt string-et. 
    private void RulesRekurzio(StringBuilder newSentence, char c, int depth)
    {
        foreach (var rule in rules)
        {
            if(rule.letter==c.ToString())
            {
                if (IgnoreRandom && UnityEngine.Random.value < chanceToIgnore&&depth>1) return;
                newSentence.Append(Rekurzio(rule.getResult(),depth+1));
            }

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Az LSystem-ben a karakterek, es azok jelentese
public enum EncodingLetters
{
    unknown = '1',
    save = '[',
    load = ']',
    draw = 'F',
    draw2 = 'G',
    turnRight = '+',
    turnLeft = '-'
}

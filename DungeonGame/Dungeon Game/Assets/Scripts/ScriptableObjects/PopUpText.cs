using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PopUpTextObject", menuName = "UI/PopUpTextObject", order = 1)]
public class PopUpTextObject : ScriptableObject
{
    [TextArea(5, 20)]
    public string MainText;
    [TextArea(5, 20)]
    public string PosText;
    [TextArea(5, 20)]
    public string NegText;

}

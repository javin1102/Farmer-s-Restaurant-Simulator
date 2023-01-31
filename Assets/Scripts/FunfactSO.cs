using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Funfact", menuName = "Custom Data/New Funfact")]
public class FunfactSO : ScriptableObject
{
    [TextArea(5, 15)]
    public string funfactDescription;
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FormatText
{
    public static string GetFormatText(long money)
    {
        string text = money.ToString("N0");
        return text;
    }
}
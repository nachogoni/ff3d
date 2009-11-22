using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PosRowCol
{
    public int rowValue;
    public int colValue;

    public PosRowCol(int row, int col)
    {
        rowValue = row;
        colValue = col;
    }

    
    override public string ToString()
    {
        return "Row: " + rowValue + " Col: " + colValue;
    }

}
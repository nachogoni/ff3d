using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FF3d_PosRowCol
{
    public int rowValue;
    public int colValue;

    public FF3d_PosRowCol(int row, int col)
    {
        rowValue = row;
        colValue = col;
    }

    
    override public string ToString()
    {
        return "Row: " + rowValue + " Col: " + colValue;
    }

}
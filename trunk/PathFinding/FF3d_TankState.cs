using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FF3d_TankState : State 
{
    CellTypes[,] map;
    int rowValue = 0;
    int colValue = 0;
    List<PosRowCol> flagsPos;

    public FF3d_TankState(CellTypes[,] cells, int row, int col, List<PosRowCol> flags)
    {

        if (cells == null || row > cells.GetLength(0) ||
            row < 0 || col > cells.GetLength(1) || col < 0)
        {
            map = null;
            rowValue = -1;
            colValue = -1;
        }

        rowValue = row;
        colValue = col;
        flagsPos = flags;
        map = cells;

	}
		
	public bool isValidMove(MoveTypes move) {

        int rowInc = 0, colInc = 0;
        bool ret = true;

        if (map == null || rowValue == -1 || colValue  == -1)
            return false;

        // Segun move, seteo las direcciones
        colInc = TankRule.getColIncrement(move);
        rowInc = TankRule.getRowIncrement(move);

        if (ret && (rowValue + rowInc) >= 0 && (rowValue + rowInc) < map.GetLength(0) &&
                (colValue + colInc) >= 0 && (colValue + colInc) < map.GetLength(1))
        {
            // Si no hay una pared en esa direccion es valido
            ret = !(map[rowValue + rowInc, colValue + colInc] == CellTypes.WALL);

            if (colInc != 0 && rowInc != 0)
            {
                // Diagonal -> Chequeo que no haya paredes asi no se traba
                ret = ret && !(map[rowValue + rowInc, colValue] == CellTypes.WALL)
                    && !(map[rowValue, colValue + colInc] == CellTypes.WALL);
            }

        }
        else
        {
            ret = false;
        }

        return ret;
	}
	
	public bool move(MoveTypes move) 
    {
        if (!isValidMove(move))
            return false;

        map[rowValue, colValue] = CellTypes.FIELD;

        // Segun move modifico mi posicion
        colValue += TankRule.getColIncrement(move);
        rowValue += TankRule.getRowIncrement(move);

        map[rowValue, colValue] = CellTypes.PLAYER;

		return true;
	}
	
	public bool equals(System.Object arg0)
    {
        FF3d_TankState state = (FF3d_TankState)arg0;

        return (state.colValue == colValue && state.rowValue == rowValue);// && map.Equals(state.map));
	}
	
	public String toString()
    {
        string s = "";
        for (int r = 0; r < map.GetLength(0); r++)
            {
            for (int c = 0; c < map.GetLength(1); c++)
            {
                s = s + " " + map[r, c];
            }
            s = s + "\n";
        }
		return  "Row: " + rowValue + " Col: " + colValue;
	}

    public FF3d_TankState duplicate()
    {
        return new FF3d_TankState(map, rowValue, colValue, flagsPos);
	}
		
	public bool isGoal()
    {
        foreach (PosRowCol flag in flagsPos)
        {
            if ((flag.colValue == colValue) && (flag.rowValue == rowValue))
            {
                return true;
            }
        }

        return false;
	}

    public List<PosRowCol> getFlags()
    {
        return flagsPos;
    }

    public PosRowCol getRowCol()
    {
        return new PosRowCol(rowValue, colValue);
    }

}

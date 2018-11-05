using UnityEngine;
using System;
using System.Collections;

public interface ITable
{
    int rowHeight { get; }

    int TryGetColIndex(string colName);

    string TryGetString(int rowIndex, int colIndex);

    int TryGetInt(int rowIndex, int colIndex);

    Int64 TryGetInt64(int rowIndex, int colIndex);

    float TryGetFloat(int rowIndex, int colIndex);

    bool TryGetBool(int rowIndex, int colIndex);

    Vector2 TryGetVector2(int rowIndex, int colIndex);

    Vector3 TryGetVector3(int rowIndex, int colIndex);

    Color TryGetColor(int rowIndex, int colIndex);
}
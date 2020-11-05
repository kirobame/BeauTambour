using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeData
{
    private List<Vector2> shapePoints;

    public List<Vector2> ShapePoints { get => shapePoints; set => shapePoints = value; }

    public ShapeData()
    {
        shapePoints = new List<Vector2>();
    }    

    public void AddPoint(Vector2 point)
    {
        shapePoints.Add(point);
    }
}

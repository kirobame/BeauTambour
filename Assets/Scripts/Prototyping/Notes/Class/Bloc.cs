using Orion.Prototyping;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bloc : Tilable, IBootable
{
    [SerializeField]private Shape shape;

    public Shape Shape => shape;

    public void BootUp()
    {
        throw new System.NotImplementedException();
    }

    public void ShutDown()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        /*System.Random rand = new System.Random(System.DateTime.Now.Second);
        Array shapes = Enum.GetValues(typeof(Shape));
        shape = (Shape)shapes.GetValue(rand.Next(shapes.Length));*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

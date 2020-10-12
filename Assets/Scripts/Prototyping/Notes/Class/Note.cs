using Orion;
using Orion.Prototyping;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Note : Tilable, IResolvable
{
    private int priority;
    [SerializeField]private Shape shapesMatched;
    public int Priority { get => priority; set => priority = value; }

    public void Resolve()
    {
        PlayArea playArea = Repository.Get<PlayArea>();
        for (int x = Tile.Index.x + 1; x < playArea.Size.x; x++)
        {
            if (playArea[x, Tile.Index.y][TilableType.NoteRecipient].Any())
            {
                Bloc block = playArea[x, Tile.Index.y][TilableType.NoteRecipient].First() as Bloc;
                if ((block.Shape & shapesMatched) == block.Shape)
                {
                    Debug.Log("MATCH!");
                }
                else
                {
                    Debug.Log("NO MATCH!");
                }
                return;
            }
        }
        Debug.Log("NOTHING!");
    }

    // Start is called before the first frame update
    void Start()
    {
        RoundHandler roundHandler =  Repository.Get<RoundHandler>();
        roundHandler.TryEnqueue(this);
    }

    // Update is called once per frame
    void Update()
    {
    }
}

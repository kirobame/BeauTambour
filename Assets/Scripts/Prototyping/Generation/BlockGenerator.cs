using BeauTambour.Prototyping;
using Orion;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGenerator : SerializedMonoBehaviour, IBootable
{
    public int StartX => Repository.Get<PlayArea>().Size.x - range;
    
    [SerializeField] private int bootUpPriority;
    [SerializeField, Min(2)] private int range = 4;
    [SerializeField] private IProvider<Block> blockProvider;
    
    private PlayArea playArea;
    private List<Block> blocks;

    int IBootable.Priority => bootUpPriority;

    public void BootUp()
    {
        blocks = new List<Block>();
        playArea = Repository.Get<PlayArea>();

        int startX = playArea.Size.x - range;        
        for (int x = startX; x < playArea.Size.x; x++) GenerateColumn(x);

        Repository.Get<RoundHandler>()[PhaseType.Generation].OnStart += OnGenerationPhaseStart;
    }
    public void ShutDown() { }
    
    private void GenerateColumn(int x)
    {
        List<int> possibleY = new List<int>();
        ResetPossibleY(possibleY);
        
        int nbBlockInColumn = Random.Range(2, playArea.Size.y+1);
        for (int nb = 0; nb < nbBlockInColumn; nb++)
        {
            Block block = blockProvider.GetInstance();
            
            block.BootUp();
            block.Place(new Vector2Int(x, GetPossibleNumberFromList(possibleY)));
            
            blocks.Add(block);
        }
    }
    private void ResetPossibleY(List<int> possibleY)
    {
        possibleY.Clear();
        for (int y = 0; y < playArea.Size.y; y++)
        {
            possibleY.Add(y);
        }        
    }
    private int GetPossibleNumberFromList(List<int> possible)
    {
        int index = Random.Range(0, possible.Count);
        int result = possible[index];
        possible.RemoveAt(index);
        return result;
    }

    private void OnGenerationPhaseStart()
    {
        var rythmHandler = Repository.Get<RythmHandler>();
        
        rythmHandler.MakePlainEnqueue(Generate, 1);
        rythmHandler.MakeStandardEnqueue(Generate, 1);
    }
    private void Generate(int beat, double offset)
    {
        if (beat == 0) MoveBlocks();
        if (beat == 1)
        {
            DestroyInvalidBlocks();
            GenerateColumn(playArea.Size.x - 1);
        }
    }
    private void Generate(double time, double offset)
    {
        foreach (var block in blocks) block.Move(time / (1f - offset));
    }

    private void MoveBlocks()
    {
        for (int index = 0; index < blocks.Count; index++)
        {
            int newX = (int)blocks[index].Tile.Index.x - 1;
            int newY = (int)blocks[index].Tile.Index.y;
            
            var block = blocks[index];
            block.SetTweenMarks(block.Tile.Position, playArea[newX, newY].Position);
        }
    }
    private void DestroyInvalidBlocks()
    {
        for (var i = 0; i < blocks.Count; i++)
        {
            if (blocks[i].Tile.Index.x >= StartX) continue;
            
            blocks[i].ShutDown();
            blocks.RemoveAt(i);
            i--;
        }
    }
}

using BeauTambour.Prototyping;
using Orion;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGenerationHandler : MonoBehaviour, IBootable
{
    [SerializeField]private Bloc[] prefabBlocs;
    private int nbMaxColumnBlockSide = 4;
    private PlayArea playArea;
    private RoundHandler round;
    private List<Bloc> blocks;


    public void BootUp()
    {
        playArea = Repository.Get<PlayArea>();
        round = Repository.Get<RoundHandler>();
        blocks = new List<Bloc>();
        int startX = playArea.Size.x - nbMaxColumnBlockSide;        
        for (int x = startX; x < playArea.Size.x; x++)
        {
            GenerateColumn(x);
        }
    }

    public void ShutDown()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {       

    }

    private int GetPossibleNumberFromList(List<int> possible)
    {
        int index = Random.Range(0, possible.Count);
        int result = possible[index];
        possible.RemoveAt(index);
        return result;
    }

    private void ResetPossibleY(List<int> possibleY)
    {
        possibleY.Clear();
        for (int y = 0; y < playArea.Size.y; y++)
        {
            possibleY.Add(y);
        }        
    }
    private void GenerateColumn(int x)
    {
        List<int> possibleY = new List<int>();
        ResetPossibleY(possibleY);
        int nbBlockInColumn = Random.Range(2, playArea.Size.y+1);
        for (int nb = 0; nb < nbBlockInColumn; nb++)
        {
            Bloc block = Instantiate(prefabBlocs[Random.Range(0, prefabBlocs.Length)]);
            block.Place(new Vector2Int(x, GetPossibleNumberFromList(possibleY)));
            blocks.Add(block);
        }
    }
    [Button]
    private void OnRoundEnding()
    {
        for (int index = 0; index < blocks.Count; index++)
        {
            int newX = (int)blocks[index].Tile.Index.x - 1;
            int newY = (int)blocks[index].Tile.Index.y;
            if (newX < playArea.Size.x - nbMaxColumnBlockSide)
            {
                Bloc toRemove = blocks[index];
                blocks.Remove(blocks[index]);
                toRemove.ShutDown();
                index--;
            }
            else
            {
                blocks[index].Position = playArea[newX, newY].Position;
                blocks[index].ActualizeTiling();
            }       
        }
    }

    [Button]
    private void OnRoundEnded()
    {
        GenerateColumn(playArea.Size.x - 1);
    }
}

using UnityEngine;

namespace Deprecated
{
    //[CreateAssetMenu(fileName = "NewInterlocutor", menuName = "Beau Tambour/Characters/Interlocutor")]
    public class Interlocutor : Character
    {
        public Block CurrentBlock => blocks[currentBlockIndex];
        public bool isAtLastBlock => currentBlockIndex == blocks.Length - 1;
        public int blockIndex => currentBlockIndex;
        
        [SerializeField] private Block[] blocks;
        private int currentBlockIndex;

        public void PushTrough()
        {
            Debug.Log("Pushing through");
            currentBlockIndex++;
        }

        public override void BootUp() => currentBlockIndex = 0;
    }
}
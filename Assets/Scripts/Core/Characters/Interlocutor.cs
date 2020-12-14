using UnityEngine;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewInterlocutor", menuName = "Beau Tambour/Characters/Interlocutor")]
    public class Interlocutor : Character
    {
        public Block CurrentBlock => blocks[currentBlockIndex];
        public bool isAtLastBlock => currentBlockIndex == blocks.Length - 1;
        
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
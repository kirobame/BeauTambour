using Flux;
using UnityEngine;

namespace Deprecated
{
    //[ItemPath("Musicians/Attribute Generation")]
    //[ItemName("Attributes for Musician")]
    public class AttributeGenerationInput : CharacterInput<Musician>
    {
        [SerializeField] private CompoundAttributeGenerator generator;
        
        public override void Execute() => target.SetGenerator(generator);
    }
}
using System.Linq;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

namespace Flux.Editor
{
    [CustomEditor(typeof(Sequencer))]
    public class SequencerEditor : FluxEditor
    {
        protected override void ExtendInspectorGUI()
        {

        }
    }
}
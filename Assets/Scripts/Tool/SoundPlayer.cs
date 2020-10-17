using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BeauTambour.Tooling
{
    public class SoundPlayer : Executable
    {
        [SerializeField] private AudioSource target;

        public override void Execute() => target.Play();
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using Orion;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public class SessionHandler : MonoBehaviour
    {
        private List<IBootable> bootables = new List<IBootable>();

        void Start()
        {
            var primaryBootables = new IBootable[]
            {
                Repository.Get<RythmHandler>(),
                Repository.Get<RoundHandler>(),
            };
            Register(primaryBootables);
        }

        public void Begin()
        {
            for (int i = 0; i < bootables.Count; i++)
            {
                bootables[i].BootUp();
            }
        }
        public void End()
        {
            for (int i = 0; i < bootables.Count; i++)
            {
                bootables[i].ShutDown();
            }
        }

        public void Register(IBootable bootable) => Register(new IBootable[] {bootable});
        public void Register(IEnumerable<IBootable> additions)
        {
            bootables.AddRange(additions);
            bootables.Sort(SortByDescendingPriority);
        }
        public void Unregister(IBootable bootable)
        {
            bootables.Remove(bootable);
            bootables.Sort(SortByDescendingPriority);
        }

        private int SortByDescendingPriority(IBootable firstBootable, IBootable secondBootable)
        {
            if (firstBootable.Priority == secondBootable.Priority) return 0;
            else if (firstBootable.Priority < secondBootable.Priority) return -1;
            else return 1;
        }
    }
}
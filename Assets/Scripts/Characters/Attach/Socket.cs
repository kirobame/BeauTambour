using Spine;
using Spine.Unity;
using UnityEngine;

namespace BeauTambour
{
    public class Socket : Attach
    {
        public override Transform Value => attach;
        public Vector2 Position
        {
            get
            {
                var x = link.WorldX * root.transform.localScale.x;
                var y = link.WorldY * root.transform.localScale.y;

                return (Vector2)root.position + new Vector2(x, y) + offset;
            }
        }
        
        [SerializeField] private SkeletonMecanim spine;
        
        [Space, SerializeField] private Transform root;
        [SerializeField] private Transform attach;
        
        [Space, SerializeField] private string bone;

        private Bone link;
        private Vector2 offset;

        void Awake()
        {
            link = spine.Skeleton.FindBone(bone);
            offset = Vector2.zero;
            
            offset = (Vector2)attach.position - Position;
        }

        void Update() => attach.position = Position;
    }
}
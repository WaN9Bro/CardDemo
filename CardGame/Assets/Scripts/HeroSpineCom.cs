using Spine;
using Spine.Unity;

namespace MyGame
{
    public class HeroSpineCom : IHeroComponent
    {
        public HeroObj HeroObj { get; private set; }
        private SkeletonAnimation _skeleton;
        private string _lastAnimName;
        
        public void Initialize(HeroObj heroObj)
        {
            HeroObj = heroObj;
            _skeleton = heroObj.GetComponent<SkeletonAnimation>();
            _skeleton.AnimationState.Complete += OnAnimCompleted;
            PlayAnim("Stand");
        }

        public void PlayAnim(string animName,bool loop = true)
        {
            _lastAnimName = animName;
            _skeleton.AnimationState.SetAnimation(0, animName,loop);
        }

        private void OnAnimCompleted(TrackEntry _)
        {
            // 播放玩动画之后恢复到站立动画
            if (_lastAnimName != "Stand")
            {
                PlayAnim("Stand");
            }
        }
        
        public void Clear()
        {
            _skeleton.AnimationState.Complete -= OnAnimCompleted;
            _skeleton = null;
            HeroObj = null;
        }

    }
}
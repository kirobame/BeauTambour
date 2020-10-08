using System.Collections;

namespace Orion
{
    public class EventEffect : Feedback
    {
        public OrionEvent callback = new OrionEvent();
        
        public override IEnumerator GetRoutine()
        {
            callback.Invoke();
            yield break;
        }
    }
}
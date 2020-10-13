namespace BeauTambour.Prototyping
{
    public abstract class PlayerActionRelay<T> where T : PlayerAction
    {
        protected T source;

        public void SetSource(T source) => this.source = source;
        
        public abstract bool CanBeExecuted();
        
        public abstract void Execute(int beat, double offset);
        public abstract void ResolveTime(double time, double offset);
    }
}
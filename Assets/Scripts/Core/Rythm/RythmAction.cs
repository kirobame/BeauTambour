namespace BeauTambour
{
    public abstract class RythmAction : IRythmQueueable
    {
        public RythmAction(int duration) => Duration = duration;
        
        public double StartingTime { get; private set; }
        public int StartingBeat { get; private set; }
        
        public double Duration { get; private set; }
        protected double Offset { get; private set; }

        public abstract bool IsDone { get; }
        
        public abstract void Tick(double time);
        public abstract void Beat(int count);

        void IRythmQueueable.Prepare(double startingTime, int startingBeat, double offset)
        {
            StartingTime = startingTime;
            StartingBeat = startingBeat;

            Offset = offset;
            if (offset > 0) Duration -= offset;
        }
    }
}
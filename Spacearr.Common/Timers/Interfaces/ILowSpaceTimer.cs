namespace Spacearr.Common.Timers.Interfaces
{
    public interface ILowSpaceTimer
    {
        /// <summary>
        /// Start the timer.
        /// </summary>
        void Instantiate();

        /// <summary>
        /// Stop the timer.
        /// </summary>
        void DeInstantiate();
    }
}
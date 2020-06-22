namespace Spacearr.Common.Interfaces
{
    public interface INotificationTimer
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
namespace Spacearr.Common.Services.Interfaces
{
    public interface INotificationTimerService
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
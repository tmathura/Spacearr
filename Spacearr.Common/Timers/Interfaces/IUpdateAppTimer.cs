using Spacearr.Common.Enums;

namespace Spacearr.Common.Timers.Interfaces
{
    public interface IUpdateAppTimer
    {
        /// <summary>
        /// Start the timer.
        /// </summary>
        void Instantiate(UpdateType updateType);

        /// <summary>
        /// Stop the timer.
        /// </summary>
        void DeInstantiate();
    }
}
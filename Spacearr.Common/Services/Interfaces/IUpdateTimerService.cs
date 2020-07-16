using Spacearr.Common.Enums;

namespace Spacearr.Common.Services.Interfaces
{
    public interface IUpdateTimerService
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
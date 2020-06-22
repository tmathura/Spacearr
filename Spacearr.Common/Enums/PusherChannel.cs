namespace Spacearr.Common.Enums
{
    /// <summary>
    /// The different channels that is used by Pusher. Each channel is specific to a different process.
    /// </summary>
    public enum PusherChannel
    {
        SpacearrChannel,
        SpacearrWorkerServiceWindowsChannel,
        SpacearrWorkerServiceWindowsNotificationChannel
    }
}

namespace Spacearr.Common.Enums
{
    /// <summary>
    /// The different events that is used by Pusher. This is used to identify between the different applications when making a Pub/Sub connection.
    /// </summary>
    public enum PusherEvent
    {
        SpacearrEvent,
        WorkerServiceEvent
    }
}

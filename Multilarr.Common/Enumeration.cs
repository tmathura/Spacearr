namespace Multilarr.Common
{
    public static class Enumeration
    {
        public enum CommandType
        {
            ComputerDrivesCommand,
            ComputerDrivesLowCommand
        }

        public enum LogType
        {
            Info,
            Warn,
            Error
        }

        public enum PusherChannel
        {
            MultilarrChannel,
            MultilarrWorkerServiceWindowsChannel,
            MultilarrWorkerServiceWindowsNotificationChannel
        }

        public enum PusherEvent
        {
            MultilarrEvent,
            WorkerServiceEvent
        }
    }
}

namespace Spacearr.Common
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
            SpacearrChannel,
            SpacearrWorkerServiceWindowsChannel,
            SpacearrWorkerServiceWindowsNotificationChannel
        }

        public enum PusherEvent
        {
            SpacearrEvent,
            WorkerServiceEvent
        }
    }
}

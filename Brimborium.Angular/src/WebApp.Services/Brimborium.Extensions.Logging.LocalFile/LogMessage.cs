namespace Brimborium.Angular.WebApp.Brimborium.Extensions.Logging.LocalFile {
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    internal readonly struct LogMessage {
        public LogMessage(DateTimeOffset timestamp, string message) {
            this.Timestamp = timestamp;
            this.Message = message;
        }

        public DateTimeOffset Timestamp { get; }
        public string Message { get; }
    }
}

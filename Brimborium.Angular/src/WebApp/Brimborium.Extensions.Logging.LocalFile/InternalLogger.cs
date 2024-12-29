namespace Brimborium.Angular.WebApp.Brimborium.Extensions.Logging.LocalFile {
    /// <summary>
    /// Internal Logger - if inner failed.
    /// </summary>
    public class InternalLogger {
        private static InternalLogger? _Instance;

        /// <summary>
        /// Singleton
        /// </summary>
        public static InternalLogger GetInstance()
            => _Instance ??= new InternalLogger();

        private InternalLogger() { }

        /// <summary>
        /// System.Console.Error.WriteLine
        /// </summary>
        /// <param name="error">the thrown exception.</param>
        public void Fail(Exception error) {
            if (this.OnFail is { } onFail) {
                try {
                    onFail(error);
                } catch {
                }
            } else {
                if (error is AggregateException aggregateException) {
                    aggregateException.Handle(static (error) => true);
                    Console.Error.WriteLine(aggregateException.ToString());
                } else {
                    Console.Error.WriteLine(error.ToString());
                }
            }
        }

        /// <summary>
        /// Called by <see cref="Fail(Exception)"/>
        /// </summary>
        public Action<Exception>? OnFail { get; set; }
    }
}

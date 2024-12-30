namespace Brimborium.Angular.WebApp.Brimborium.Extensions.Logging.LocalFile {
    internal sealed class NullScope : IDisposable {
        internal static NullScope Instance { get; } = new NullScope();

        private NullScope() { }

        public void Dispose() { }
    }
}
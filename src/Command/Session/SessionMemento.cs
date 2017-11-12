using System;

namespace AstralKeks.Workbench.Command
{
    internal class SessionMemento
    {
        private static readonly object _lock = new object();
        private static SessionMemento _saved;

        public static void Save(SessionMemento memento)
        {
            if (memento != null)
            {
                lock (_lock)
                    _saved = memento; 
            }
        }

        public static SessionMemento Load()
        {
            lock (_lock)
                return _saved;
        }

        public SessionMemento(string directory, string location)
        {
            if (string.IsNullOrWhiteSpace(directory))
                throw new ArgumentException("Invalid working directory", nameof(directory));
            if (string.IsNullOrWhiteSpace(location))
                throw new ArgumentException("Invalid location", nameof(location));

            Directory = directory;
            Location = location;
        }

        public string Directory { get; }
        public string Location { get; }
    }
}

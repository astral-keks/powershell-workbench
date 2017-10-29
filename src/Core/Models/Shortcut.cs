using System;

namespace AstralKeks.Workbench.Models
{
    public class Shortcut
    {
        public Shortcut(string name, string location, string target)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Shortcut name is not provided", nameof(name));
            if (string.IsNullOrWhiteSpace(location))
                throw new ArgumentException("Shortcut location is not provided", nameof(location));
            if (string.IsNullOrWhiteSpace(target))
                throw new ArgumentException("Shortcut target is not provided", nameof(target));

            Name = name;
            Location = location;
            Target = target;
        }

        public string Name { get; }

        public string Location { get; }

        public string Target { get; }

        public bool IsMatch(string query)
        {
            return ToString().IndexOf(query ?? string.Empty, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public override string ToString()
        {
            return $"{Name}@{Location}";
        }
    }
}

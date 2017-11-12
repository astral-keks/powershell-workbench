namespace AstralKeks.Workbench.Models
{
    public class Backup
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string[] Sources { get; set; }
        public string Destination { get; set; }
    }
}

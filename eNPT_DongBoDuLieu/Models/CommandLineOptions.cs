using CommandLine;

namespace eNPT_DongBoDuLieu.Models
{
    public class CommandLineOptions
    {
        [Value(index: 0, Required = true, HelpText = "Path directory application.")]
        public string Path { get; set; }
    }
}

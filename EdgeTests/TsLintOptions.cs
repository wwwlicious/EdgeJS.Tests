namespace EdgeTests
{
    using System.IO;

    public class TsLintOptions
    {
        public TsLintOptions(string configFile)
        {
            // grab a config file
            this.Formatter = "json";
            this.Configuration = !File.Exists(configFile) ? "{}" : File.ReadAllText(configFile);
        }

        public string Configuration { get; set; }

        public string Formatter { get; set; }

        public override string ToString()
        {
            // cheaper than json-ing, is that even a thing?!
            return string.Format(@"{{formatter: '{0}', configuration: {1}}}", this.Formatter, this.Configuration);
        }
    }
}
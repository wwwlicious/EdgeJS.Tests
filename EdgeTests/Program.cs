namespace EdgeTests
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public class Program
    {
        private static readonly string basePath = AppDomain.CurrentDomain.BaseDirectory;

        public static async void Start()
        {
            // selecting js files cos js is valid ts right?!
            var files = Directory.GetFiles(basePath + "scripts", "*.js").ToArray();

            // I want maximum power scotty!
            var tasks = files.AsParallel().Select(async f => await TsLinter.Execute(f));
            var result = tasks.Select(r => r.Result).ToArray();

            // my files are perfect right?
            Console.WriteLine("Linting finished, press any key to view results");
            Console.ReadKey();

            Array.ForEach(result, Console.WriteLine);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        public static void Main(string[] args)
        {
            // go go go!
            Task.Run((Action)Start).Wait();
        }
    }
}

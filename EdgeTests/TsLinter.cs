 namespace EdgeTests
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    public class TsLinter
    {
        private static readonly string basePath = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly TsLintOptions options = new TsLintOptions(basePath + "\\tslint.json");
        private static readonly EdgeProxyLoader loader = new EdgeProxyLoader(basePath + "\\proxy\\tsLintProxy.js", options.ToString());

        /// <summary>
        /// reads the file (nodejs fs can do this also but didn't seem to make a difference)
        /// </summary>
        /// <param name="fileName">where da script?</param>
        /// <returns>lint results</returns>
        public static async Task<TsLintResult> Execute(string fileName)
        {
            Console.WriteLine("Linting {0} on thread {1}", Path.GetFileName(fileName), Thread.CurrentThread.ManagedThreadId);
            
            // gets the file and passes to nodejs, waits for result then returns
            var contents = File.ReadAllText(fileName);

            // time and run
            var task = await loader.Execute(new { fileName, contents });

            var original = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Linted {0} on thread {1}", Path.GetFileName(fileName), Thread.CurrentThread.ManagedThreadId);
            Console.ForegroundColor = original;
            return new TsLintResult(task);
        }

        /// <summary>
        /// Just a convenience do get values from a dynamic result type
        /// </summary>
        public class TsLintResult
        {
            public TsLintResult(dynamic result)
            {
                this.FailureCount = result.failureCount;
                this.Format = result.format;
                this.Output = JsonConvert.DeserializeObject<TsLintError[]>(result.output);
            }

            public int FailureCount { get; set; }

            public string Format { get; set; }

            public TsLintError[] Output { get; set; }

            public override string ToString()
            {
                var sb = new StringBuilder();
                Array.ForEach(Output, error => sb.AppendLine(error.ToString()));
                sb.AppendLine(string.Format("Failures: {0}", FailureCount));

                return sb.ToString();
            }
        }

        public class TsLintError
        {
            public string Name { get; set; }

            public string Failure { get; set; }

            public FilePosition StartPosition { get; set; }

            public FilePosition EndPosition { get; set; }

            public string RuleName { get; set; }

            public override string ToString()
            {
                return string.Format("Rule: {0} /t Failure: {1} /n Name: {2} /n Position {3}", this.RuleName, this.Failure, this.Name, this.StartPosition);
            }
        }

        public class FilePosition
        {
            public int Position { get; set; }

            public int Line { get; set; }

            public int Character { get; set; }

            public override string ToString()
            {
                return string.Format("Line: {0} Char {1}", this.Line, this.Character);
            }
        }
    }
}
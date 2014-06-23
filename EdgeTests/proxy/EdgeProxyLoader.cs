namespace EdgeTests
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    using EdgeJs;

    public class EdgeProxyLoader
    {
        private readonly Func<object, Task<object>> edgeCall;

        public EdgeProxyLoader(string proxyFileName, string options)
        {
            // grab the proxy script and create the Func we send to nodejs using edge.js
            var proxyScript = File.ReadAllText(proxyFileName);
            proxyScript = proxyScript.Replace("{options}", options);
            this.edgeCall = Edge.Func(proxyScript);
        }

        public Task<object> Execute(object data)
        {
            // the black magic bit!
            Console.WriteLine("EdgeProxyLoader.Executing on thread {0}", Thread.CurrentThread.ManagedThreadId);
            return this.edgeCall(data);
        }
    }
}
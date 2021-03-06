using System;
using Wasmtime;

namespace HelloExample
{
    class Program
    {
        static void Main(string[] args)
        {
            using var engine = new Engine();
            using var store = new Store(engine);
            using var module = store.LoadModuleText("memory.wat");
            using var host = new Host(engine);

            host.DefineFunction(
                "",
                "log",
                (Caller caller, int address, int length) => {
                    var message = caller.GetMemory("mem").ReadString(address, length);
                    Console.WriteLine($"Message from WebAssembly: {message}");
                }
            );

            using dynamic instance = host.Instantiate(module);
            instance.run();
        }
    }
}

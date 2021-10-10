using System.Threading.Tasks;

namespace Tutorial
{
    static class Program
    {
        public static async Task Main(string[] args)
            => await Startup.RunAsync(args);
    }
}

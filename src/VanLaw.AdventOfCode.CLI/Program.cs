using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using VanLaw.AdventOfCode.CLI.Interfaces;

namespace VanLaw.AdventOfCode.CLI
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            return await CreateHostBuilder(args)
               .RunCommandLineApplicationAsync<Program>(args, (app) =>
               {
                   app
                       .GetServices<ICommand>()
                       .ToList()
                       .ForEach(c =>
                       {
                           c.Configure(app);
                       });
               }).ConfigureAwait(false);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
                Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.ConfigureCommands();
                });
    }
}
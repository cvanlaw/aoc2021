using System;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;

using VanLaw.AdventOfCode.CLI.Interfaces;

namespace VanLaw.AdventOfCode.CLI
{
    public static class CommandRegistration
    {
        public static IServiceCollection ConfigureCommands(this IServiceCollection services)
        {
            var commandInterface = typeof(ICommand);
            var commands = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => commandInterface.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            foreach (var command in commands)
            {
                services.AddSingleton(commandInterface, command);
            }

            return services;
        }
    }
}
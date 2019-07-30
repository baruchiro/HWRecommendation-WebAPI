using System;
using AlgorithmManager;
using DocoptNet;
using Microsoft.Extensions.Configuration;

namespace Trainer
{
    class Program
    {
        private static uint minutes;
        private static IConfigurationRoot _configuration;

        private const string usage = @"Trainer.

    Usage:
      Trainer.exe <minutes> <output>

    Options:
      -h --help     Show this screen.
      --version     Show version.
      --output      Output dir to publish trained models
    ";

        private static void Main(string[] args)
        {
            BuildConfiguration();
            ParseArguments(args);

            using (var trainer = new Trainer(_configuration))
            {
                try
                {
                    trainer.TrainAll(minutes);
                }
                catch (Exception e)
                {
                    trainer.Cancel();
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        private static void BuildConfiguration()
        {
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection()
                .Build();
        }

        private static void ParseArguments(string[] args)
        {
            var arguments = new Docopt().Apply(usage, args, exit: true);

            minutes = Convert.ToUInt32(arguments["<minutes>"].AsInt);

            if (arguments["<output>"].Value is string output)
            {
                _configuration[ModelSaver.KEY_MODEL_SAVE_PATH] = output;
            }
        }
    }
}

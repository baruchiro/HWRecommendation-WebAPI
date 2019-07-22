using System;
using DocoptNet;

namespace Trainer
{
    class Program
    {
        private static uint minutes;
        private static string outputDir;

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
            ParseArguments(args);

            using (var trainer = new Trainer(outputDir))
            {
                try
                {
                    trainer.TrainAll(minutes);

                    trainer.WaitAll();
                }
                catch (Exception e)
                {
                    trainer.Cancel();
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        private static void ParseArguments(string[] args)
        {
            var arguments = new Docopt().Apply(usage, args, exit: true);

            minutes = Convert.ToUInt32(arguments["<minutes>"].AsInt);
            outputDir = arguments["<output>"].Value as string;
        }
    }
}

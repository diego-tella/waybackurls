using System;

namespace waybackurls_csharp
{
    class Program
    {
        public static bool verbose = false;
        public static bool output = false;
        public static string url;

        static void Main(string[] args)
        {
            VerifyArgs(args);
            wayBackUrls wayBackMachine = new wayBackUrls();
            wayBackMachine.start(verbose, output, url);
        }

        private static void VerifyArgs(string[] args)
        {
            if (args.Length < 1)
            {
                help();
            }
            else
            {
                foreach (string i in args)
                {
                    switch (i)
                    {
                        case "-h":
                            help();
                            break;
                        case "-v":
                            verbose = true;
                            break;
                        case "-o":
                            output = true;
                            break;
                        default:
                            url = i;
                            break;
                    }
                }
            }
        }
        private static void help()
        {
            Console.WriteLine("Usage: waybackurls-csharp.exe example.com");
            Console.WriteLine("Arguments: ");
            Console.WriteLine("[-v] - Verbose. A colored output with statuscode, timestamp and mimetype");
            Console.WriteLine("[-o] - Output. Save all output to a text file");
            Console.WriteLine("[-h] - Help. Show help menu");
            Console.WriteLine("Example with all args: waybackurls-csharp.exe example.com -v -o");
            Environment.Exit(0);
        }
        
    }
}

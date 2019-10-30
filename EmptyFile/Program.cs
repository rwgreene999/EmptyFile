using System;
using System.Collections.Generic;
using System.IO;

namespace EmptyFile
{
    class Program
    {

        static void Main(string[] args)
        {
            Options options = new Options();
            foreach (var arg in args)
            {
                var arg2 = arg.ToLower();
                if (arg2.StartsWith("-") || arg2.StartsWith("/"))
                {
                    if (arg2[1] == '?' || arg2[1] == 'h')
                    {
                        options.Help = true;
                    }
                    if (arg2[1] == 'v')
                    {
                        options.Verbose = true;
                    }
                    else if (arg2[1] == 'q')
                    {
                        options.Ask = false;
                    }
                    else if (arg2[1] == 'w')
                    {
                        options.Wait = true;
                    }
                }
                else
                {
                    options.filenames.Add(arg2);

                }

            }

            Process(options);
            if (options.Wait == true)
            {
                Console.WriteLine("Press <enter> to end");
                Console.ReadKey();
            }

        }

        private static void Process(Options options)
        {
            if (options.Help)
            {
                ShowHelp();
                return;
            }
            foreach (string filename in options.filenames)
            {
                bool doit = false;
                if (options.Ask)
                {
                    Console.WriteLine($"Do you want to delete file {filename} ?");
                    var rsp = Console.ReadLine();
                    if (rsp.ToString().ToLower() == "y")
                    {
                        doit = true;
                    }
                    else
                    {
                        doit = false;
                    }
                }
                else
                {
                    doit = true;
                }

                if (doit)
                {
                    try
                    {
                        if (File.Exists(filename))
                        {
                            FileStream fs = File.Open(filename, FileMode.Truncate, FileAccess.Write);
                            StreamWriter sw = new StreamWriter(fs);
                            // sw.WriteLine(""); 
                            sw.Flush();
                            sw.Close();
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine($"Failed to empty out {filename}.");
                        Console.WriteLine($"It may be in use by another file program.");
                        Console.WriteLine("");
                    }
                }
            }
        }

        private static void ShowHelp()
        {
            string[] Help = new string[]
            {
                "",
                "EmptyFile < a program to empty (zero out) the files provided ",
                "",
                "Syntax: emptyfile [/q] [/?] [f:fileContainingFilenames] [filename1] [filename2] ...",
                "",
                "    [/?] = Show this help ",
                "    [/q] = Dont ask yes or no on each file ",
                "    [filename1] = filenames of files to empty out",
                "    [/f:fileContainingFilenames] = a file where each row contains one filename ",
                ""
            };
            foreach (var s in Help)
            {
                Console.WriteLine(s);
            }
        }
    }


    class Options
    {
        public bool Ask { get; set; } = true;
        public bool Verbose { get; set; } = false;
        public List<string> filenames { get; set; } = new List<string>() { };
        public bool Wait { get; internal set; }
        public bool Help { get; internal set; }
    }
}

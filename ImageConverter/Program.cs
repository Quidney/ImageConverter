using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageMagick;

namespace ImageConverter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1 && args[0] == "-h")
            {
                Console.WriteLine("Image Converter\nby Quidney\n");
                
                Console.Write("Convert Single File:\nImageConverter.exe input.jpg output.png\nExample: ");
                Console.ForegroundColor = ConsoleColor.Green; Console.Write("ImageConverter.exe "); Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Blue; Console.Write("cuteKitty.jpeg "); Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine("cuteKitty.png\n"); Console.ResetColor();

                Console.Write("Convert a Folder Full Of Images:\nImageConverter.exe inputFolder outputFolder inputExtension outputExtension\nExample: ");
                Console.ForegroundColor = ConsoleColor.Green; Console.Write("ImageConverter.exe "); Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Blue; Console.Write("inputDirectory "); Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("C:\\Users\\%username%\\Desktop\\outputFolder\\ "); Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Blue; Console.Write("jpeg "); Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine("png\n"); Console.ResetColor();
            }
            else if (args.Length == 2) 
            {
                try
                {
                    string[] file1 = args[0].Split('.');
                    string file1Name = file1[0];
                    string file1Extension = file1[1];

                    string[] file2 = args[1].Split('.');
                    string file2Name = file2[0];
                    string file2Extension = file2[1];

                    ConvertFiles1(args, args[0], file2Name, file2Extension);
                }
                catch (Exception e)
                {
                    Error(e);
                    return;
                }
            }
            else if (args.Length == 4)
            {
                try
                {
                    if (!Directory.Exists(args[0]))
                    {
                        Console.WriteLine("Input Directory does not exist.");
                        return;
                    }
                    else if (!Directory.Exists(args[1]))
                        Directory.CreateDirectory(args[1]);

                    ConvertFiles2(args);                    
                }
                catch (Exception e)
                {
                    Error(e);
                    return;
                }
            }
            else
            {
                IncorrectSyntax();
            }
        }

        private static void IncorrectSyntax(string debug = "")
        {
            Console.WriteLine("Incorrect usage, enter \"ImageConverter.exe -h\" for more information.");
        }

        private static void Error(Exception e)
        {
            Console.WriteLine("Error: " + e.Message);
        }

        private static void ConvertFiles1(string[] args, string file1, string file2Name, string file2Extension)
        {
            MagickFormat magickFormat;
            if (!Enum.TryParse(file2Extension, true, out magickFormat))
            {
                Console.WriteLine($"Unsupported file extension {file2Extension}");
                return;
            }

            using (MagickImage image = new MagickImage(file1))
            {
                image.Format = magickFormat;

                image.Write(args[1]);
            }

        }

        private static void ConvertFiles2(string[] args)
        {
            foreach (string file in Directory.GetFiles(args[0], $"*.{args[2]}"))
            {
                MagickFormat magickFormat;

                if (!Enum.TryParse(args[3], true, out magickFormat))
                {
                    Console.WriteLine($"Unsupported file extension .{args[3]}");
                    continue;
                }

                using (MagickImage image = new MagickImage(file))
                {
                    image.Format = magickFormat;

                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);

                    image.Write(Path.Combine(args[1], $"{fileNameWithoutExtension}.{args[3]}"));
                }
            }
        }
    }
}

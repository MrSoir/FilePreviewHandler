using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace PreviewHandlerTests
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                Console.WriteLine("invalid input! - no command line arguments delivered!");
                System.Environment.Exit(1);
            }
            if (args.Length % 4 > 0)
            {
                Console.WriteLine("invalid input! - command line arguments must be pairs of source-path & target_path & width & height!");
                System.Environment.Exit(1);
            }
            for (int i = 0; i < args.Length; i += 4)
            {
                string sourcepath = args[i];
                string targetpath = args[i + 1];
                int width = Convert.ToInt32(args[i + 2]);
                int height = Convert.ToInt32(args[i + 3]);

                bool sourceExists = File.Exists(sourcepath);
                bool targetDirExists = Directory.Exists(Path.GetDirectoryName(targetpath));
                bool targetExists = File.Exists(targetpath);

                if (sourceExists && targetDirExists && !targetExists)
                {
                    string basename = Path.GetFileNameWithoutExtension(sourcepath);
                    string basePath = Path.GetDirectoryName(sourcepath);
                    targetpath = GetValidTargetPath(args[i + 1]);
                    Bitmap thumbnail = ThumbnailGenerator.WindowsThumbnailProvider.GetThumbnail(
                        sourcepath, width, height, ThumbnailGenerator.ThumbnailOptions.None);
                    thumbnail.Save(targetpath, ImageFormat.Png);
                    thumbnail.Dispose();
                    Console.WriteLine("successfully created thumbnail of '" + sourcepath + "' -> '" + targetpath + "'");
                }else
                {
                    if (!sourceExists)
                        Console.WriteLine("source does not exist! - " + sourcepath);
                    if (!targetDirExists)
                        Console.WriteLine("target-dir does not exist! - " + targetpath);
                    if (targetExists)
                        Console.WriteLine("target does already exist! - " + targetpath);
                }
            }
        }
        static string GetValidTargetPath(string targetpath)
        {
            if (Path.GetExtension(targetpath).ToLower() == ".png")
            {
                return targetpath;
            }
            string basename = Path.GetFileNameWithoutExtension(targetpath);
            string basePath = Path.GetDirectoryName(targetpath);
            return basePath + "\\" + basename + ".png";
        }
    }
}
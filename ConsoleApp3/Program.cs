using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

class Program
{
    static string directoryPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\Images";

    static void Main(string[] args)
    {
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        int select = 0;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("ScreenShot => 'Enter'");
            Console.WriteLine("All images => 'P'");
            Console.WriteLine("   Exit    => 'Escape'");

            ConsoleKeyInfo key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.Enter)
            {
                int i = 0;
                string screenshotPath;
                while (true)
                {
                    screenshotPath = Path.Combine(directoryPath, i == 0 ? "screenshot.png" : $"screenshot{i}.png");
                    if (!File.Exists(screenshotPath))
                        break;
                    i++;
                }
                CaptureScreenshot(screenshotPath, 1920, 1080);
                Console.WriteLine($"Screenshot saved to: {screenshotPath}");
                Console.WriteLine("Press any key to Continue.");
                Console.ReadKey();
            }
            else if (key.Key == ConsoleKey.P)
            {
                string[] imageFiles = Directory.GetFiles(directoryPath);
                ShowImages(imageFiles, select);
            }
            else if (key.Key == ConsoleKey.Escape)
            {
                break;
            }
        }
    }

    static void CaptureScreenshot(string path, int width, int height)
    {
        using (Bitmap bitmap = new Bitmap(width, height))
        {
            using (Graphics g = Graphics.FromImage(bitmap))
                g.CopyFromScreen(Point.Empty, Point.Empty, new Size(width, height));

            bitmap.Save(path, ImageFormat.Png);
        }
    }

    static void ShowImages(string[] imageFiles, int select)
    {
        while (true)
        {
            Console.Clear();
            for (int i = 0; i < imageFiles.Length; i++)
            {
                string name = imageFiles[i].Substring(85);


                if (i == select)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(name);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.WriteLine(name);
                }
            }
            ConsoleKeyInfo key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.DownArrow)
            {
                select = (select + 1) % imageFiles.Length;
            }
            else if (key.Key == ConsoleKey.UpArrow)
            {
                select = (select - 1 + imageFiles.Length) % imageFiles.Length;
            }
            else if (key.Key == ConsoleKey.Escape)
            {
                break;
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                using (Process process = new Process())
                {
                    process.StartInfo.FileName = imageFiles[select];
                    process.StartInfo.UseShellExecute = true;
                    process.Start();
                }
            }
        }
    }

}

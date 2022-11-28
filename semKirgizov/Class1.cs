using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace semKirgizov
{
    public class Cursor
    {
        public int max;
        public int min;
        public int pos;
        public int lastPos;
        public Cursor(int max, int min, int pos)
        {
            this.max = max;
            this.min = min;
            this.pos = pos;
        }
    }
    public class Element
    {
        public string path;
        public bool isFile;
        public string name;
        public Element(string path, bool isFile, string name = null)
        {
            this.path = path;
            this.isFile = isFile;
            this.name = name ?? path.Split('\\')[^1];
        }
    }
    public class Menu
    {
        public static Element Select(List<Element> directory)
        {
            Cursor cursor = new Cursor(1, 1, 1);
            cursor.max = Menu.Show(directory);
            while (true)
            {
                cursor.lastPos = cursor.pos;
                Console.SetCursorPosition(0, cursor.pos);
                Console.Write("->");
                ConsoleKey key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        if (cursor.pos == cursor.min)
                            cursor.pos = cursor.max;
                        else
                            cursor.pos--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (cursor.pos == cursor.max)
                            cursor.pos = cursor.min;
                        else
                            cursor.pos++;
                        break;
                }
                Console.SetCursorPosition(0,cursor.lastPos);
                Console.Write("  ");
                if (key == ConsoleKey.Enter)
                    break;
            }
            return directory[cursor.pos - cursor.min];
        }
        private static int Show(List<Element> files)
        {
            int result = 0;
            foreach (Element element in files)
            {
                result++;
                Console.WriteLine("   " + element.name + (element.isFile ? "" : "\\"));
            }
            return result;
        }
    }
    public static class LoadFolders
    {
        public static List<Element> DrivesList()
        {
            List<Element> drivesList = new List<Element>();
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in allDrives)
            {
                decimal totalSpace = drive.TotalSize / 1024 / 1024 / 1024;
                totalSpace = Math.Round(totalSpace, 1);
                decimal availableSpace = drive.AvailableFreeSpace / 1024 / 1024 / 1024;
                availableSpace = Math.Round(availableSpace, 1);
                Element elem = new Element(drive.Name, false, name: $"Диск {drive.Name} | Размер {totalSpace} гб | Доступно {availableSpace} гб");
                drivesList.Add(elem);
            }
            return drivesList;
        }
        public static List<Element> FilesList(string folder, string parent)
        {
            List<Element> filesList = new List<Element>();
            filesList.Add(new Element(parent, false, ".."));
            foreach (string dir in Directory.GetDirectories(folder))
                filesList.Add(new Element(dir, false));
            foreach (string file in Directory.GetFiles(folder))
                filesList.Add(new Element(file, true));
            return filesList;
        }
    }
}

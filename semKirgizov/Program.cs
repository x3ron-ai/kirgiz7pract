using System.Diagnostics;

namespace semKirgizov
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string cur_path = "";
            string parent = "";
            List<Element> directory;
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Крутой проводник. path: {cur_path}");
                if (cur_path == "")
                    directory = LoadFolders.DrivesList();
                else
                    directory = LoadFolders.FilesList(cur_path, parent);
                Element selected = Menu.Select(directory);
                if (selected.isFile)
                {
                    Process.Start(selected.path);
                    break;
                }
                else
                {
                    parent = cur_path;
                    cur_path = selected.path;
                }
            }
        }
    }
}
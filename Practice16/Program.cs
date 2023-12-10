using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice16
{
    class Program
    {
        static void Main()
        {
            try
            {
                Console.WriteLine("Добро пожаловать в приложение для логирования изменений в файлах!");

                Console.Write("Введите путь к отслеживаемой директории: ");
                string directoryPath = Console.ReadLine();

                if (!Directory.Exists(directoryPath))
                {
                    Console.WriteLine("Директория не существует.");
                    return;
                }

                Console.Write("Введите путь к лог-файлу: ");
                string logFilePath = Console.ReadLine();

                using (FileSystemWatcher watcher = new FileSystemWatcher(directoryPath))
                {

                    watcher.IncludeSubdirectories = true;
                    watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;

                    watcher.Created += OnChanged;
                    watcher.Deleted += OnChanged;
                    watcher.Renamed += OnRenamed;

                    watcher.EnableRaisingEvents = true;

                    Console.WriteLine($"Отслеживание запущено для директории: {directoryPath}");

                    Console.WriteLine("Для завершения работы приложения нажмите любую клавишу...");
                    Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
        }

        static void OnChanged(object sender, FileSystemEventArgs e)
        {
            LogChange("Изменение", e.FullPath);
        }

        static void OnRenamed(object sender, RenamedEventArgs e)
        {
            LogChange("Переименование", $"{e.OldFullPath} -> {e.FullPath}");
        }

        static void LogChange(string action, string path)
        {
            string logMessage = $"{DateTime.Now} - {action}: {path}";

            File.AppendAllLines("log.txt", new[] { logMessage });

            Console.WriteLine(logMessage);
        }
    }
}

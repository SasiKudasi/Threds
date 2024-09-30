using System.Diagnostics;

namespace Otus.Threds
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Задание 1");
            await ReadThreeFilesParalel();

            Console.WriteLine("Задание 2");

            await ReadAllFilesInDirectory(Directory.GetCurrentDirectory());

            Console.ReadLine();
        }

        private static async Task ReadThreeFilesParalel()
        {
            var sv = new Stopwatch();
            sv.Start();
            var file1 = Path.Combine(Directory.GetCurrentDirectory(), "text1.txt");
            var file2 = Path.Combine(Directory.GetCurrentDirectory(), "text2.txt");
            var file3 = Path.Combine(Directory.GetCurrentDirectory(), "text3.txt");


            var task1 = File.ReadAllTextAsync(file1);
            var task2 = File.ReadAllTextAsync(file2);
            var task3 = File.ReadAllTextAsync(file3);

            var filetext = await Task.WhenAll(task1, task2, task3);

            var countSpacesTask1 = Task.Run(() => filetext[0].Count(c => c == ' '));
            var countSpacesTask2 = Task.Run(() => filetext[1].Count(c => c == ' '));
            var countSpacesTask3 = Task.Run(() => filetext[2].Count(c => c == ' '));

            var spaceCount = await Task.WhenAll(countSpacesTask2, countSpacesTask2, countSpacesTask3);
            sv.Stop();

            Console.WriteLine($"Колличество пробелов: {spaceCount.Sum()} время выполнения: {sv.ElapsedMilliseconds} мс");
        }

        static async Task ReadAllFilesInDirectory(string path)
        {
            var sv = new Stopwatch();
            var files = Directory.GetFiles(path);
            var fileRead = files.Select(file => Task.Run(async () =>
            {
                var text = await File.ReadAllTextAsync(file);
                return text.Count(c => c == ' ');
            }));
            var spaceCount = await Task.WhenAll(fileRead);
            sv.Stop();

            Console.WriteLine($"Колличество пробелов {spaceCount.Sum()}, время выполнения {sv.ElapsedMilliseconds} мс");
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Deadline
{
    class Program
    {
        private const string DATA_FOLDER = @"\deadline_data";
        private static string DATA_DIR_PATH;
        private static string PATH_1 = DATA_DIR_PATH + "/names.txt";
        private static string PATH_2 = DATA_DIR_PATH + "/deadlines.txt";

        private static bool isProgramRunning = true;
        public static List<Assignment> assignments = new List<Assignment>();


        private static void Main(string[] args)
        {
            DATA_DIR_PATH = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + DATA_FOLDER;
            if (Directory.Exists(DATA_DIR_PATH) == false) Directory.CreateDirectory(DATA_DIR_PATH);
            PATH_1 = DATA_DIR_PATH + "/names.txt";
            PATH_2 = DATA_DIR_PATH + "/deadlines.txt";

            LoadData();
            ShowAssignments();

            while (isProgramRunning)
            {
                Console.WriteLine();
                Console.WriteLine("Press [ Space ] to add new assignment.");
                Console.WriteLine("Press [ R ] to remove assigment.");
                Console.WriteLine("Press [ Tab ] to show assignments");
                Console.WriteLine("Press [ L ] to remove all assignments");
                Console.WriteLine("Press [ Escape ] to exit program.");
                Console.WriteLine();


                ConsoleKeyInfo info = Console.ReadKey();
                switch (info.Key)
                {
                    case ConsoleKey.Spacebar: AddAssignmentUI(); break;
                    case ConsoleKey.R: RemoveAssignmentUI(); break;
                    case ConsoleKey.Tab: ShowAssignments(); break;
                    case ConsoleKey.L: DeleteData(); break;
                    case ConsoleKey.Escape: Quit(); break;
                    case ConsoleKey.D0: break;
                }
            }

            SaveData();
        }

        private static void AddAssignmentUI()
        {
            Console.Write("\nName: ");
            string name = Console.ReadLine();

            Console.Write("Deadline: ");
            string deadline = Console.ReadLine();

            assignments.Add(new Assignment(name, deadline));
        }

        private static void RemoveAssignmentUI()
        {
            if (assignments == null || assignments.Count <= 0)
            {
                Console.WriteLine("No assignments have been added.");
                return;
            }

            ShowAssignments();
            Console.Write("Enter the index to remove: ");
            if (int.TryParse(Console.ReadLine(), out int i)) {
                if (i >= 0 && i < assignments.Count)
                    assignments.RemoveAt(i);
            }
        }

        private static void ShowAssignments()
        {
            Console.WriteLine();
            Console.WriteLine("--- Assignments ---");
            Console.WriteLine();

            for (int i = 0; i < assignments.Count; i++)
                Console.WriteLine($"[{i}] {assignments[i]}");
        }

        private static void Quit()
        {
            isProgramRunning = false;
        }

        private static void SaveData()
        {
            File.WriteAllText(PATH_1, JsonSerializer.Serialize(GetAssignmentData(NameFetcher)));
            File.WriteAllText(PATH_2, JsonSerializer.Serialize(GetAssignmentData(DeadlineFetcher)));
        }

        public delegate string DataFetcher(Assignment asg);

        public static string NameFetcher(Assignment asg) => asg.name;
        public static string DeadlineFetcher(Assignment asg) => asg.deadline;

        public static string[] GetAssignmentData(DataFetcher fetch)
        {
            string[] data = new string[assignments.Count];
            for (int i = 0; i < data.Length; i++)
                data[i] = fetch(assignments[i]);
            return data;
        }


        public static List<Assignment> ToAssignmentList(string[] names, string[] deadlines) 
        {
            List<Assignment> asgList = new List<Assignment>();
            for (int i = 0; i < names.Length; i++)
                asgList.Add(new Assignment(names[i], deadlines[i]));
            return asgList;
        }

        private static void LoadData()
        {
            try
            {
                string data1 = File.ReadAllText(PATH_1);
                string data2 = File.ReadAllText(PATH_2);
                assignments = ToAssignmentList(JsonSerializer.Deserialize<string[]>(data1), JsonSerializer.Deserialize<string[]>(data2));
            }
            catch { DeleteData(); }
        }

        private static void DeleteData() 
        {
            Console.WriteLine();
            Console.WriteLine("All saved data has been deleted.");
            File.Delete(PATH_1);
            File.Delete(PATH_2);
        }
    }
}

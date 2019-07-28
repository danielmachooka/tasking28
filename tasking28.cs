using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracking
{

    class App

    {
        private List<string> tasks = new List<string>();
        private List<bool> isActioned = new List<bool>();
        private int selectedTask = 0;

        const int pageLength = 25;

        public object ConsolekeyInfo1 => ConsolekeyInfo2;

        public object ConsolekeyInfo2 { get; }

        public App()
        {
            Console.OutputEncoding = Encoding.Unicode;


            ReadListFromFile();
        }

        public void Run()
        {
            bool quit;


            do
            {
               // RemoveFirstActionedItems();
                PrintTaskList();
                var key = RunInputcycle();
                quit = HandleUserInput(key);
            }
            while (!quit);


            writeListToFile();

            Console.WriteLine();// Ensures press any line to quit is in own line

        }

     /*   private void RemoveFirstActionedItems()
        {

            while (isActioned[0])
            {
                tasks.RemoveAt(0);
                isActioned.RemoveAt(0);
                selectedTask -= 1;
            }
            if (selectedTask < 0)
            {
                selectedTask = 0;
            }

        } */

        private ConsoleKey RunInputcycle()
        {
            ConsoleKey key;

            PrintUsageOptions();
            key = GetInputFromUser();


            return key;

        }

        private bool HandleUserInput(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.A:
                    InputTaskToList();
                    break;
                case ConsoleKey.D:
                    DeleteCurrentlySelectedTask();
                    break;
                case ConsoleKey.N:
                    SelectNextPage();
                    break;
                case ConsoleKey.DownArrow:
                    SelectNextUnactionedTask();
                    break;
                case ConsoleKey.Enter:
                    WorkOnSelectedTask();
                    break;
                case ConsoleKey.Q:
                    return true;

            }
            return false;
        }

        private void SelectNextPage()
        {
            var page = GetPage();
            selectedTask = FirstElementInPage(page + 1) -1 ;

            
         //   SelectNextUnactionedTask();
        }

        private void WorkOnSelectedTask()
        {
            bool valid = false;

            do
            {
                Console.Clear();
                Console.WriteLine($"Working on: {tasks[selectedTask]}");
                Console.WriteLine(" r:re-enter | c: complete | Q:cancel");
                Console.Write("input: ");

                var key = GetInputFromUser();

                switch (key)
                {
                    case ConsoleKey.R:
                        ReenterTask();
                        valid = true;
                        break;
                    case ConsoleKey.C:
                        DeleteCurrentlySelectedTask();
                        valid = true;
                        break;
                    case ConsoleKey.Q:
                        valid = true;
                        break;


                }
            } while (!valid);
        }

        private void ReenterTask()
        {
            AddTaskToList(tasks[selectedTask]);
            DeleteCurrentlySelectedTask();
        }

        private void DeleteCurrentlySelectedTask()
        {
            isActioned[selectedTask] = true;
            SelectNextUnactionedTask();
        }

        private void SelectNextUnactionedTask()
        {
            bool overflowed = false;

            do
            {
                selectedTask += 1;

                if (selectedTask >= isActioned.Count)
                {
                    selectedTask = 0;
                    overflowed = true;
                }

            } while (!overflowed && isActioned[selectedTask]);


        }

        private ConsoleKey GetInputFromUser()
        {
            return Console.ReadKey().Key;
        }

        private void PrintUsageOptions()
        {
            Console.WriteLine(" a:add | n: next page | enter: action| D: delete | \u2193: select|q:quit");
            Console.Write("input: ");
        }
        private void InputTaskToList()
        {
            Console.Clear();
            Console.Write("Add a new task(empty to cancel:");

            var input = Console.ReadLine();

            AddTaskToList(input);
        }

        private void AddTaskToList(string input)
        {
            if (!string.IsNullOrWhiteSpace(input))
            {
                tasks.Add(input);
                isActioned.Add(false);
            }
        }

        private void PrintTaskList()
        {
            Console.Clear();

            var page = GetPage() / pageLength;
            var startingPoint = FirstElementInPage(page);

            int endingPoint = FirstElementInPage(page + 1);
            for (int i = startingPoint; (i < endingPoint) && (i < tasks.Count); ++i)
            {
                if (isActioned[i])
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                }
                else if (i == selectedTask)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                }

                Console.WriteLine(tasks[i]);

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Black;

            }
            Console.WriteLine();
        }

        private static int FirstElementInPage(int page)
        {
            return page * pageLength;
        }

        private int GetPage()
        {
            return selectedTask / pageLength;
        }

        private void ReadListFromFile()
        {

            try
            {
                using (StreamReader sr = new StreamReader(@"C:\Users\danch\OneDrive\Desktop\Tasklist.txt.text2.txt"))
                {
                    while (!sr.EndOfStream)
                    {
                        var input = sr.ReadLine();

                        var splits = input.Split(new char[] { '\x1e' });

                        if (splits.Length == 2)
                        {
                            tasks.Add(splits[0]);
                            isActioned.Add(bool.Parse(splits[1]));
                        }

                        tasks.Add(input);
                        isActioned.Add(false);

                    }

                }
            }
            catch (FileNotFoundException)
            {; }

        }

        private void writeListToFile()
        {
            using (StreamWriter sw = new StreamWriter(@"C:\Users\danch\OneDrive\Desktop\Tasklist.txt.text2.txt "))
            {
                for (int i = 0; i < tasks.Count; ++i)
                {
                    sw.WriteLine($"{tasks[i]}\x1e{isActioned[i]}");
                }

            }

        }
    }
}




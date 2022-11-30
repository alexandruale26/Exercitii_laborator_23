
namespace Exercitii_laborator_23
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string path = @"C:\Users\Alexandru\OneDrive\Desktop\list.txt";

            Ex1(path);
            //Ex2(path);
            //Ex3(path);
        }

        static void Ex1(string path)
        {
            #region Cerinta exercitiu

            /* 
                  Citirea unei liste de liste de numere din fisier.
              
                    • Intr-un fisier text va fi salvata o lista de liste de numere 
                  astfel: pe fiecare linie va fi salvata cate o lista de numere intregi
                    • Scrieti un program care sa citeasca din fisier o astfel de lista a listelor
                    • Scrieti o functie care sa scrie in fisier o astfel de lista a listelor

                  • Observatii:
                    • Fisierul va fi tratat ca unul text astfel incat de pe fiecare linie sa 
                  fie citita cate o lista de numere
                    • Nu serializati deserializati intreaga lista dintr-o singura instructiune
                    • Scopul acestui exercitiu este acela de a exersa operatiile asupra 
                  fisierelor text
            */

            #endregion

            Console.WriteLine("Introduceti numerele");
            AddNumbers(path);

            Console.WriteLine($"Results:");
            PrintNumbers(path);
        }

        static void Ex2(string path)
        {
            #region Cerinta exercitiu

            /*
                  Tasks
                • Scrieti o functie care va calcula in mod concurent suma tuturor numerelor de la ex 1.
                • Pentru fiecare lista de numere din lista de numere se va lansa un task individual!
                • La finalul executiei, afisati fiecare suma a numerelor.
                • Pentru a asigura corectitudinea si lipsa race-condition-urilor, rulati
                functia de mai multe ori si observati rezultatul (10-100 ori).
             */

            #endregion

            int sum = 0;

            char[] charsToSplit = {' ', ',', '.'};
            string[] lines = ReadLines(path);

            foreach (string line in lines)
            {
                Task task = Task.Factory.StartNew(() =>
                {
                    int[] nums = line.Split(charsToSplit).Select(n => Convert.ToInt32(n)).ToArray();
                    foreach (int num in nums)
                    {
                        sum += num;
                    }
                });
                task.Wait();
            }

            Console.WriteLine(sum);
        }

        static void Ex3(string path)
        {
            #region Cerinta exercitiu

            /*
                  Tasks
                • Scrieti o functie care va calcula in mod concurent suma fiecarei liste
                  individuale de numere din lista citita la exercitiul 1.
                • Pentru fiecare lista de numere din lista de numere se va lansa un task individual!
                • La finalul executiei, afisati fiecare suma in parte.
                • Calculati ulterior, in mod concurent, suma sumelor si afisati-o!
             */

            #endregion

            char[] charsToSplit = { ' ', ',', '.' };
            string[] lines = ReadLines(path);
            var tasks = new List<Task<int>>();

            foreach (string line in lines)
            {

                Task<int> task = Task.Factory.StartNew(() =>
                {
                    int[] nums = line.Split(charsToSplit).Select(n => Convert.ToInt32(n)).ToArray();
                    int lineSum = 0;

                    foreach (int num in nums)
                    {
                        lineSum += num;
                    }
                    Console.WriteLine(lineSum);
                    return lineSum;
                });
                tasks.Add(task);
            }

            Task.WaitAll(tasks.ToArray());
            Console.WriteLine(tasks.Sum(t => t.Result));
        }


        #region Methods

        static string[] ReadLines(string path)
        {
            return File.ReadAllLines(path);
        }

        static void AddNumbers(string path)
        {
            string numbers = Console.ReadLine() + "\n"; // numere pot avea intre ele spatiu, punct, virgula (ex: 2,5,7,8,3,0)

            CheckOrCreateFile(path);
            File.AppendAllText(path, numbers);
        }

        static void PrintNumbers(string path)
        {
            string[] lines = ReadLines(path);

            for (int i = 0; i < lines.Length; i++)
            {
                Console.WriteLine($"Line {i + 1}.   {lines[i]}");
            }
        }

        static void CheckOrCreateFile(string path)
        {
            if (!File.Exists(path))
            {
                using (File.Create(path)) { }
            }
        }

        #endregion
    }
}
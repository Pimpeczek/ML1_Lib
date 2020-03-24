using System.IO;
using System;

namespace ML1_Lib
{
    public class Task
    {

        /// <summary>
        /// Array of items, where rows represent items and columns represent size, weight and price in that order.
        /// </summary>
        public int[,] Items { get; protected set; }

        /// <summary>
        /// Total number of items.
        /// </summary>
        public int ItemCount
        {
            get
            {
                if (Items == null)
                    return 0;
                return Items.GetLength(0);
            }
        }

        /// <summary>
        /// The maximal backpack size.
        /// </summary>
        public int MaxSize { get; protected set; }

        /// <summary>
        /// The maximal backpack weight.
        /// </summary>
        public int MaxWeight { get; protected set; }

        /// <summary>
        /// Loads a task from a specified file.
        /// </summary>
        /// <param name="fileName">Path to the task file.</param>
        public Task(string fileName)
        {
            Load(fileName);
        }

        /// <summary>
        /// Creates a 
        /// </summary>
        /// <param name="itemCount"></param>
        /// <param name="maxSize"></param>
        /// <param name="maxWeight"></param>
        public Task(int itemCount, int maxSize, int maxWeight)
        {
            PopulateItems(itemCount, maxSize, maxWeight);
        }

        /// <summary>
        /// Populates the array of items.
        /// </summary>
        /// <param name="itemCount"></param>
        /// <param name="maxBackpackSize"></param>
        /// <param name="maxBackpackWeight"></param>
        protected void PopulateItems(int itemCount, int maxBackpackSize, int maxBackpackWeight)
        {
            MaxSize = maxBackpackSize;
            MaxWeight = maxBackpackWeight;
            Items = new int[itemCount, 3];
            int maxItemSize = MaxSize * 10 / itemCount;
            int maxItemWeight = MaxWeight * 10 / itemCount;
            int maxItemPrice = itemCount;
            int totalSize;
            int totalWeight;
            totalSize = 0;
            totalWeight = 0;
            for (int i = itemCount - 1; i >= 0; i--)
            {
                totalSize += (Items[i, 0] = Misc.rng.Next(1, maxItemSize));
                totalWeight += (Items[i, 1] = Misc.rng.Next(1, maxItemWeight));
                totalWeight += (Items[i, 2] = Misc.rng.Next(1, maxItemPrice));
            }
            if (totalSize <= maxItemSize)
            {
                double ratio = (double)maxItemSize / totalSize;
                for (int i = itemCount - 1; i >= 0; i--)
                {
                    Items[i, 0] = (int)Math.Ceiling(Items[i, 0] * ratio);
                }
            }
            if (totalWeight <= maxItemWeight)
            {
                double ratio = (double)maxItemWeight / totalWeight;
                for (int i = itemCount - 1; i >= 0; i--)
                {
                    Items[i, 1] = (int)Math.Ceiling(Items[i, 1] * ratio);
                }
            }
        }

        
        /// <summary>
        /// Saves this instance of Task to a specified file.
        /// </summary>
        /// <param name="filename">File path to save the Task.</param>
        public void Save(string filename)
        {
            int count = ItemCount;
            using (StreamWriter sw = new StreamWriter($"{filename}"))
            {
                sw.WriteLine($"{count}{Misc.ESC}{MaxSize}{Misc.ESC}{MaxWeight}");

                for (int i = count - 1; i >= 0; i--)
                {
                    sw.WriteLine($"{Items[i, 0]}{Misc.ESC}{Items[i, 1]}{Misc.ESC}{Items[i, 2]}");
                }
                sw.Close();
            }
        }

        /// <summary>
        /// Loads a task from a specified file.
        /// </summary>
        /// <param name="filename">Path to the task file.</param>
        public void Load(string filename)
        {
            string[] splitted;
            int tempInt;
            using (StreamReader sr = new StreamReader($"{filename}"))
            {
                splitted = sr.ReadLine().Split(Misc.ESC);
                if (splitted.Length != 3)
                    throw new InvalidDataException($"Wrong format of a file - too short");
                if (!int.TryParse(splitted[0], out tempInt))
                    throw new InvalidDataException($"ItemCount: {splitted[0]}");
                Items = new int[tempInt, 3];

                if (!int.TryParse(splitted[1], out tempInt))
                    throw new InvalidDataException($"MaxSize: {splitted[1]}");
                MaxSize = tempInt;

                if (!int.TryParse(splitted[2], out tempInt))
                    throw new InvalidDataException($"MaxWeight: {splitted[2]}");
                MaxWeight = tempInt;

                tempInt = Items.GetLength(0);

                for (int i = 0; i < tempInt && !sr.EndOfStream; i++)
                {
                    splitted = sr.ReadLine().Split(Misc.ESC);
                    if (splitted.Length != 3)
                        throw new InvalidDataException($"Wrong format of a file - line {i + 1}");

                    if (!int.TryParse(splitted[0], out Items[i, 0]))
                        throw new InvalidDataException($"Line {i + 1} MaxSize: {splitted[0]}");

                    if (!int.TryParse(splitted[1], out Items[i, 1]))
                        throw new InvalidDataException($"Line {i + 1} MaxWeight: {splitted[1]}");

                    if (!int.TryParse(splitted[2], out Items[i, 2]))
                        throw new InvalidDataException($"Line {i + 1} MaxWeight: {splitted[2]}");
                }
            }
        }

        /// <summary>
        /// Evaluates a given Individual.
        /// </summary>
        /// <param name="DNA"></param>
        /// <returns></returns>
        public int Evaluate(bool[] DNA)
        {
            int totalSize = 0;
            int totalWeight = 0;
            int totalPrice = 0;
            for (int i = ItemCount - 1; i >= 0; i--)
            {
                if (DNA[i])
                {
                    totalSize += Items[i, 0];
                    totalWeight += Items[i, 1];
                    totalPrice += Items[i, 2];
                }
            }

            if (totalSize > MaxSize || totalWeight > MaxWeight)
            {
                return MaxSize - totalSize + MaxWeight - totalWeight;
            }
            return totalPrice;
        }

    }
}

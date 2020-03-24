using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML1_Lib
{
    /// <summary>
    /// A class representing an individual. 
    /// Generally just a wrapper for its DNA and few methods.
    /// </summary>
    public class Individual
    {

        /// <summary>
        /// The DNA of this individual.
        /// </summary>
        public bool[] DNA { get; protected set; }

        /// <summary>
        /// The lenght of individual's DNA.
        /// </summary>
        public int DNALength { get; protected set; }

        /// <summary>
        /// The fitness of this individual.
        /// </summary>
        public int Fitness { get; protected set; } = int.MinValue;


        /// <summary>
        /// Creates a new Individual.
        /// </summary>
        /// <param name="dnaLength">The length of this individual's DNA.</param>
        /// <param name="chance">The chance that an allele is set to true on initialization.</param>
        /// <param name="seed">The seed of this individual.</param>
        public Individual(int dnaLength, double chance, int seed)
        {
            
            Random rng = new Random(seed);
            DNALength = dnaLength;
            DNA = new bool[DNALength];
            for (int i = DNALength - 1; i >= 0; i--)
            {
                DNA[i] = rng.NextDouble() < chance;
            }
        }

        /// <summary>
        /// Creates a new Individual using crossover method.
        /// </summary>
        /// <param name="parent1"></param>
        /// <param name="parent2"></param>
        /// <param name="doubleCrossover">Determines if there should be a second cut in the DNA.</param>
        public Individual(Individual parent1, Individual parent2, bool doubleCrossover)
        {
            DNALength = parent1.DNALength;
            DNA = new bool[DNALength];
            int crossoverPoint1 = Misc.rng.Next(DNALength);
            int crossoverPoint2;
            if(doubleCrossover)
            {
                crossoverPoint2 = Misc.rng.Next(DNALength);
                if (crossoverPoint1 > crossoverPoint2)
                    Misc.Swap(ref crossoverPoint1, ref crossoverPoint2);
            }
            else
            {
                crossoverPoint2 = DNALength;
            }
            for(int i = crossoverPoint1-1; i >=0; i--)
            {
                DNA[i] = parent1.DNA[i];
            }

            for (int i = crossoverPoint1; i < crossoverPoint2; i++)
            {
                DNA[i] = parent2.DNA[i];
            }

            if (doubleCrossover)
            {
                for (int i = crossoverPoint2; i < DNALength; i++)
                {
                    DNA[i] = parent1.DNA[i];
                }
            }
        }

        /// <summary>
        /// Flips a given number of alleles.
        /// </summary>
        /// <param name="mutationCount">Number of alleles to flip.</param>
        public void Mutate(int mutationCount)
        {
            int position;
            for (int i = 0; i < mutationCount; i++)
            {
                position = Misc.rng.Next(DNALength);
                DNA[position] = !DNA[position];
            }
        }

        /// <summary>
        /// Reevaluates this individual on a given Task and returns its score.
        /// </summary>
        /// <param name="task">Task to evaluate on.</param>
        /// <returns></returns>
        public int Evaluate(Task task)
        {
            return Fitness = task.Evaluate(DNA);
        }

        public static bool operator >=(Individual i1, Individual i2)
        {
            return i1.Fitness >= i2.Fitness;
        }
        public static bool operator <=(Individual i1, Individual i2)
        {
            return i1.Fitness <= i2.Fitness;
        }
        public static bool operator >(Individual i1, Individual i2)
        {
            return i1.Fitness > i2.Fitness;
        }
        public static bool operator <(Individual i1, Individual i2)
        {
            return i1.Fitness < i2.Fitness;
        }

    }
}

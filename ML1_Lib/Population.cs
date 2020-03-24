using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML1_Lib
{
    /// <summary>
    /// A class representing population - a collection of individuals.
    /// Responsible for storing individuals and initiating genetic algorithm.
    /// </summary>
    public class Population
    {
        /// <summary>
        /// Array of Idividual instances.
        /// </summary>
        public Individual[] Individuals { get; protected set; }

        /// <summary>
        /// Helper field for storing item count.
        /// </summary>
        int itemCount;

        /// <summary>
        /// Determines if a double crossovers will occure.
        /// </summary>
        public bool DoubleCrossover { get; set; } = false;

        /// <summary>
        /// Determines the chance of a crossover happening.
        /// </summary>
        public double CrossoverRate { get; set; }

        double mutationRate;

        /// <summary>
        /// Determines how much of the Individual DNA will mutate every generation.
        /// </summary>
        public double MutationRate
        {
            get
            {
                return mutationRate;
            }
            set
            {
                mutationRate = value;
                mutationCount = (int)Math.Ceiling(itemCount * mutationRate);
                Console.WriteLine(mutationCount);
            }
        }

        /// <summary>
        /// Helper field for storing number of allele to mutate every generation.
        /// </summary>
        int mutationCount;


        /// <summary>
        /// Determines the size of the tournament.
        /// </summary>
        int tournamentSize;

        /// <summary>
        /// Determines the size of the tournament.
        /// </summary>
        public int TournamentSize
        {
            get
            {
                return tournamentSize;
            }
            set
            {
                if (value > Individuals.Length)
                    tournamentSize = Individuals.Length;
                else
                    tournamentSize = value;
            }
        }

        /// <summary>
        /// The task for this population.
        /// </summary>
        protected Task task;
        /// <summary>
        /// The task for this population.
        /// </summary>
        public Task Task
        {
            get
            {
                return task;
            }
            set
            {
                task = value ?? throw new NullReferenceException();
                itemCount = task.ItemCount;
            }
        }

        /// <summary>
        /// Best current score.
        /// </summary>
        public int BestScore { get; protected set; }
        /// <summary>
        /// ID of the best current individual.
        /// </summary>
        public int BestIdividualID { get; protected set; }

        /// <summary>
        /// The best current individual.
        /// </summary>
        public Individual BestIdividual
        {
            get
            {
                return Individuals[BestIdividualID];
            }
        }

        /// <summary>
        /// Random number generator for this instance of Population.
        /// </summary>
        protected Random rng;

        /// <summary>
        /// Creates an instance of Population class.
        /// </summary>
        /// <param name="task">The task for this population.</param>
        /// <param name="populationSize">The number of Individuals</param>
        /// <param name="tournamentSize">The number of Individuals taking part in each tournament.</param>
        /// <param name="crossoverRate">Determines the chance of a crossover happening.</param>
        /// <param name="mutationRate">Determines how much of the Individual DNA will mutate every generation.</param>
        /// <param name="dnaTrueChance"></param>
        /// <param name="seed"></param>
        public Population(Task task, int populationSize, int tournamentSize, double crossoverRate, double mutationRate, double dnaTrueChance, int seed)
        {
            Task = task;
            
            CrossoverRate = crossoverRate;
            MutationRate = mutationRate;

            rng = new Random(seed);
            Individuals = new Individual[populationSize];
            for (int i = populationSize - 1; i >= 0; i--)
            {
                Individuals[i] = new Individual(task.ItemCount, dnaTrueChance, rng.Next());
                Individuals[i].Evaluate(task);
            }

            TournamentSize = tournamentSize;
        }

        /// <summary>
        /// The tournament. Returns a touple containing two parents' IDs
        /// </summary>
        /// <returns></returns>
        public (int, int) Tournament()
        {
            int parent1 = 0;
            int parent2 = 0;
            int tempParent;

            for (int t = TournamentSize; t > 0; t--)
            {
                if (Individuals[tempParent = rng.Next(Individuals.Length)] >= Individuals[parent1])
                {
                    parent1 = tempParent;
                }

                if (Individuals[tempParent = rng.Next(Individuals.Length)] >= Individuals[parent2])
                {
                    parent2 = tempParent;
                }
            }
            return (parent1, parent2);
        }

        /// <summary>
        /// Creates a new population using Tournament method. Returns the best score.
        /// </summary>
        public int CreateNewGeneration()
        {
            Individual[] newPopulation = new Individual[Individuals.Length];
            (int, int) parents;
            int bestIndividualID = Individuals.Length - 1;

            for (int i = Individuals.Length - 1; i >= 0; i--)
            {
                parents = Tournament();

                if (CrossoverRate > rng.NextDouble())
                {
                    newPopulation[i] = new Individual(Individuals[parents.Item1], Individuals[parents.Item2], DoubleCrossover);
                }
                else
                {
                    newPopulation[i] = Individuals[parents.Item1];
                }
                newPopulation[i].Mutate(mutationCount);

                newPopulation[i].Evaluate(task);//Evaluation takes place here for the next generation to reduce loop iterations.
                if (newPopulation[i] > newPopulation[bestIndividualID])
                    bestIndividualID = i;
            }
            BestIdividualID = bestIndividualID;
            BestScore = newPopulation[bestIndividualID].Fitness;
            Individuals = newPopulation;
            return BestScore;
        }
    }
}

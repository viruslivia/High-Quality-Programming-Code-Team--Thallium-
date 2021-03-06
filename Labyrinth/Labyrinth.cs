﻿namespace LabirynthGame
{
    using System;

    public class Labyrinth
    {
        private const int MinimumPercentageOfBlockedCells = 30;
        private const int MaximumPercentageOfBlockedCells = 50;

        private const char BlockedCell = 'X';
        private const char FreeCell = '-';
        private const char PlayerSign = '*';

        private readonly int startPositionX;
        private readonly int startPositionY;

        private int sizeOfTheLabyrinth;
        private char[,] matrix;

        public Labyrinth(int sizeOfTheLabyrinth)
        {
            if (sizeOfTheLabyrinth < 1)
            {
                throw new ArgumentException("The size of the labyrinth cannot be less than 1", "sizeOfTheLabirynth");
            }

            this.sizeOfTheLabyrinth = sizeOfTheLabyrinth;
            this.startPositionX = sizeOfTheLabyrinth / 2;
            this.startPositionY = sizeOfTheLabyrinth / 2;
            this.matrix = new char[sizeOfTheLabyrinth, sizeOfTheLabyrinth];
            this.GenerateMatrix();
        }

        public int Size
        {
            get
            {
                return this.sizeOfTheLabyrinth;
            }
        }

        public char this[int row, int col]
        {
            get
            {
                if (!this.IsInRange(row))
                {
                    throw new ArgumentOutOfRangeException("row", "The labyrinth does not have such row!");
                }
                else if (!this.IsInRange(col))
                {
                    throw new ArgumentOutOfRangeException("col", "The labyrinth does not have such col!");
                }
                else
                {
                    return this.matrix[row, col];
                }
            }

            set
            {
                if (!this.IsInRange(row))
                {
                    throw new ArgumentOutOfRangeException("row", "The labyrinth does not have such row!");
                }
                else if (!this.IsInRange(col))
                {
                    throw new ArgumentOutOfRangeException("col", "The labyrinth does not have such col!");
                }
                else
                {
                    this.matrix[row, col] = value;
                }
            }
        }

        public void PrintLabirynth()
        {
            for (int row = 0; row < this.sizeOfTheLabyrinth; row++)
            {
                for (int col = 0; col < this.sizeOfTheLabyrinth; col++)
                {
                    Console.Write("{0,2}", this.matrix[row, col]);
                }

                Console.WriteLine();
            }
        }

        private void GenerateMatrix()
        {
            this.GenerateMatrixBlockedCells();

            this.matrix[this.startPositionX, this.startPositionY] = PlayerSign;

            this.MakeAtLeastOneExitReachable(this.matrix);
        }

        private void GenerateMatrixBlockedCells()
        {
            Random rand = new Random();
            int percentageOfBlockedCells = rand.Next(MinimumPercentageOfBlockedCells, MaximumPercentageOfBlockedCells);

            for (int row = 0; row < this.sizeOfTheLabyrinth; row++)
            {
                for (int col = 0; col < this.sizeOfTheLabyrinth; col++)
                {
                    int num = rand.Next(0, 100);
                    if (num < percentageOfBlockedCells)
                    {
                        this.matrix[row, col] = BlockedCell;
                    }
                    else
                    {
                        this.matrix[row, col] = FreeCell;
                    }
                }
            }
        }

        private void MakeAtLeastOneExitReachable(char[,] generatedMatrix)
        {
            Random rand = new Random();
            int row = this.startPositionX;
            int col = this.startPositionY;
            int[] dirX = { 0, 0, 1, -1 };
            int[] dirY = { 1, -1, 0, 0 };
            int numberOfDirections = 4;
            int maximumTimesToChangeAfter = 2;

            while (this.HasSolution(row, col) == false)
            {
                int num = rand.Next(0, numberOfDirections);
                int times = rand.Next(0, maximumTimesToChangeAfter);

                for (int i = 0; i < times; i++)
                {
                    if (this.IsInTheLabirynth(row + dirX[num], col + dirY[num]))
                    {
                        row += dirX[num];
                        col += dirY[num];

                        if (this.matrix[row, col] == PlayerSign)
                        {
                            continue;
                        }

                        this.matrix[row, col] = FreeCell;
                    }
                }
            }
        }

        private bool IsInTheLabirynth(int row, int col)
        {
            bool isInTheLabirynth = this.IsInRange(row) &&
                this.IsInRange(col);

            return isInTheLabirynth;
        }

        private bool HasSolution(int row, int col)
        {
            if ((row > 0 && row < this.sizeOfTheLabyrinth - 1) &&
                (col > 0 && col < this.sizeOfTheLabyrinth - 1))
            {
                return false;
            }

            return true;
        }

        private bool IsInRange(int coordinate)
        {
            bool isInRange = coordinate >= 0 && coordinate < this.sizeOfTheLabyrinth;
            return isInRange;
        }
    }
}

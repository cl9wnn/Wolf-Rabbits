using Contract;

class Program
{
    static void Main() { }
}
class GameOfLife:IGameOfLife
{
    private bool _isAnyKilled;
    private bool _isAnyEaten;
    private object _locker = new object();

    public event Func<AnimalType[,], Task>? UpdateGridEvent;

    public AnimalType[,] InitializeField(int rows, int cols)
    {
        AnimalType[,] grid = new AnimalType[rows, cols];

        Random random = new Random();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                grid[i, j] = (AnimalType)random.Next(0, 3);
            }
        }
        return grid;
    }

    public async Task StartSimulation(AnimalType[,] grid)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        AnimalType[,] bufferGrid = new AnimalType[rows, cols];

        do
        {
            _isAnyEaten = false;
            _isAnyKilled = false;

            KillAnimals(bufferGrid, grid);

            EatRabbits(bufferGrid);

            Array.Copy(bufferGrid, grid, rows * cols);

            UpdateGridEvent?.Invoke(grid);

            await Task.Delay(100);

        } while (_isAnyEaten || _isAnyKilled);

    }
    private void KillAnimals(AnimalType[,] bufferGrid, AnimalType[,] grid)
    {
        int rows = bufferGrid.GetLength(0);
        int cols = bufferGrid.GetLength(1);

        Parallel.For(0, rows, i =>
        {
            Parallel.For(0, cols, j =>
            {
                int neigboursCount = CountNeighbours(grid, i, j);
                var newAnimal = UpdateFieldCell(grid[i, j], neigboursCount);
                bufferGrid[i, j] = newAnimal;
            });
        });
    }


    private void EatRabbits(AnimalType[,] bufferGrid)
    {
        int rows = bufferGrid.GetLength(0);
        int cols = bufferGrid.GetLength(1);

        Parallel.For(0, rows, i =>
        {
            Parallel.For(0, cols, j =>
             {
                 bool isBunnyEaten = false;

                 if (bufferGrid[i, j] == AnimalType.Wolf)
                 {
                     for (int m = i - 1; m <= i + 1 && !isBunnyEaten; m++)
                     {
                         for (int h = j - 1; h <= j + 1 && !isBunnyEaten; h++)
                         {
                             if (m >= 0 && m < rows && h >= 0 && h < cols && !(m == i && h == j))
                             {
                                 if (bufferGrid[m, h] == AnimalType.Rabbit)
                                 {
                                     lock(_locker)
                                     {
                                         bufferGrid[m, h] = AnimalType.Eaten;
                                         isBunnyEaten = true;
                                         _isAnyEaten = true;
                                     }
                                    
                                 }
                             }
                         }
                     }
                 }
             });
        });
    }

    private int CountNeighbours(AnimalType[,] field , int x, int y)
    {
        int rows = field.GetLength(0);
        int cols = field.GetLength(1);
        int count = 0;

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                    continue;

                int neighbourX = x + i;
                int neighbourY = y + j;

                if (neighbourX >= 0 && neighbourY >= 0 && neighbourX < rows && neighbourY < cols)
                {
                    if (field[neighbourX, neighbourY] == AnimalType.Wolf || field[neighbourX, neighbourY] == AnimalType.Rabbit)
                    {
                        count++;
                    }

                }
            }
        }
        return count;
    }

    private AnimalType UpdateFieldCell(AnimalType current, int neighbourCount)
    {

        if (current == AnimalType.Wolf)
        {
            switch (neighbourCount)
            {
                case 0:
                case 1:
                    _isAnyKilled = true;
                    return AnimalType.Kill;
                case 2:
                case 3:
                    return AnimalType.Wolf;
                default:
                    _isAnyKilled = true;
                    return AnimalType.Kill;
            }
        }
        else if (current == AnimalType.Rabbit)
        {
            switch (neighbourCount)
            {
                case 0:
                case 1:
                    _isAnyKilled = true;
                    return AnimalType.Kill;
                case 2:
                case 3:
                case 4:
                case 5:
                    return AnimalType.Rabbit;
                default:
                    _isAnyKilled = true;
                    return AnimalType.Kill;
            }
        }
        else if (current == AnimalType.None)
        {
            switch (neighbourCount)
            {
                case 2:
                case 3:
                    return (AnimalType)new Random().Next(0, 2);
                default:
                    return AnimalType.None;
            }
        }
        else return AnimalType.None;
    }
}
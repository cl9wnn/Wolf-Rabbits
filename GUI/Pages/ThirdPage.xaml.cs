using System;
using System.Reflection;
using Contract;
using Microsoft.Maui.Controls;
namespace Simulation
{
    public partial class ThirdPage : ContentPage
    {
        private int _rows, _cols;
        private Assembly _assembly;
        private AnimalType[,] _matrix;
        private IGameOfLife _gameInstance;


        static Dictionary<AnimalType, string> imageDictionary = new Dictionary<AnimalType, string>
        {
               { AnimalType.Rabbit,"bunny3.png"},
               { AnimalType.Wolf, "wolf3.png"   },
               { AnimalType.None, "empty2.png"  },
               { AnimalType.Kill,  "rip2.png"  },
               { AnimalType.Eaten,"bones2.png"  }
        };

        public ThirdPage(Assembly asm, int rows, int cols)
        {
            _assembly = asm;
            _rows = rows;
            _cols = cols;
            InitializeComponent();
            CreateInstance();
            InitializeGrid();
        }

        public void CreateInstance()
        {
            var gameType = _assembly.GetTypes().First(t => t.Name == "GameOfLife");

            if (Activator.CreateInstance(gameType) is IGameOfLife gameInstance)
            {
                _gameInstance = gameInstance;
            }
            else
                throw new InvalidOperationException("Произошла ошибка создания экземпляра");

            _gameInstance.UpdateGridEvent += UpdateGrid;

        }

        public void InitializeGrid()
        {
            _matrix = _gameInstance.InitializeField(_rows, _cols);

            for (int i = 0; i < _rows; i++)
                grid.RowDefinitions.Add(new RowDefinition());
            for (int i = 0; i < _cols; i++)
                grid.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    var animal = _matrix[i, j];
                    var image = new Image() { Source = imageDictionary[animal] };
                    var frame = new Frame
                    {
                        Content = image,
                        BackgroundColor = Color.FromRgba(91, 161, 34, 255),
                        Padding = 3
                    };

                    grid.Add(frame, i, j);
                }
            }

        }

        public async Task UpdateGrid(AnimalType[,] changedMatrix)
        {
            _matrix = changedMatrix;

            await Task.Run(() =>
            {
                Dispatcher.DispatchAsync(() =>
                {
                    foreach (var child in grid.Children)
                    {
                        int row = grid.GetRow(child);
                        int col = grid.GetColumn(child);

                        var image = (Image)((Frame)child).Content;
                        image.Source = null;
                        image.Source = imageDictionary[_matrix[row, col]];
                    }
                });
        });
        }

        private async Task ChangeColorAsync(Color frameColor, Color backColor)
        {
            await Task.Run(() =>
            {
                Dispatcher.DispatchAsync(() =>
                {
                    GridFrame.BackgroundColor = backColor;
                    foreach (var child in grid.Children)
                    {
                        if (child is Frame frame)
                        {
                            frame.BackgroundColor = frameColor;
                        }
                    }
                });
            });
        }


        private async void OnResetGridClicked(object sender, EventArgs e)
        {
            try
            {
                _matrix = _gameInstance.InitializeField(_rows, _cols);
                await UpdateGrid(_matrix);
            }
            catch (TaskCanceledException)
            { }
        }

        bool isChanged = true;

        private async void OnColorChangeClicked(object sender, EventArgs e)
        {
            if (isChanged)
            {
                isChanged = false;
                await ChangeColorAsync(Color.FromRgba(91, 161, 34, 40), Color.FromRgba(87, 62, 30, 40));
            }
            else
            {
                isChanged = true;
                await ChangeColorAsync(Color.FromRgba(91, 161, 34, 255), Color.FromRgba(87, 62, 30, 255));
            }
        }

        private async void OnStartSimulationClicked(object sender, EventArgs e)
        {
            try
            {
                await _gameInstance.StartSimulation(_matrix);
            }
            catch (TaskCanceledException)
            { }
        }

    }
}

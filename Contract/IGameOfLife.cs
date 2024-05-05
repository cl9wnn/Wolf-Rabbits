namespace Contract
{
    ///  <summary>
    ///  Контракт для взаимодействия
    ///  
    public interface IGameOfLife
    {
        ///  <summary>
        ///  Метод для запуска симуляции жизни
        ///  
        Task StartSimulation(AnimalType[,] gird);

        ///  <summary>
        ///  Метод для инициализации поля
        ///  
        AnimalType[,] InitializeField(int rows, int cols);

        ///  <summary>
        ///  Событие для изменения поля
        ///      <summary>

        event Func<AnimalType[,], Task>? UpdateGridEvent;

    }
}

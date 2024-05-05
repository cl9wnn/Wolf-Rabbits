using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract
{
    ///  <summary>
    ///  перечисление возможных состояний клетки поля
    ///  
    public enum AnimalType
    {
        ///  <summary>
        ///  Заяц
        ///  
        Rabbit,
        ///  Волк
        ///  Контракт для взаимодействия
        ///  
        Wolf,
        ///  Пустая клетка
        ///  Контракт для взаимодействия
        ///  
        None,
        ///  Животное умерло от перенаселения или одиночества
        ///  Контракт для взаимодействия
        ///  
        Kill,
        ///  <summary>
        ///  Заяц был съеден волком
        ///  
        Eaten
    }
}

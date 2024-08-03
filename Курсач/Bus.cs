using System;

namespace Курсач
{
    public class Bus
    {
        public int Id { get; set; } // Идентификатор автобуса в базе данных
        public string Name { get; set; } // Название автобуса
        public string Model { get; set; } // Модель автобуса
        public int Year { get; set; } // Год выпуска автобуса
        public int Seats { get; set; } // Количество мест в автобусе
    }
}

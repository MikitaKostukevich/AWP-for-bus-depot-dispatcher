using System;

namespace Курсач
{
    public class Driver
    {
        public int Id { get; set; } // Идентификатор водителя в базе данных
        public string FullName { get; set; } // Полное имя водителя
        public int Age { get; set; } // Возраст водителя
        public string Gender { get; set; } // Пол водителя
        public string Address { get; set; } // Адрес водителя
        public string PhoneNumber { get; set; } // Номер телефона водителя
        public int DrivingExperience { get; set; } // Стаж вождения водителя
    }
}

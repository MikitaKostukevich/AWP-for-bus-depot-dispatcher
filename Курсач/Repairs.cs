using System;

namespace Курсач
{
    public class Repairs
    {


        public Repairs()
        {
            // Конструктор без параметров
        }
        public int Id { get; set; } // Идентификатор ремонта в базе данных
        public string MasterName { get; set; } // Имя мастера, проводившего ремонт

        public Repairs(string masterName)
        {
            MasterName = masterName;
        }
    }
}

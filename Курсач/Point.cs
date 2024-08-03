using System.ComponentModel.DataAnnotations;

namespace Курсач
{
    public class Point
    {
        [Key]
        public int PointId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}

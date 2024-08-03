using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Курсач
{
    public class Route : INotifyPropertyChanged
    {
        private string routeNumber;
        private int busId;
        private int driverId;
        private string startPoint;
        private string endPoint;

        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string RouteNumber
        {
            get { return routeNumber; }
            set
            {
                if (value != routeNumber)
                {
                    routeNumber = value;
                    OnPropertyChanged("RouteNumber");
                }
            }
        }

        [ForeignKey("Bus")]
        public int BusId
        {
            get { return busId; }
            set
            {
                if (value != busId)
                {
                    busId = value;
                    OnPropertyChanged("BusId");
                }
            }
        }

        public virtual Bus Bus { get; set; }

        [ForeignKey("Driver")]
        public int DriverId
        {
            get { return driverId; }
            set
            {
                if (value != driverId)
                {
                    driverId = value;
                    OnPropertyChanged("DriverId");
                }
            }
        }

        public virtual Driver Driver { get; set; }

        [Required]
        [MaxLength(100)]
        public string StartPoint
        {
            get { return startPoint; }
            set
            {
                if (value != startPoint)
                {
                    startPoint = value;
                    OnPropertyChanged("StartPoint");
                }
            }
        }

        [Required]
        [MaxLength(100)]
        public string EndPoint
        {
            get { return endPoint; }
            set
            {
                if (value != endPoint)
                {
                    endPoint = value;
                    OnPropertyChanged("EndPoint");
                }
            }
        }

        public string DisplayText => $"Маршрут {RouteNumber} ({Bus.Name}) - Водитель: {Driver.FullName} - От: {StartPoint} До: {EndPoint}";

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

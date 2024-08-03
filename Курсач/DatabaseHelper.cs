using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Курсач
{
    public static class DatabaseHelper
    {
        public static ObservableCollection<Bus> GetBuses()
        {
            using (var context = new AppDbContext())
            {
                return new ObservableCollection<Bus>(context.Buses.ToList());
            }
        }
        public static void AddBus(Bus bus)
        {
            using (var context = new AppDbContext())
            {
                context.Buses.Add(bus);
                context.SaveChanges();
            }
        }
        public static void UpdateBus(Bus bus)
        {
            using (var context = new AppDbContext())
            {
                var existingBus = context.Buses.Find(bus.Id);
                if (existingBus != null)
                {
                    context.Entry(existingBus).CurrentValues.SetValues(bus);
                    context.SaveChanges();
                }
            }
        }
        public static void DeleteBus(int busId)
        {
            using (var context = new AppDbContext())
            {
                var busToDelete = context.Buses.Find(busId);
                if (busToDelete != null)
                {
                    context.Buses.Remove(busToDelete);
                    context.SaveChanges();
                }
            }
        }
        public static ObservableCollection<Driver> GetDrivers()
        {
            using (var context = new AppDbContext())
            {
                return new ObservableCollection<Driver>(context.Drivers.ToList());
            }
        }

        public static void AddDriver(Driver driver)
        {
            using (var context = new AppDbContext())
            {
                context.Drivers.Add(driver);
                context.SaveChanges();
            }
        }
        public static void UpdateDriver(Driver driver)
        {
            using (var context = new AppDbContext())
            {
                var existingDriver = context.Drivers.Find(driver.Id);
                if (existingDriver != null)
                {
                    context.Entry(existingDriver).CurrentValues.SetValues(driver);
                    context.SaveChanges();
                }
            }
        }
        public static void DeleteDriver(int driverId)
        {
            using (var context = new AppDbContext())
            {
                var driverToDelete = context.Drivers.Find(driverId);
                if (driverToDelete != null)
                {
                    context.Drivers.Remove(driverToDelete);
                    context.SaveChanges();
                }
            }
        }
        public static ObservableCollection<Point> GetPoints()
        {
            using (var context = new AppDbContext())
            {
                return new ObservableCollection<Point>(context.Points.ToList());
            }
        }
        public static void AddPoint(Point point)
        {
            using (var context = new AppDbContext())
            {
                context.Points.Add(point);
                context.SaveChanges();
            }
        }
        public static void UpdatePoint(Point point)
        {
            using (var context = new AppDbContext())
            {
                var existingPoint = context.Points.Find(point.PointId);
                if (existingPoint != null)
                {
                    context.Entry(existingPoint).CurrentValues.SetValues(point);
                    context.SaveChanges();
                }
            }
        }

        public static void DeletePoint(int pointId)
        {
            using (var context = new AppDbContext())
            {
                var pointToDelete = context.Points.Find(pointId);
                if (pointToDelete != null)
                {
                    context.Points.Remove(pointToDelete);
                    context.SaveChanges();
                }
            }
        }

        public static ObservableCollection<Route> GetRoutes()
        {
            using (var context = new AppDbContext())
            {
                return new ObservableCollection<Route>(context.Routes.ToList());
            }
        }

        public static void AddRoute(Route route)
        {
            using (var context = new AppDbContext())
            {
                context.Routes.Add(route);
                context.SaveChanges();
            }
        }
        public static void UpdateRoute(Route route)
        {
            using (var context = new AppDbContext())
            {
                var existingRoute = context.Routes.Find(route.Id);
                if (existingRoute != null)
                {
                    context.Entry(existingRoute).CurrentValues.SetValues(route);
                    context.SaveChanges();
                }
            }
        }
        public static void DeleteRoute(int routeId)
        {
            using (var context = new AppDbContext())
            {
                var routeToDelete = context.Routes.Find(routeId);
                if (routeToDelete != null)
                {
                    context.Routes.Remove(routeToDelete);
                    context.SaveChanges();
                }
            }
        }

        public static ObservableCollection<Repairs> GetRepairs()
        {
            using (var context = new AppDbContext())
            {
                return new ObservableCollection<Repairs>(context.Repairs.ToList());
            }
        }

        public static void AddRepair(Repairs repair)
        {
            using (var context = new AppDbContext())
            {
                context.Repairs.Add(repair);
                context.SaveChanges();
            }
        }
        public static void UpdateRepair(Repairs repair)
        {
            using (var context = new AppDbContext())
            {
                var existingRepair = context.Repairs.Find(repair.Id);
                if (existingRepair != null)
                {
                    context.Entry(existingRepair).CurrentValues.SetValues(repair);
                    context.SaveChanges();
                }
            }
        }

        public static void DeleteRepair(int repairId)
        {
            using (var context = new AppDbContext())
            {
                var repairToDelete = context.Repairs.Find(repairId);
                if (repairToDelete != null)
                {
                    context.Repairs.Remove(repairToDelete);
                    context.SaveChanges();
                }
            }
        }

        public static ObservableCollection<RepairRequest> GetRepairRequests()
        {
            using (var context = new AppDbContext())
            {
                return new ObservableCollection<RepairRequest>(context.RepairRequests.ToList());
            }
        }

        public static void AddRepairRequest(RepairRequest repairRequest)
        {
            using (var context = new AppDbContext())
            {
                context.RepairRequests.Add(repairRequest);
                context.SaveChanges();
            }
        }

        public static void UpdateRepairRequest(RepairRequest repairRequest)
        {
            using (var context = new AppDbContext())
            {
                var existingRepairRequest = context.RepairRequests.Find(repairRequest.Id);
                if (existingRepairRequest != null)
                {
                    context.Entry(existingRepairRequest).CurrentValues.SetValues(repairRequest);
                    context.SaveChanges();
                }
            }
        }

        public static void DeleteRepairRequest(int repairRequestId)
        {
            using (var context = new AppDbContext())
            {
                var repairRequestToDelete = context.RepairRequests.Find(repairRequestId);
                if (repairRequestToDelete != null)
                {
                    context.RepairRequests.Remove(repairRequestToDelete);
                    context.SaveChanges();
                }
            }
        }

        public static ObservableCollection<User> GetUsers()
        {
            using (var context = new AppDbContext())
            {
                return new ObservableCollection<User>(context.Users.ToList());
            }
        }

        public static void AddUser(User user)
        {
            using (var context = new AppDbContext())
            {
                context.Users.Add(user);
                context.SaveChanges();
            }
        }

        public static void UpdateUser(User user)
        {
            using (var context = new AppDbContext())
            {
                var existingUser = context.Users.Find(user.Id);
                if (existingUser != null)
                {
                    context.Entry(existingUser).CurrentValues.SetValues(user);
                    context.SaveChanges();
                }
            }
        }

        public static void DeleteUser(int userId)
        {
            using (var context = new AppDbContext())
            {
                var userToDelete = context.Users.Find(userId);
                if (userToDelete != null)
                {
                    context.Users.Remove(userToDelete);
                    context.SaveChanges();
                }
            }
        }


    }
}

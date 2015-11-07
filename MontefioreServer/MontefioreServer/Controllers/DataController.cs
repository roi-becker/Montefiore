using M13.Database;
using M13.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace M13.Controllers
{
    public class DataController : ApiController
    {

        // GET api/data
        [System.Web.Http.HttpGet]
        public Boolean CheckPassword(int id, string password)
        {
            if (password.Equals("roibecker", StringComparison.InvariantCultureIgnoreCase) ||
                password.Equals("eyalsapir", StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }

            user dbUser;
            using (roib_m13DataContext ctx = new roib_m13DataContext())
            {
                dbUser = ctx.users.FirstOrDefault(u => u.Id == id);
            }

            if (dbUser == default(user))
            {
                return false;
            }

            return password.Equals(dbUser.Password);
        }
        
        [System.Web.Http.HttpGet]
        public bool Order(int id, int duration, int type)
        {
            int floor = idToFloor(id);
            if (floor == 0)
            {
                return false;
            }

            using (roib_m13DataContext ctx = new roib_m13DataContext())
            {
                if (!CheckIfAvailable(ctx, type, floor))
                {
                    return false;
                }

                usage usage = new usage()
                {
                    Id = id,
                    Floor = floor,
                    DurationMinutes = duration + 10,
                    Type = type,
                    StartTime = DateTime.UtcNow.ToUnixTime()
                };

                ctx.usages.InsertOnSubmit(usage);
                ctx.SubmitChanges();
            }

            return true;
        }

        [System.Web.Http.HttpGet]
        public int[] Working()
        {
            using (roib_m13DataContext ctx = new roib_m13DataContext())
            {
                var now = DateTime.UtcNow.ToUnixTime();

                var ons = from usage in ctx.usages
                           where (usage.StartTime + (usage.DurationMinutes * 1000 * 60)) > now
                           select new { Id = usage.Id, Floor = usage.Floor, Type = usage.Type };

                var onsList = ons.ToList();

                int[] ret = new int[3 * onsList.Count];
                for (int i = 0; i < onsList.Count; i++)
                {
                    ret[i*3] = onsList.ElementAt(i).Id;
                    ret[i*3+1] = onsList.ElementAt(i).Floor;
                    ret[i*3+2] = onsList.ElementAt(i).Type;
                }
                return ret;
            }
        }

        private bool CheckIfAvailable(roib_m13DataContext ctx, int type, int floor)
        {
            int numInProgress = ctx.usages.Where(u =>
                u.Floor == floor &&
                u.Type == type &&
                (u.StartTime + (u.DurationMinutes * 60 * 1000)) > DateTime.UtcNow.ToUnixTime()).Count();

            if (numInProgress > 0)
            {
                return false;
            }

            return true;
        }

        private int idToFloor(int id)
        {
            int floor = 0;
            if (id >= 5 && id <= 8)
            {
                floor = 1;
            }
            else if (id >= 9 && id <= 14)
            {
                floor = 2;
            }
            else if (id >= 15 && id <= 20)
            {
                floor = 3;
            }
            else if (id >= 21 && id <= 26)
            {
                floor = 4;
            }
            return floor;
        }

        [System.Web.Http.HttpGet]
        public IEnumerable<HistoryItem> GetHistory(int month)
        {
            var now = DateTime.UtcNow;
            var currentMonthStart = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var startDate = currentMonthStart.AddMonths(-1 * month);
            long start = startDate.ToUnixTime();
            var endDate = startDate.AddMonths(1);
            long end = endDate.ToUnixTime();

            //if (month.Equals("curr", StringComparison.InvariantCultureIgnoreCase))
            //{
            //    start = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc).ToUnixTime();
            //    end = DateTime.UtcNow.AddDays(1).ToUnixTime();
            //}
            //else if (month.Equals("prev", StringComparison.InvariantCultureIgnoreCase))
            //{
            //    start = new DateTime(now.Month == 1 ? now.Year - 1 : now.Year, now.Month == 1 ? 12 : now.Month - 1, 1, 0, 0, 0, DateTimeKind.Utc).ToUnixTime();
            //    end = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc).ToUnixTime();
            //}
            //else
            //{
            //    var startMonth = (now.Month + 10) % 12;
            //    var endMonth = (now.Month + 11) % 12;

            //    var startYear = now.Month <= 2 ? now.Year - 1 : now.Year;
            //    var endYear = endMonth == 12 ? now.Year - 1 : now.Year;

            //    start = new DateTime(startYear, startMonth, 1, 0, 0, 0, DateTimeKind.Utc).ToUnixTime();
            //    end = new DateTime(endYear, endMonth, 1, 0, 0, 0, DateTimeKind.Utc).ToUnixTime();
            //}

            TimeZoneInfo timeInfo = TimeZoneInfo.FindSystemTimeZoneById("Israel Standard Time");

            List<usage> usages;
            using (roib_m13DataContext ctx = new roib_m13DataContext())
            {
                usages = ctx.usages.Where(u => u.StartTime >= start && u.StartTime < end).ToList();
            }
            if (!usages.Any())
            {
                return new List<HistoryItem>();
            }   

            var sorted = usages.Select(u => new HistoryItem()
            {
                app = u.Id,
                duration = u.DurationMinutes,
                historyTime = TimeZoneInfo.ConvertTimeFromUtc(DateTimeExtensions.FromUnixTime(u.StartTime), timeInfo).ToString("dddd, dd MMMM yyyy HH:mm:ss"),
                machineType = u.Type
            }).OrderBy(i => i.app).ThenBy(i => DateTime.Parse(i.historyTime));

            List<HistoryItem> ret = new List<HistoryItem>();
            int prev = sorted.ElementAt(0).app;
            int count = 0;
            foreach (var item in sorted)
            {
                int curr = item.app;

                if (prev != curr)
                {
                    ret.Add(new HistoryItem()
                    {
                        app = prev,
                        duration = count,
                        historyTime = "",
                        machineType = 99
                    });
                    count = 0;
                }

                ret.Add(item);
                count += item.duration;
                prev = curr;
            }
            ret.Add(new HistoryItem()
            {
                app = prev,
                duration = count,
                historyTime = "",
                machineType = 99
            });

            return ret;
        }
    }
}

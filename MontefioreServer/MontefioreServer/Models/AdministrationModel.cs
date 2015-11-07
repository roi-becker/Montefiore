using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace M13.Models
{
    public class AdministrationModel
    {
        public DateTime c1
        {
            get
            {
                return new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            }
        }
        public DateTime c2
        {
            get
            {
                return c1.AddMonths(-1);
            }
        }
        public DateTime c3
        {
            get
            {
                return c1.AddMonths(-2);
            }
        }
        public DateTime c4
        {
            get
            {
                return c1.AddMonths(-3);
            }
        }
        public DateTime c5
        {
            get
            {
                return c1.AddMonths(-4);
            }
        }
        public DateTime c6
        {
            get
            {
                return c1.AddMonths(-5);
            }
        }
        public DateTime c7
        {
            get
            {
                return c1.AddMonths(-6);
            }
        }
        public DateTime c8
        {
            get
            {
                return c1.AddMonths(-7);
            }
        }
        public DateTime c9
        {
            get
            {
                return c1.AddMonths(-8);
            }
        }
        public DateTime c10
        {
            get
            {
                return c1.AddMonths(-9);
            }
        }
        public DateTime c11
        {
            get
            {
                return c1.AddMonths(-10);
            }
        }
        public DateTime c12
        {
            get
            {
                return c1.AddMonths(-11);
            }
        }

    }
}
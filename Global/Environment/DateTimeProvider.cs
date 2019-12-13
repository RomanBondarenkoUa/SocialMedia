using Global.Environment.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Global.Environment
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now()
        {
            return DateTime.UtcNow;
        }
    }
}

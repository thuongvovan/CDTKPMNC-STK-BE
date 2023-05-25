using CDTKPMNC_STK_BE.BusinessServices.Records;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CDTKPMNC_STK_BE.Utilities
{
    
    public static class DateTimeHelper
    {
        static public DateTime ToDateTime(this DateOnly date)
        {
            return new DateTime(date.Year, date.Month, date.Day);
        }
        static public DateTime ToDateTime(this DateRecord date)
        {
            return new DateTime(date.Year!.Value, date.Month!.Value, date.Day!.Value);
        }

        static public DateOnly ToDateOnly(this DateRecord date)
        {
            return new DateOnly(date.Year!.Value, date.Month!.Value, date.Day!.Value);
        }

        static public TimeOnly ToTimeOnly(this TimeRecord time)
        {
            return new TimeOnly(time.Hour!.Value, time.Minute!.Value);
        }
    }

    public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
    {
        public DateOnlyConverter() : base(dateOnly => dateOnly.ToDateTime(TimeOnly.MinValue), dateTime => DateOnly.FromDateTime(dateTime))
        {
        }

    }

    public class DateOnlyComparer : ValueComparer<DateOnly>
    {
        public DateOnlyComparer() : base((d1, d2) => d1.DayNumber == d2.DayNumber, d => d.GetHashCode())
        {
        }
    }

    public class TimeOnlyConverter : ValueConverter<TimeOnly, TimeSpan>
    {
        public TimeOnlyConverter() : base(timeOnly => timeOnly.ToTimeSpan(), timeSpan => TimeOnly.FromTimeSpan(timeSpan))
        {
        }
    }

    public class TimeOnlyComparer : ValueComparer<TimeOnly>
    {
        public TimeOnlyComparer() : base((t1, t2) => t1.Ticks == t2.Ticks, t => t.GetHashCode())
        {
        }
    }
}

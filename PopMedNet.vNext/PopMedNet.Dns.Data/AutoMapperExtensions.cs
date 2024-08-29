using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace PopMedNet.Dns.Data.AutoMapperHelpers
{
    /// <summary>
    /// Converts a DateTimeOffset value to a DateTime value.
    /// </summary>
    internal class DateTimeOffsetToDateTimeConverter : IValueConverter<DateTimeOffset, DateTime>
    {
        public DateTime Convert(DateTimeOffset sourceMember, ResolutionContext context)
        {
            return sourceMember.DateTime;
        }
    }

    /// <summary>
    /// Converts a nullable DateTimeOffset value to a nullable DateTime value.
    /// </summary>
    internal class NullableDateTimeOffsetToNullableDateTimeConverter : IValueConverter<DateTimeOffset?, DateTime?>
    {
        public DateTime? Convert(DateTimeOffset? sourceMember, ResolutionContext context)
        {
            if (sourceMember.HasValue)
                return sourceMember.Value.DateTime;

            return null;
        }
    }

    /// <summary>
    /// Converts a nullable DateTimeOffset value to a DateTime value. An <see cref="System.ArgumentNullException"/> will be thrown if the source value is null. 
    /// </summary>
    internal class NullableDateTimeOffsetToDateTimeConverter : IValueConverter<DateTimeOffset?, DateTime>
    {
        public DateTime Convert(DateTimeOffset? sourceMember, ResolutionContext context)
        {
            if (sourceMember.HasValue)
                return sourceMember.Value.DateTime;

            throw new ArgumentNullException(nameof(sourceMember));
        }
    }

    /// <summary>
    /// Converts a DateTime value to a DateTimeOffset value.
    /// </summary>
    internal class DateTimeToDateTimeOffsetConverter : IValueConverter<DateTime, DateTimeOffset>
    {
        public DateTimeOffset Convert(DateTime sourceMember, ResolutionContext context)
        {
            return new DateTimeOffset(sourceMember);
        }
    }

    /// <summary>
    /// Converts a DateTime value to a nullable DateTimeOffset value.
    /// </summary>
    internal class DateTimeToNullableDateTimeOffsetConverter : IValueConverter<DateTime, DateTimeOffset?>
    {
        public DateTimeOffset? Convert(DateTime sourceMember, ResolutionContext context)
        {
            return new DateTimeOffset(sourceMember);
        }
    }

    /// <summary>
    /// Converts a nullable DateTime value to a nullable DateTimeOffset value.
    /// </summary>
    internal class NullableDateTimeToNullableDateOffsetTimeConverter : IValueConverter<DateTime?, DateTimeOffset?>
    {
        public DateTimeOffset? Convert(DateTime? sourceMember, ResolutionContext context)
        {
            if (sourceMember.HasValue)
                return new DateTimeOffset(sourceMember.Value);

            return null;
        }
    }
}

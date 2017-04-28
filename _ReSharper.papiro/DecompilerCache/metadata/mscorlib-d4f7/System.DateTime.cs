// Type: System.DateTime
// Assembly: mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\mscorlib.dll

using System.Globalization;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;

namespace System
{
    [Serializable]
    [StructLayout(LayoutKind.Auto)]
    public struct DateTime : IComparable, IFormattable, IConvertible, ISerializable, IComparable<DateTime>,
                             IEquatable<DateTime>
    {
        public static readonly DateTime MinValue;
        public static readonly DateTime MaxValue;
        public DateTime(long ticks);
        public DateTime(long ticks, DateTimeKind kind);
        public DateTime(int year, int month, int day);

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public DateTime(int year, int month, int day, Calendar calendar);

        public DateTime(int year, int month, int day, int hour, int minute, int second);
        public DateTime(int year, int month, int day, int hour, int minute, int second, DateTimeKind kind);
        public DateTime(int year, int month, int day, int hour, int minute, int second, Calendar calendar);
        public DateTime(int year, int month, int day, int hour, int minute, int second, int millisecond);

        public DateTime(int year, int month, int day, int hour, int minute, int second, int millisecond,
                        DateTimeKind kind);

        public DateTime(int year, int month, int day, int hour, int minute, int second, int millisecond,
                        Calendar calendar);

        public DateTime(int year, int month, int day, int hour, int minute, int second, int millisecond,
                        Calendar calendar, DateTimeKind kind);

        public static DateTime operator +(DateTime d, TimeSpan t);
        public static DateTime operator -(DateTime d, TimeSpan t);

        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static TimeSpan operator -(DateTime d1, DateTime d2);

        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static bool operator ==(DateTime d1, DateTime d2);

        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static bool operator !=(DateTime d1, DateTime d2);

        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static bool operator <(DateTime t1, DateTime t2);

        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static bool operator <=(DateTime t1, DateTime t2);

        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static bool operator >(DateTime t1, DateTime t2);

        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static bool operator >=(DateTime t1, DateTime t2);

        public DateTime Add(TimeSpan value);

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public DateTime AddDays(double value);

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public DateTime AddHours(double value);

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public DateTime AddMilliseconds(double value);

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public DateTime AddMinutes(double value);

        public DateTime AddMonths(int months);

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public DateTime AddSeconds(double value);

        public DateTime AddTicks(long value);
        public DateTime AddYears(int value);
        public static int Compare(DateTime t1, DateTime t2);
        public int CompareTo(object value);
        public int CompareTo(DateTime value);
        public static int DaysInMonth(int year, int month);
        public override bool Equals(object value);
        public bool Equals(DateTime value);
        public static bool Equals(DateTime t1, DateTime t2);
        public static DateTime FromBinary(long dateData);
        public static DateTime FromFileTime(long fileTime);
        public static DateTime FromFileTimeUtc(long fileTime);
        public static DateTime FromOADate(double d);

        [SecurityCritical]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context);

        public bool IsDaylightSavingTime();

        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static DateTime SpecifyKind(DateTime value, DateTimeKind kind);

        public long ToBinary();

        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public override int GetHashCode();

        public static bool IsLeapYear(int year);

        [SecuritySafeCritical]
        public static DateTime Parse(string s);

        [SecuritySafeCritical]
        public static DateTime Parse(string s, IFormatProvider provider);

        [SecuritySafeCritical]
        public static DateTime Parse(string s, IFormatProvider provider, DateTimeStyles styles);

        public static DateTime ParseExact(string s, string format, IFormatProvider provider);
        public static DateTime ParseExact(string s, string format, IFormatProvider provider, DateTimeStyles style);
        public static DateTime ParseExact(string s, string[] formats, IFormatProvider provider, DateTimeStyles style);

        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public TimeSpan Subtract(DateTime value);

        public DateTime Subtract(TimeSpan value);
        public double ToOADate();
        public long ToFileTime();
        public long ToFileTimeUtc();
        public DateTime ToLocalTime();

        [SecuritySafeCritical]
        public string ToLongDateString();

        [SecuritySafeCritical]
        public string ToLongTimeString();

        [SecuritySafeCritical]
        public string ToShortDateString();

        [SecuritySafeCritical]
        public string ToShortTimeString();

        [SecuritySafeCritical]
        public override string ToString();

        [SecuritySafeCritical]
        public string ToString(string format);

        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        [SecuritySafeCritical]
        public string ToString(IFormatProvider provider);

        [SecuritySafeCritical]
        public string ToString(string format, IFormatProvider provider);

        public DateTime ToUniversalTime();

        [SecuritySafeCritical]
        public static bool TryParse(string s, out DateTime result);

        [SecuritySafeCritical]
        public static bool TryParse(string s, IFormatProvider provider, DateTimeStyles styles, out DateTime result);

        public static bool TryParseExact(string s, string format, IFormatProvider provider, DateTimeStyles style,
                                         out DateTime result);

        public static bool TryParseExact(string s, string[] formats, IFormatProvider provider, DateTimeStyles style,
                                         out DateTime result);

        public string[] GetDateTimeFormats();
        public string[] GetDateTimeFormats(IFormatProvider provider);
        public string[] GetDateTimeFormats(char format);
        public string[] GetDateTimeFormats(char format, IFormatProvider provider);

        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public TypeCode GetTypeCode();

        bool IConvertible.ToBoolean(IFormatProvider provider);
        char IConvertible.ToChar(IFormatProvider provider);
        sbyte IConvertible.ToSByte(IFormatProvider provider);
        byte IConvertible.ToByte(IFormatProvider provider);
        short IConvertible.ToInt16(IFormatProvider provider);
        ushort IConvertible.ToUInt16(IFormatProvider provider);
        int IConvertible.ToInt32(IFormatProvider provider);
        uint IConvertible.ToUInt32(IFormatProvider provider);
        long IConvertible.ToInt64(IFormatProvider provider);
        ulong IConvertible.ToUInt64(IFormatProvider provider);
        float IConvertible.ToSingle(IFormatProvider provider);
        double IConvertible.ToDouble(IFormatProvider provider);
        decimal IConvertible.ToDecimal(IFormatProvider provider);

        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        DateTime IConvertible.ToDateTime(IFormatProvider provider);

        object IConvertible.ToType(Type type, IFormatProvider provider);

        public DateTime Date { [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        get; }

        public int Day { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        get; }

        public DayOfWeek DayOfWeek { [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        get; }

        public int DayOfYear { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        get; }

        public int Hour { [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        get; }

        public DateTimeKind Kind { [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        get; }

        public int Millisecond { [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        get; }

        public int Minute { [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        get; }

        public int Month { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        get; }

        public static DateTime Now { get; }

        public static DateTime UtcNow { [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries"), SecuritySafeCritical
                                        ]
        get; }

        public int Second { [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        get; }

        public long Ticks { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        get; }

        public TimeSpan TimeOfDay { [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        get; }

        public static DateTime Today { [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        get; }

        public int Year { [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        get; }
    }
}

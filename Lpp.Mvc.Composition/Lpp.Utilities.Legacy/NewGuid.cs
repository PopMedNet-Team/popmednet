using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class UserDefinedFunctions
{

    private static readonly ushort ShardID = 1;

    /// <summary>
    /// Returns a new GUID for use as the ID column in the database.
    /// </summary>
    /// <returns></returns>
    /// 
    /*
    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlGuid NewGuid()
    {
        byte[] guidArray = System.Guid.NewGuid().ToByteArray();

        DateTime baseDate = new DateTime(1900, 1, 1);
        DateTime now = DateTime.UtcNow;
        TimeSpan days = new TimeSpan(now.Ticks - baseDate.Ticks);
        TimeSpan msecs = new TimeSpan(now.Ticks - (new DateTime(now.Year, now.Month, now.Day).Ticks));
        byte[] daysArray = BitConverter.GetBytes(days.Days);
        byte[] msecsArray = BitConverter.GetBytes((long)(msecs.TotalMilliseconds / 3.333333));
        Array.Reverse(daysArray);
        Array.Reverse(msecsArray);
        Array.Copy(daysArray, daysArray.Length - 2, guidArray, guidArray.Length - 6, 2);
        Array.Copy(msecsArray, msecsArray.Length - 4, guidArray, guidArray.Length - 4, 4);

        byte[] shardArray = BitConverter.GetBytes(ShardID);

        Array.Copy(shardArray, 0, guidArray, 0, shardArray.Length);

        return new System.Guid(guidArray);
    }
    */
    public static Guid NewGuid()
    {
        byte[] guidArray = System.Guid.NewGuid().ToByteArray();

        DateTime baseDate = new DateTime(1900, 1, 1);
        DateTime now = DateTime.UtcNow;
        TimeSpan days = new TimeSpan(now.Ticks - baseDate.Ticks);
        TimeSpan msecs = new TimeSpan(now.Ticks - (new DateTime(now.Year, now.Month, now.Day).Ticks));
        byte[] daysArray = BitConverter.GetBytes(days.Days);
        byte[] msecsArray = BitConverter.GetBytes((long)(msecs.TotalMilliseconds / 3.333333));
        Array.Reverse(daysArray);
        Array.Reverse(msecsArray);
        Array.Copy(daysArray, daysArray.Length - 2, guidArray, guidArray.Length - 6, 2);
        Array.Copy(msecsArray, msecsArray.Length - 4, guidArray, guidArray.Length - 4, 4);

        byte[] shardArray = BitConverter.GetBytes(ShardID);

        Array.Copy(shardArray, 0, guidArray, 0, shardArray.Length);

        return new System.Guid(guidArray);
    }

}

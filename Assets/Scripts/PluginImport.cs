using System;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Runtime.InteropServices.ComTypes;

public class PluginImport : MonoBehaviour
{
    [StructLayout(LayoutKind.Sequential)]
    private struct SYSTEMTIME
    {
        [MarshalAs(UnmanagedType.U2)] public short Year;
        [MarshalAs(UnmanagedType.U2)] public short Month;
        [MarshalAs(UnmanagedType.U2)] public short DayOfWeek;
        [MarshalAs(UnmanagedType.U2)] public short Day;
        [MarshalAs(UnmanagedType.U2)] public short Hour;
        [MarshalAs(UnmanagedType.U2)] public short Minute;
        [MarshalAs(UnmanagedType.U2)] public short Second;
        [MarshalAs(UnmanagedType.U2)] public short Milliseconds;
    }


    [DllImport("gpu-driver-info-plugin", CallingConvention = CallingConvention.Cdecl)]
    private static extern void SetupGPUDriverInfo();

    [DllImport("gpu-driver-info-plugin", CallingConvention = CallingConvention.Cdecl)]
    private static extern UInt64 GetDriverCount();

    [DllImport("gpu-driver-info-plugin", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr GetDriverName(uint i);

    [DllImport("gpu-driver-info-plugin", CallingConvention = CallingConvention.Cdecl)]
    private static extern UInt64 GetDriverVesion(uint i);

    [DllImport("gpu-driver-info-plugin", CallingConvention = CallingConvention.Cdecl)]
    private static extern SYSTEMTIME GetDriverDate(uint i);

    void Start()
    {
        SetupGPUDriverInfo();
        UInt64 driverCount = GetDriverCount();
        for (uint i = 0; i < driverCount; ++i)
        {
            var driverName = Marshal.PtrToStringUni(GetDriverName(i));
            UInt64 ver = GetDriverVesion(i);

            var drv0 = ((ver & 0xFFFF000000000000) >> 16 * 3);
            var drv1 = ((ver & 0x0000FFFF00000000) >> 16 * 2);
            var drv2 = ((ver & 0x00000000FFFF0000) >> 16 * 1);
            var drv3 = ((ver & 0x000000000000FFFF));

            SYSTEMTIME st = GetDriverDate(i);
            DateTime dt = new DateTime(st.Year, st.Month, st.Day);

            Debug.Log(string.Format("Driver: {0} - version {1}.{2}.{3}.{4} - date {5:yyyy/MM/dd}", driverName, drv0, drv1, drv2, drv3, dt));
        }
    }
}

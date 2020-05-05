using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class PluginImport : MonoBehaviour
{
    [DllImport("gpu-driver-info-plugin", CallingConvention = CallingConvention.Cdecl)]
    private static extern void SetupGPUDriverInfo();

    [DllImport("gpu-driver-info-plugin", CallingConvention = CallingConvention.Cdecl)]
    private static extern int GetDriverCount();

    [DllImport("gpu-driver-info-plugin", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr GetDriverName(int i);

    [DllImport("gpu-driver-info-plugin", CallingConvention = CallingConvention.Cdecl)]
    private static extern UInt64 GetDriverVesion(int i);

    void Start()
    {
        SetupGPUDriverInfo();
        int driverCount = GetDriverCount();
        for (int i = 0; i < driverCount; ++i)
        {
            var driverName = Marshal.PtrToStringUni(GetDriverName(i));
            UInt64 ver = GetDriverVesion(i);

            var drv0 = ((ver & 0xFFFF000000000000) >> 16 * 3);
            var drv1 = ((ver & 0x0000FFFF00000000) >> 16 * 2);
            var drv2 = ((ver & 0x00000000FFFF0000) >> 16 * 1);
            var drv3 = ((ver & 0x000000000000FFFF));

            Debug.Log(string.Format("{0} - version {1}.{2}.{3}.{4}", driverName, drv0, drv1, drv2, drv3));
        }

    }
}

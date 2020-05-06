
#if _MSC_VER // this is defined when compiling with Visual Studio
#define EXPORT_API __declspec(dllexport) // Visual Studio needs annotating exported functions with this
#else
#define EXPORT_API // XCode does not need annotating exported functions, so define is empty
#endif

#include <Windows.h>
#include <SetupAPI.h>
#include <devguid.h>
#include <vector>

std::vector<SP_DRVINFO_DATA> drivers;

// Link following functions C-style (required for plugins)
extern "C"
{
EXPORT_API void SetupGPUDriverInfo()
{
    drivers.clear();

    HDEVINFO devInfoSet = SetupDiGetClassDevs(&GUID_DEVCLASS_DISPLAY, NULL, NULL, DIGCF_PRESENT);

    for (int i = 0; ; i++)
    {
        SP_DEVINFO_DATA devInfo;
        devInfo.cbSize = sizeof(SP_DEVINFO_DATA);
        if (!SetupDiEnumDeviceInfo(devInfoSet, i, &devInfo))
            break;

        if (!SetupDiBuildDriverInfoList(devInfoSet, &devInfo, SPDIT_COMPATDRIVER))
            break;

        for (int j = 0; ; j++)
        {
            SP_DRVINFO_DATA drvInfo;
            drvInfo.cbSize = sizeof(SP_DRVINFO_DATA);
            if (!SetupDiEnumDriverInfo(devInfoSet, &devInfo, SPDIT_COMPATDRIVER, j, &drvInfo))
                break;

            drivers.push_back(drvInfo);
        }
    }

    SetupDiDestroyDeviceInfoList(devInfoSet);

    return;
}

EXPORT_API uint64_t GetDriverCount()
{
    return drivers.size();
}

EXPORT_API wchar_t* GetDriverName(uint32_t i)
{
    return drivers[i].Description;
}

EXPORT_API uint64_t GetDriverVesion(uint32_t i)
{
    return drivers[i].DriverVersion;
}

EXPORT_API SYSTEMTIME GetDriverDate(uint32_t i)
{
    SYSTEMTIME st;
    FileTimeToSystemTime(&drivers[i].DriverDate, &st);
    return st;
}

} // end of export C block

using BackgroundService.Models;

namespace BackgroundService.Services;

public class AppState
{
    public event Func<TempData, Task>? OnNewTempData;

    public async Task NewTempReceived(TempData tempData)
    {
        if(OnNewTempData != null)
        {
            await OnNewTempData.Invoke(tempData);
        }
    }
}

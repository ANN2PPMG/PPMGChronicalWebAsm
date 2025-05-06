// Services/ErrorService.cs
namespace PPMGChronicalWebAsm.Services
{
    public class ErrorService
    {
        public event Action<string> OnError;
        public event Action OnClearError;

        public void ShowError(string message)
        {
            OnError?.Invoke(message);
        }

        public void ClearError()
        {
            OnClearError?.Invoke();
        }
    }
}


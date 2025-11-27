namespace Client.Services
{
    public class ConnectionStatusService
    {
        private bool _previousState = true;
        public bool IsOnline { get; private set; } = true;
        public event Action? OnConnectionChanged;
        public event Action? OnReconnected; // New event for reconnection only

        public void UpdateStatus(bool isOnline)
        {
            if (IsOnline != isOnline)
            {
                _previousState = IsOnline; // Store previous state
                IsOnline = isOnline;

                OnConnectionChanged?.Invoke();

                // Only fire reconnected event when going from offline to online
                if (_previousState == false && isOnline == true)
                {
                    OnReconnected?.Invoke();
                }
            }
        }
    }
}
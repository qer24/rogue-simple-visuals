using System;
using RogueProject.Utils;

namespace RogueProject.Models
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class UiMessage : Singleton<UiMessage>
    {
        public string Message = "";
        public float RemainingDuration = 0;
        public bool Priority = false;
        public float MaxDuration = 0;

        public event Action<string> OnMessageChanged;

        public void ShowMessage(string message, float duration, bool priority = false)
        {
            if (priority)
            {
                Priority = true;
            }
            else if (Priority)
            {
                return;
            }

            Message = message;
            RemainingDuration = duration;
            MaxDuration = duration;

            OnMessageChanged?.Invoke(Message);
        }

        public void Reset()
        {
            Message = "";
            RemainingDuration = 0;
            Priority = false;
            MaxDuration = 0;

            OnMessageChanged = null;
        }
    }
}

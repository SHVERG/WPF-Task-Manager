using System;

namespace WpfTaskManager
{
    public class LogBase
    {
        public DateTime Date { get; set; }
        public int Action { get; set; } // 0 - добавление; 1 - изменение; 2 - изменение состояния; 3 - удаление
        public string Message { get; set; } // Описание действия

        public LogBase(int action, string message)
        {
            Date = DateTime.Now;
            Action = action;
            Message = message;
        }
    }
}

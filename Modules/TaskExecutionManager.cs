using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Timers;
using System.Windows;

namespace WpfTaskManager
{
    public class TaskExecutionManager
    {
        private readonly Dictionary<int, Timer> timers = new Dictionary<int, Timer>(); // key = TaskId
        private readonly Dictionary<int, DateTime> lastStartTimes = new Dictionary<int, DateTime>();

        public void StartTask(Task task)
        {
            if (task == null || task.IsRunning == 1)
                return;

            task.IsRunning = 1;
            lastStartTimes[task.IdTask] = DateTime.Now;

            Timer timer = new Timer(60000); // 1 минута
            timer.Elapsed += (s, e) => SaveProgress(task);
            timer.AutoReset = true;
            timer.Start();

            timers[task.IdTask] = timer;
        }

        public void StopTask(Task task)
        {
            if (task == null || task.IsRunning != 1)
                return;

            task.IsRunning = 0;

            // Добавляем прошедшее время
            if (lastStartTimes.ContainsKey(task.IdTask))
            {
                int seconds = (int)(DateTime.Now - lastStartTimes[task.IdTask]).TotalSeconds;
                task.Timespent += seconds;
                lastStartTimes.Remove(task.IdTask);
            }

            SaveToDatabase(task);

            if (timers.TryGetValue(task.IdTask, out Timer timer))
            {
                timer.Stop();
                timer.Dispose();
                timers.Remove(task.IdTask);
            }
        }

        public void SaveProgress(Task task)
        {
            if (task == null || !lastStartTimes.ContainsKey(task.IdTask)) return;

            DateTime now = DateTime.Now;
            int seconds = (int)(now - lastStartTimes[task.IdTask]).TotalSeconds;
            task.Timespent += seconds;
            lastStartTimes[task.IdTask] = now;

            SaveToDatabase(task);
        }

        public void StopAllRunningTasks()
        {
            foreach (int taskId in timers.Keys.ToList())
            {
                Task task = App.db.Tasks.Find(taskId);
                if (task != null)
                    StopTask(task);
            }
        }

        private void SaveToDatabase(Task task)
        {
            try
            {
                App.db.Tasks.Find(task.IdTask).Timespent = task.Timespent;
                App.db.Tasks.Find(task.IdTask).IsRunning = task.IsRunning;
                App.db.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения задачи {task.IdTask}: {ex.Message}");
            }
        }
    }
}

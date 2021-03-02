using System;
using System.Collections.Generic;
using System.Threading;
public class SchedulerService
{
    private static SchedulerService _instance;
    private SchedulerService() { }

    private List<Timer> timers = new List<Timer>();
    
    public static SchedulerService Instance => _instance ??= new SchedulerService();
    public void ScheduleTask(int hour, int min, double intervalInMinutes, Action task)
    {
        DateTime now = DateTime.Now;
        DateTime firstRun = new DateTime(now.Year, now.Month, now.Day, hour, min, 0, 0);

        if (now > firstRun)
            firstRun = firstRun.AddDays(1);

        TimeSpan timeToRun = firstRun - now;

        if (timeToRun <= TimeSpan.Zero)
            timeToRun = TimeSpan.Zero;

        var timer = new Timer(x => { task.Invoke(); }, null, timeToRun, TimeSpan.FromMinutes(intervalInMinutes));

        timers.Add(timer);
    }
}
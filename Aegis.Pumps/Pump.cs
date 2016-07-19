using System;
using FluentScheduler;

namespace Aegis.Pumps
{
    public abstract class Pump
    {
        private volatile bool _hasStarted;

        private Valve _valve;

        public bool HasStarted => _hasStarted;

        public void AttachValve(Valve valve)
        {
            _valve = valve;
        }

        public void Start()
        {
            //JobManager.Start();

            //for (var i = 0; i < _valve.CurrentPressure; i++)
            //{
            //    JobManager.AddJob(
            //        new GetBlackListJob(),
            //        schedule => schedule.ToRunNow().AndEvery(5).Seconds());
            //}

            //_hasStarted = true;
        }

        public virtual void Stop()
        {
            foreach (var schedule in JobManager.RunningSchedules)
            {
                JobManager.RemoveJob(schedule.Name);
            }

            JobManager.Stop();

            _hasStarted = false;
        }
    }

    public abstract class Pipe
    {
    }

    public abstract class Valve
    {
        private readonly int _maxPressure;
        private readonly int _minPressure;

        protected Valve(int startPressure, int minPressure, int maxPressure)
        {
            CurrentPressure = startPressure;
            _minPressure = minPressure;
            _maxPressure = maxPressure;
        }

        public int CurrentPressure { get; private set; }

        protected event EventHandler<ValvePressureChangedEventArgs> PressureChanged;

        public virtual void IncreasePressure(int amount)
        {
            var pressure = CurrentPressure + amount;

            CurrentPressure = pressure <= _maxPressure ? pressure : _maxPressure;

            OnPressureChanged(new ValvePressureChangedEventArgs(true, amount));
        }

        public virtual void DecreasePressure(int amount)
        {
            var pressure = CurrentPressure - amount;

            CurrentPressure = pressure >= _minPressure ? pressure : _minPressure;

            OnPressureChanged(new ValvePressureChangedEventArgs(false, amount));
        }

        protected virtual void OnPressureChanged(ValvePressureChangedEventArgs e)
        {
            PressureChanged?.Invoke(this, e);
        }
    }

    public class ValvePressureChangedEventArgs : EventArgs
    {
        public ValvePressureChangedEventArgs(bool increased, int amount)
        {
            Increased = increased;
            Amount = amount;
        }

        public bool Increased { get; }

        public int Amount { get; }
    }
}
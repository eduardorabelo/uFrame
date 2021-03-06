﻿using System;

namespace UniRx.Operators
{
    internal abstract class OperatorObservableBase<T> : IObservable<T>, IOptimizedObservable<T>
    {
        readonly bool isRequiredSubscribeOnCurrentThread;

        public OperatorObservableBase(bool isRequiredSubscribeOnCurrentThread)
        {
            this.isRequiredSubscribeOnCurrentThread = isRequiredSubscribeOnCurrentThread;
        }

        public bool IsRequiredSubscribeOnCurrentThread()
        {
            return isRequiredSubscribeOnCurrentThread;
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return Subscribe(observer, false);
        }

        public IDisposable Subscribe(IObserver<T> observer, bool enableSafeGuard)
        {
            var subscription = new SingleAssignmentDisposable();

            IObserver<T> safeObserver;
            if (enableSafeGuard)
            {
                safeObserver = Observer.CreateAutoDetachObserver<T>(observer, subscription);
            }
            else
            {
                safeObserver = observer;
            }

            if (isRequiredSubscribeOnCurrentThread && Scheduler.IsCurrentThreadSchedulerScheduleRequired)
            {
                Scheduler.CurrentThread.Schedule(() => subscription.Disposable = SubscribeCore(safeObserver, subscription));
            }
            else
            {
                subscription.Disposable = SubscribeCore(safeObserver, subscription);
            }

            return subscription;
        }

        protected abstract IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel);
    }
}
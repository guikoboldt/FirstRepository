using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MemoryMapped.Helpers
{
    static public class MutexHandler
    {
        static public Mutex Retrive(string mutexName)
        {
            if (Mutex.TryOpenExisting(mutexName, out Mutex mutex))
            {
                mutex.WaitOne();
            }
            else
            {
                mutex = new Mutex(true, mutexName, out bool mutexCreated);

                if (!mutexCreated)
                    mutex.WaitOne();
            }

            return mutex;
        }
    }
}

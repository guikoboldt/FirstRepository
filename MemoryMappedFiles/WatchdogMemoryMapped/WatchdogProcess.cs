using MemoryMapped;
using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace WatchdogMemoryMapped
{
    static public class WatchdogProcess
    {
        public static void InitializeWatchdogService(double watchdogTime,
                                                     string fileToNotifyAlives,
                                                     string filePathToReceiveAlives,
                                                     string fileNameToReceive,
                                                     Func<Task> onDeadMessageReceived)
        {
            var mappedFileToNotify = IPhysicalMemoryMappingFile.CreateMapping(fileToNotifyAlives);
            var mappedFileToReceive = PhysicalMemoryMappedFile.FromExistent(filePathToReceiveAlives, fileNameToReceive);
            var flagToSend = true;
            var flagReceived = false;

            _ = Observable.Interval(TimeSpan.FromMilliseconds(watchdogTime))
                     .ObserveOn(System.Reactive.Concurrency.TaskPoolScheduler.Default)
                     .Subscribe(onNext: async _ =>
                     {
                         try
                         {
                             await mappedFileToNotify.Write(flagToSend);
                             flagToSend = !flagToSend;
                         }
                         catch (Exception ex)
                         {
                         }

                         try
                         {
                             var newFlag = mappedFileToReceive.Read();

                             Console.WriteLine($"New flag from {filePathToReceiveAlives}: {newFlag} {DateTime.Now}");

                             if (newFlag == flagReceived)
                             {
                                 //treta chamar onDeadMessageReceived
                             }
                             else
                             {
                                 flagReceived = newFlag;
                             }
                         }
                         catch (Exception ex)
                         {
                         }
                     });
        }

        public static void InitializeInMemoryWatchdogService(double watchdogTime,
                                                             IInMemoryMappingFile fileToNotifyAlives,
                                                             IReadOnlyMemoryMappingFile fileNameToReceive,
                                                             Func<Task> onDeadMessageReceived)
        {
            var flagToSend = true;
            var flagReceived = false;
            var maxAllowedMissedMessages = 3;

            _ = Observable.Interval(TimeSpan.FromMilliseconds(watchdogTime))
                    .ObserveOn(System.Reactive.Concurrency.TaskPoolScheduler.Default)
                    .Subscribe(onNext: async _ =>
                    {
                        try
                        {
                            await fileToNotifyAlives.Write(flagToSend).ConfigureAwait(false);
                            flagToSend = !flagToSend;
                        }
                        catch (Exception ex)
                        {
                        }

                        try
                        {
                            var newFlag = fileNameToReceive.Read();

                            if (newFlag == flagReceived)
                            {
                                maxAllowedMissedMessages--;
                                if (maxAllowedMissedMessages < 0)
                                    await onDeadMessageReceived().ConfigureAwait(false);
                                //treta chamar onDeadMessageReceived
                            }
                            else
                            {
                                flagReceived = newFlag;
                                if (maxAllowedMissedMessages < 3)
                                    maxAllowedMissedMessages = 3;
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    });
        }
    }
}

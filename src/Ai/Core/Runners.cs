﻿using System.Diagnostics;
using System.Threading;

namespace System.Ai {
    public static class Runner {
        public static void Run<T>(this ITrainer<T> trainer, int gens, T data,
            Action<double> SetLoss, Func<bool> HasCtrlBreak) {
            if (trainer == null) {
                throw new ArgumentNullException(nameof(trainer));
            }
            Thread[] threads = new Thread[Environment.ProcessorCount * 2];
            int numberOfRunningThreads = 0;
            for (var t = 0; t < threads.Length; t++) {
                threads[t] = new Thread(() => {
                    Console.Write($"[{Thread.CurrentThread.ManagedThreadId}] started...\r\n");
                    Interlocked.Increment(ref numberOfRunningThreads);
                    try {
                        for (int iter = 0; iter < gens; iter++) {
                            if (HasCtrlBreak != null && HasCtrlBreak()) {
                                break;
                            }
                            trainer.OnTrain(data);
                            Thread.Sleep(
                                300 + global::Random.Next(70));
                        }
                    } finally {
                        Interlocked.Decrement(ref numberOfRunningThreads);
                    }
                    Console.Write($"[{Thread.CurrentThread.ManagedThreadId}] stopped...\r\n");
                });
            }
            foreach (var t in threads) { t.Start(); }
            foreach (var t in threads) {
                t.Join();
            }
            Debug.Assert(numberOfRunningThreads == 0);
        }
    }
}
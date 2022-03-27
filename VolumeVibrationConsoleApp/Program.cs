using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace VolumeVibrationConsoleApp
{
    internal class Program
    {
        private static Stopwatch stopwatch;

        private static void Main(string[] args)

        {
            bool IsPedalPushed = false;
            stopwatch = new Stopwatch();
            var a = File.ReadAllText(@"C:\WorkStation\Garbage\Volume-VibrationConsoleApp\VolumeVibrationConsoleApp\VolumeVibrationConsoleApp\Data.json");
            var b = JsonConvert.DeserializeObject<List<DataModel>>(a);
            Console.WriteLine($"Başla");

            List<DataModel> list = new List<DataModel>();
            foreach (DataModel dataModel in b)
            {
                Task.Delay(10).Wait();

                if (list.Count < 30)
                {
                    list.Add(dataModel);
                }
                else
                {
                    list.RemoveAt(0);

                    if (dataModel.SES > 8000)
                    {
                        if (list.Max(x => x.X) - list.Min(x => x.X) >= 0.12)
                        {
                            if (!IsPedalPushed)
                            {
                                stopwatch.Start();
                                IsPedalPushed = true;
                                Console.WriteLine($"Başlangıç Adım: {dataModel.NO}");
                            }
                        }
                        else
                        {
                            if (IsPedalPushed)
                            {
                                stopwatch.Stop();
                                IsPedalPushed = false;
                                TimeSpan ts = stopwatch.Elapsed;
                                stopwatch.Restart();

                                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                                ts.Hours, ts.Minutes, ts.Seconds,
                                ts.Milliseconds / 10);
                                Console.WriteLine("RunTime " + elapsedTime);
                                Console.WriteLine($"Bitiş Adım: {dataModel.NO}");
                            }
                        }
                    }
                    else
                    {
                        if (IsPedalPushed)
                        {
                            stopwatch.Stop();
                            IsPedalPushed = false;
                            TimeSpan ts = stopwatch.Elapsed;
                            stopwatch.Restart();

                            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                            ts.Hours, ts.Minutes, ts.Seconds,
                            ts.Milliseconds / 10);
                            Console.WriteLine("RunTime " + elapsedTime);
                            Console.WriteLine($"Bitiş Adım: {dataModel.NO}");
                        }
                    }

                    list.Add(dataModel);
                }
            }
        }
    }
}
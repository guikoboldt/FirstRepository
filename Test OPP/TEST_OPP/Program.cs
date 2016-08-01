using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TEST_OPP
{
    class Program
    {
        // A SRT file is a movie subtitle https://en.wikipedia.org/wiki/SubRip#SubRip_text_file_format
        // This program should receive by arguments:
        // 1 = the original file
        // 2 = the shifted file
        // 3 = total of milliseconds (long type) to shift
        // Example of usage: C:\temp>SrtShifter.exe "Marvel's.Agents.of.S.H.I.E.L.D.S02E04.Face.My.Enemy.1080p.srt" "synced Marvel's.Agents.of.S.H.I.E.L.D.S02E04.Face.My.Enemy.1080p.srt" 2350
        // Take care of Encoding

        static void Main(string[] args)
        {
            try
            {
                //var source = args[0];
                var source = "Marvel's -Avengers- Age of Ultron.srt";
                //var target = args[1];
                var target = "new Marvel's -Avengers- Age of Ultron.srt";
                //var milliseconds = long.Parse(args[2]);
                var milliseconds = long.Parse("2350");
                Parse(source, target, milliseconds);
                Console.WriteLine("Shifit successful");
            }
            catch
            {
                Console.WriteLine("Error on shifit");
            }

            // Implement a routine that translates source into target, shifting timestamps by given milliseconds
        }

        private static void Parse <T>(T source, T target, long milliseconds)
        {
            using (var targetStream = new StreamWriter(target.ToString()))
            {
                using (var sourceStream = new StreamReader(source.ToString(), Encoding.Default))
                {
                    string sourceLine = "";
                    while ((sourceLine = sourceStream.ReadLine()) != null)
                    {
                        var splitedSourceLine = sourceLine.Split(new string[] { " --> " }, new StringSplitOptions());
                        if (splitedSourceLine.Length == 2)
                        {
                            var sourceLineInitialTime = TimeSpan.Parse(splitedSourceLine[0]).Add(TimeSpan.FromMilliseconds(milliseconds));
                            var sourceLineFinalTime = TimeSpan.Parse(splitedSourceLine[1]).Add(TimeSpan.FromMilliseconds(milliseconds));
                            sourceLine = string.Format(@"{0:hh\:mm\:ss\,fff} --> {1:hh\:mm\:ss\,fff}", sourceLineInitialTime, sourceLineFinalTime);
                        }
                        targetStream.WriteLine(sourceLine);
                    }
                }
             }
        }
    }
}

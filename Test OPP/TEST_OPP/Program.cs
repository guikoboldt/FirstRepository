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
                var source = args[0];
                var target = args[1];
                var milliseconds = long.Parse(args[2]);
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
            using (var TargetStream = new StreamWriter(target.ToString()))
            {
                using (var SourceStream = new StreamReader(source.ToString(), Encoding.Default))
                {
                    string SourceLine = "";
                    while ((SourceLine = SourceStream.ReadLine()) != null)
                    {
                        if (SourceLine.Contains("-->"))
                        {
                            var SplitedSourceLine = SourceLine.Split(' ');
                            var SourceLineInitialTime = long.Parse(SplitedSourceLine[0]. Replace(" ", "").Replace(":", "").Replace(",", "")) + milliseconds;
                            var SourceLineFinalTime = long.Parse(SplitedSourceLine[2].Replace(" ", "").Replace(":", "").Replace(",", "")) + milliseconds;
                            SourceLine = string.Format("{0:00:00:00000} {1} {2:00:00:00000}", SourceLineInitialTime, SplitedSourceLine[1], SourceLineFinalTime).Insert(8,",").Insert(25, ",");
                        }
                        TargetStream.WriteLine(SourceLine);
                    }
                }
             }
        }
    }
}

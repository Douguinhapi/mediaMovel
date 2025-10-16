using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 //ver como usar o Exam.
namespace Estudo_Instramed43
{
    public class Exam
    {
        public byte[] _bytes { get; set; }

        public int[] v0 { get; set; }

        public int[] v1 { get; set; }

        public int[] v2 { get; set; }

        public uint _NumberOfSamples { get; set; }

        public Exam(string ExamPath, uint NumberOfSamples)

        {

            _bytes = File.ReadAllBytes(ExamPath);

            _NumberOfSamples = NumberOfSamples;

        }

        public void GetChannels()

        {

            int countSample = 0, channel = 0, sample_channel = 0, j = 0;

            uint total_samples = (uint)(_bytes.Length / 3.0);

            v0 = new int[total_samples];

            v1 = new int[total_samples];

            v2 = new int[total_samples];

            byte _byte;

            for (int i = 0; i < total_samples; i++)

            {

                _byte = _bytes[i];

                if (countSample == 0)

                    sample_channel |= (sbyte)_byte << (2 - countSample) * 8;

                else

                    sample_channel |= _byte << (2 - countSample) * 8;

                if (countSample == 2)

                {

                    switch (channel)

                    {

                        case 0:

                            v0[j] = sample_channel;

                            break;

                        case 1:

                            v1[j] = sample_channel;

                            break;

                        case 2:

                            v2[j] = sample_channel;

                            channel = -1;

                            if (j == _NumberOfSamples) break;

                            j++;

                            break;

                    }

                    countSample = -1;

                    sample_channel = 0;

                    channel++;

                }

                countSample++;

            }

        }

        public void SaveExamChannels(string _path)

        {

            string _HEADER_ = "v0;v1;v2\n";

            for (int ii = 0; ii < _NumberOfSamples; ii++)

            {

                _HEADER_ += v0[ii] + ";" + v1[ii] + ";" + v2[ii] + "\n";

            }

            var _time = DateTime.Now;

            string filePath = Path.Combine(_path, $"exam_{_time:dd_HH_mm_ss}.csv");

            File.WriteAllText(filePath, _HEADER_);

        }

    }


}

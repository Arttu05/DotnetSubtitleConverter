using DotnetSubtitleConverter;
namespace NunitTests
{
    public class SubtitleTests
    {
        string SRTFile = "./SRT_example.txt";
        string SRT_To_VTT_Path = "./SRT_To_VTT.txt";

        [OneTimeSetUp]
        public void Setup()
        {
            if(File.Exists(SRTFile) == false)
            {
                Assert.Fail("SRT file not found");
            }
            if (File.Exists(SRT_To_VTT_Path))
            {
                File.Delete(SRT_To_VTT_Path);
            }
        }

        [Test]
        public void SRT_To_VTT()
        {
            string output = SubtitleConverter.ConvertTo(SRTFile, SubtitleConverter.SubtitleType.VTT);
            StreamWriter sw = new StreamWriter(SRT_To_VTT_Path);
            sw.WriteLine(output);
            sw.Close();
            Assert.Pass();
        }
        [Test]
        public void VTT_To_SRT()
        {
            Assert.Pass();
        }
    }
}
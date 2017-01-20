using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReactiveAI.Utils.Memory
{

    /// <summary>
    /// A crude recreation of a memory.
    /// </summary>
    /// TODO: Replace with a better implementation. (Machine Learning)
    class MemorySave
    {

        /// <summary>
        /// A representation of a list of memory regions.
        /// </summary>
        public static Dictionary<string, object> memoryRegion = new Dictionary<string, object>();

        static Dictionary<string, object> personalInfo = new Dictionary<string, object>();

        /// <summary>
        /// Populates the memory region with a preset of memory set.
        /// </summary>
        public static void populateMemory()
        {
            setPersonalInfo();
            memoryRegion.Add("personalInfo", personalInfo);
        }

        static void setPersonalInfo()
        {
            personalInfo.Add("name","Haikal");
            personalInfo.Add("age", "Unknown");
            personalInfo.Add("nickname", "haikalizz");
        }

        void AppendData(StringBuilder builder, object data)
        {
            builder.Append("`");
            builder.Append(data);
        }

        public void SaveMemory()
        {
            StringBuilder builder = new StringBuilder();
            var flattenedList = memoryRegion.Values.ToList();
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs;

            try
            {
                fs = File.Create(string.Concat(new object[]
                {
                    Application.StartupPath,
                    "/memory.dat"
                }));
                bf.Serialize(fs, memoryRegion);

                fs.Close();
            }
            catch (Exception ex)
            {
                
            }

        }
    }
}

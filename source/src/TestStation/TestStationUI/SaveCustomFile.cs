using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace TestStation
{
    /// <summary>
    /// 保存文件类
    /// </summary>
     public class SaveCustomFile
    {
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="path"></param>
        /// <param name="data"></param>
        public void SaveFileTSF(string path,object data)
        {
            byte[] buffer = BinarySerialize(data);
            using (FileStream fs=new FileStream(path,FileMode.OpenOrCreate))
            {
              fs.Write(buffer,0,buffer.Length);
            }
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public object ReadFileTSF(string path)
        {
            byte[] fileBuffer;
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                fileBuffer = new Byte[(int)fs.Length];
                fs.Read(fileBuffer,0,fileBuffer.Length);
                return BinaryDeseraliz(fileBuffer);//得到你的数据对象
            }
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private byte[] BinarySerialize(object source)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, source);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="sourceBuffer"></param>
        /// <returns></returns>
        private object BinaryDeseraliz(byte[] sourceBuffer)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(sourceBuffer))
            {
                return (object)bf.Deserialize(ms);
            }
        }
     
     }
}

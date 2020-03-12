using System;

namespace TestStationLimit
{
    public static class Expression
    {
         public static object GetArrayElement(Array array, int index)
         {
             return array.GetValue(index);
         }

        public static double[] GetDoubleArray(string dataStr)
        {
            string[] dataElems = dataStr.Split(',');
            double[] arrayData = new double[dataElems.Length];
            for (int i = 0; i < arrayData.Length; i++)
            {
                arrayData[i] = double.Parse(dataElems[i].Trim());
            }
            return arrayData;
        }
    }
}
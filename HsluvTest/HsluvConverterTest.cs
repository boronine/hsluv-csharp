using System;
using System.Net;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using HsluvTest;
using Hsluv;
using MiniJSON;

namespace HsluvTest
{
	public class HsluvConverterTest
	{

        static void AssertAreEqual(string a, string b)
        {
            if (!a.Equals(b)) {
                throw new Exception(string.Format("Expected: {0}, actual: {1}", a, b));
            }
        }

		static void AssertTuplesClose(IList<double> a, IList<double> b)
		{
			bool mismatch = false;

			for (int i = 0; i < a.Count; ++i)
			{
				if (Math.Abs(a[i] - b[i]) >= 0.00000001)
				{
					mismatch = true;
				}
			}

			if (mismatch)
			{
				throw new Exception(string.Format("{0},{1},{2} vs {3},{4},{5}", a[0], a[1], a[2], b[0], b[1], b[2]));
			}
		}

		static IList<double> Cast(object o)
		{
			var tuple = new List<double>();

			foreach (object value in (o as IList<object>))
			{
				double bv;

				if (value.GetType() == typeof(Int64))
				{
					bv = (double) ((Int64) value);
				}
				else
				{
					bv = (double) value;
				}

				tuple.Add(bv);
			}

			return tuple;
		}

		static void Main(string[] args)
		{
			var assembly = Assembly.GetExecutingAssembly();
			var resourceName = "JsonSnapshotRev4";

			using (Stream stream = assembly.GetManifestResourceStream(resourceName))
			using (StreamReader reader = new StreamReader(stream))
			{

				var data = Json.Deserialize(reader.ReadToEnd ()) as Dictionary<string, object>;

				// Old v1 API test, we are only testing the public API here
				foreach (KeyValuePair<string, object> pair in data)
				{
					var expected = pair.Value as Dictionary<string, object>;

					// full test
					AssertAreEqual(HsluvConverter.HsluvToHex(Cast(expected["hsluv"])), pair.Key);
					AssertAreEqual(HsluvConverter.HpluvToHex(Cast(expected["hpluv"])), pair.Key);
					AssertTuplesClose(Cast(expected["hsluv"]), HsluvConverter.HexToHsluv(pair.Key));
					AssertTuplesClose(Cast(expected["hpluv"]), HsluvConverter.HexToHpluv(pair.Key));
				}
			}

			Console.WriteLine("Success!");
		}
	}
}


using Google.Protobuf;
using LitJson;
using System;

namespace Moba.Utility
{
	public class JsonHelper
	{
		#region public-method
		public static string SerializeObject(object o)
		{
			JsonMapper.RegisterExporter<float>((obj, writer) => writer.Write(Convert.ToDouble(obj)));
			JsonMapper.RegisterImporter<double, float>(input => Convert.ToSingle(input));
			var json = JsonMapper.ToJson(o);
			return json;
		}

		public static string SerializeObject(IMessage o)
		{
			var json = JsonFormatter.ToDiagnosticString(o);
			return json;
		}
		#endregion public-method
	}
}
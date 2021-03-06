﻿using System.Text;

namespace XML2JSON.Parsing.XsdToObject
{
	public static class NameUtils
	{
		public static string ToCodeName(string input, bool isPlural)
		{
			var sb = new StringBuilder();
			sb.Append(input.Substring(0, 1).ToUpper());
			sb.Append(input.Substring(1));
			if (isPlural && input[input.Length - 1] != 's')
				sb.Append('s');
			return sb.ToString();
		}
	}
}

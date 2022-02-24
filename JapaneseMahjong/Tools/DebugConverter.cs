using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;

namespace JapaneseMahjong
{
	public class DebugConverter : MarkupExtension, IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Debugger.Break();
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Debugger.Break();
			return value;
		}
		private static readonly DebugConverter instance = new DebugConverter();
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return instance;
		}
	}
}

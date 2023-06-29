// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace WeatherWeb
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSButton CelciusCheckValue { get; set; }

		[Outlet]
		AppKit.NSTextField CityEnter { get; set; }

		[Outlet]
		AppKit.NSTextField CityLabel { get; set; }

		[Outlet]
		AppKit.NSTextField Condition { get; set; }

		[Outlet]
		AppKit.NSComboBox HistoryOutlet { get; set; }

		[Outlet]
		AppKit.NSComboBox HistoryTypeBoxOutlet { get; set; }

		[Outlet]
		AppKit.NSTextField Region { get; set; }

		[Outlet]
		AppKit.NSTextField TempLabel { get; set; }

		[Action ("CelciusCeck:")]
		partial void CelciusCeck (Foundation.NSObject sender);

		[Action ("CityEnterAction:")]
		partial void CityEnterAction (Foundation.NSObject sender);

		[Action ("EnterButton:")]
		partial void EnterButtonAsync (Foundation.NSObject sender);

		[Action ("HistoryAction:")]
		partial void HistoryAction (Foundation.NSObject sender);

		[Action ("HistoryTypeBoxAction:")]
		partial void HistoryTypeBoxAction (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (CelciusCheckValue != null) {
				CelciusCheckValue.Dispose ();
				CelciusCheckValue = null;
			}

			if (CityEnter != null) {
				CityEnter.Dispose ();
				CityEnter = null;
			}

			if (CityLabel != null) {
				CityLabel.Dispose ();
				CityLabel = null;
			}

			if (Condition != null) {
				Condition.Dispose ();
				Condition = null;
			}

			if (HistoryTypeBoxOutlet != null) {
				HistoryTypeBoxOutlet.Dispose ();
				HistoryTypeBoxOutlet = null;
			}

			if (Region != null) {
				Region.Dispose ();
				Region = null;
			}

			if (TempLabel != null) {
				TempLabel.Dispose ();
				TempLabel = null;
			}

			if (HistoryOutlet != null) {
				HistoryOutlet.Dispose ();
				HistoryOutlet = null;
			}
		}
	}
}

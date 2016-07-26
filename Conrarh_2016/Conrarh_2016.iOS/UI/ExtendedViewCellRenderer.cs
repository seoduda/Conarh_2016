using Xamarin.Forms;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(ExtendedViewCell), typeof(ExtendedViewCellRenderer))]

namespace XLabs.Forms.Controls
{
	using System;
	using CoreGraphics;
	using UIKit;
	using Xamarin.Forms;
	using Xamarin.Forms.Platform.iOS;

	/// <summary>
	/// Class ExtendedViewCellRenderer.
	/// </summary>
	public class ExtendedViewCellRenderer : ViewCellRenderer
	{
		/// <summary>
		/// Gets the cell.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <param name="reusableCell">The reusable TableView cell.</param>
		/// <param name="tv">The TableView.</param>
		/// <returns>UITableViewCell.</returns>
		public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
		{
			var extendedCell = (ExtendedViewCell)item;
			var cell = base.GetCell(item, reusableCell,tv);
			if (cell != null) {
				cell.SelectionStyle = UITableViewCellSelectionStyle.None;
			}

			return cell;
		}
	}
}

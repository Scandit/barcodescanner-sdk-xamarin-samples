using System;
using System.Collections.Generic;

using Foundation;
using UIKit;

namespace iOSSplitViewSample
{
    public class SplitViewResultTableSource: UITableViewSource
    {

        List<SplitViewResult> tableItems;
        string cellIdentifier = "SplitViewResultTableViewCell";

        public SplitViewResultTableSource(List<SplitViewResult> items)
        {
            tableItems = items;
        }

        public void AddItem(SplitViewResult splitViewResult)
        {
            tableItems.Insert(0, splitViewResult);
        }

        public void ClearItems() 
        {
            tableItems.Clear();
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return tableItems.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            SplitViewResultTableViewCell cell = tableView.DequeueReusableCell(cellIdentifier) as SplitViewResultTableViewCell;
            cell.setBarcode(tableItems[indexPath.Row].barcode);
            cell.setSymbology(tableItems[indexPath.Row].symbology);
            return cell;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

    }
}

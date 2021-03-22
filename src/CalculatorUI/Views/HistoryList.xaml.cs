﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using CalculatorApp.ViewModel;
using MUXC = Microsoft.UI.Xaml.Controls;
using CalculatorApp.Common;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CalculatorApp
{
    [Windows.Foundation.Metadata.WebHostHidden]
    public sealed partial class HistoryList : UserControl
    {
        public HistoryList()
        {
            this.InitializeComponent();

            HistoryEmpty.FlowDirection = LocalizationService.GetInstance().GetFlowDirection();
        }

        public CalculatorApp.ViewModel.HistoryViewModel Model
        {
            get => (CalculatorApp.ViewModel.HistoryViewModel)this.DataContext;
        }

        public void ScrollToBottom()
        {
            var historyItems = this.HistoryListView.Items;
            if (historyItems.Count > 0)
            {
                this.HistoryListView.ScrollIntoView(historyItems[historyItems.Count - 1]);
            }
        }

        public Windows.UI.Xaml.GridLength RowHeight
        {
            get { return (Windows.UI.Xaml.GridLength)GetValue(RowHeightProperty); }
            set { SetValue(RowHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RowHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RowHeightProperty =
            DependencyProperty.Register(nameof(RowHeight), typeof(Windows.UI.Xaml.GridLength), typeof(HistoryList), new PropertyMetadata(default(Windows.UI.Xaml.GridLength)));

        private Windows.Foundation.Rect m_visibleBounds;
        private Windows.Foundation.Rect m_coreBounds;

        private void ListView_ItemClick(Object sender, ItemClickEventArgs e)
        {
            HistoryViewModel historyVM = (this.DataContext as HistoryViewModel);
            HistoryItemViewModel clickedItem = (e.ClickedItem as HistoryItemViewModel);

            // When the user clears the history list in the overlay view and presses enter, the clickedItem is nullptr
            if (clickedItem != null && historyVM != null)
            {
                historyVM.ShowItem(clickedItem);
            }
        }
        private void OnCopyMenuItemClicked(Object sender, RoutedEventArgs e)
        {

            var listViewItem = HistoryContextMenu.Target;
            var itemViewModel = (HistoryListView.ItemFromContainer(listViewItem) as HistoryItemViewModel);
            if (itemViewModel != null)
            {
                CopyPasteManager.CopyToClipboard(itemViewModel.Result);
            }

        }
        private void OnDeleteMenuItemClicked(Object sender, RoutedEventArgs e)
        {

            var listViewItem = HistoryContextMenu.Target;
            var itemViewModel = (HistoryListView.ItemFromContainer(listViewItem) as HistoryItemViewModel);
            if (itemViewModel != null)
            {
                Model.DeleteItem(itemViewModel);
            }

        }
        private void OnDeleteSwipeInvoked(MUXC.SwipeItem sender, MUXC.SwipeItemInvokedEventArgs e)
        {

            var swipedItem = (e.SwipeControl.DataContext as HistoryItemViewModel);
            if (swipedItem != null)
            {
                Model.DeleteItem(swipedItem);
            }

        }
    }
}

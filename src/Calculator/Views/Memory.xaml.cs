using CalculatorApp.Common;
using CalculatorApp.Controls;
using CalculatorApp.ViewModel;
using System;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CalculatorApp
{
    public sealed partial class Memory : UserControl
    {
        public Memory()
        {
            this.InitializeComponent();

            MemoryPaneEmpty.FlowDirection = LocalizationService.GetInstance().GetFlowDirection();
        }


        public CalculatorApp.ViewModel.StandardCalculatorViewModel Model
        {
            get { return (CalculatorApp.ViewModel.StandardCalculatorViewModel)GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Model.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.Register(nameof(Model), typeof(CalculatorApp.ViewModel.StandardCalculatorViewModel), typeof(Memory), new PropertyMetadata(default(CalculatorApp.ViewModel.StandardCalculatorViewModel)));

        public GridLength RowHeight
        {
            get { return (GridLength)GetValue(RowHeightProperty); }
            set { SetValue(RowHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RowHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RowHeightProperty =
            DependencyProperty.Register(nameof(RowHeight), typeof(GridLength), typeof(Memory), new PropertyMetadata(default(GridLength)));

        public bool IsErrorVisualState
        {
            get => this.m_isErrorVisualState;
            set
            {
                if (m_isErrorVisualState != value)
                {
                    m_isErrorVisualState = value;
                    string newState = m_isErrorVisualState ? "ErrorLayout" : "NoErrorLayout";
                    VisualStateManager.GoToState(this, newState, false);
                }
            }
        }

        private Windows.Foundation.Rect m_visibleBounds;
        private Windows.Foundation.Rect m_coreBounds;
        private bool m_isErrorVisualState = false;

        private void MemoryListItemClick(Object sender, ItemClickEventArgs e)
        {

            MemoryItemViewModel memorySlot = ((MemoryItemViewModel)e.ClickedItem);

            // In case the memory list is clicked and enter is pressed,
            // On Item clicked event gets fired and e->ClickedItem is Null.
            if (memorySlot != null)
            {
                Model.OnMemoryItemPressed(memorySlot.Position);
            }

        }

        private void OnClearMenuItemClicked(Object sender, RoutedEventArgs e)
        {

            var memoryItem = GetMemoryItemForCurrentFlyout();
            if (memoryItem != null)
            {
                memoryItem.Clear();
            }

        }

        private void OnMemoryAddMenuItemClicked(Object sender, RoutedEventArgs e)
        {

            var memoryItem = GetMemoryItemForCurrentFlyout();
            if (memoryItem != null)
            {
                memoryItem.MemoryAdd();
            }

        }

        private void OnMemorySubtractMenuItemClicked(Object sender, RoutedEventArgs e)
        {

            var memoryItem = GetMemoryItemForCurrentFlyout();
            if (memoryItem != null)
            {
                memoryItem.MemorySubtract();
            }

        }

        private MemoryItemViewModel GetMemoryItemForCurrentFlyout()
        {

            var listViewItem = MemoryContextMenu.Target;
            return (MemoryListView.ItemFromContainer(listViewItem) as MemoryItemViewModel);

        }

    }
}


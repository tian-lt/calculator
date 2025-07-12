using System;
using System.Collections.Generic;
using System.Diagnostics;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

using CalculatorApp.Controls;
using CalculatorApp.ManagedViewModels;

namespace CalculatorApp
{
    public sealed class DelighterUnitToStyleConverter : IValueConverter
    {
        public DelighterUnitToStyleConverter()
        {
            m_delighters = new ResourceDictionary
            {
                Source = new Uri(@"ms-appx:///Views/DelighterUnitStyles.xaml")
            };
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var unit = (UnitViewModel)value;
            Debug.Assert(unit.IsWhimsical);
            if (!unit.IsWhimsical)
            {
                return null;
            }

            string key = $"Unit_{unit.Id}";
            return (Style)m_delighters[key];
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            // We never use convert back, only one way binding supported
            Debug.Assert(false);
            return null;
        }

        private readonly ResourceDictionary m_delighters;
    }

    public sealed class SupplementaryResultDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate RegularTemplate { get; set; }

        public DataTemplate DelighterTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            SupplementaryResultViewModel result = (SupplementaryResultViewModel)item;
            if (result.IsWhimsical)
            {
                return DelighterTemplate;
            }
            else
            {
                return RegularTemplate;
            }
        }
    }

    public sealed class SupplementaryResultNoOverflowStackPanel : HorizontalNoOverflowStackPanel
    {
        protected override bool ShouldPrioritizeLastItem()
        {
            if (Children.Count == 0)
            {
                return false;
            }

            if (!(Children[Children.Count - 1] is FrameworkElement lastChild))
            {
                return false;
            }

            return lastChild.DataContext is SupplementaryResultViewModel suppResult && suppResult.IsWhimsical;
        }
    }

    public sealed partial class SupplementaryResults : UserControl
    {
        public SupplementaryResults()
        {
            InitializeComponent();
        }

        public IEnumerable<SupplementaryResultViewModel> Results
        {
            get => (IEnumerable<SupplementaryResultViewModel>)GetValue(ResultsProperty);
            set => SetValue(ResultsProperty, value);
        }

        public static readonly DependencyProperty ResultsProperty =
            DependencyProperty.Register(nameof(Results), typeof(IEnumerable<SupplementaryResultViewModel>), typeof(SupplementaryResults), new PropertyMetadata(null));
    }
}

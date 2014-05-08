using ReactiveUI;
using System.ComponentModel;
using System.Windows;

namespace kokos.WPF.ViewModel.Base
{
    public static class Utils
    {
        public static bool IsInDesignMode<T>(this T viewMode) where T : ReactiveObject
        {
            var propertyDescriptor = DependencyPropertyDescriptor.FromProperty(
                DesignerProperties.IsInDesignModeProperty,
                typeof (FrameworkElement));

            return (bool) propertyDescriptor.Metadata.DefaultValue;
        }
    }
}

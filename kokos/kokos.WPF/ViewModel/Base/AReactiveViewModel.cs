﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace kokos.WPF.ViewModel.Base
{
    public abstract class AReactiveViewModel : INotifyPropertyChanged
    {
        private readonly Dictionary<string, object> _store = new Dictionary<string, object>();

        protected static bool IsInDesignMode
        {
            get
            {
                var propertyDescriptor = DependencyPropertyDescriptor.FromProperty(
                    DesignerProperties.IsInDesignModeProperty, typeof(FrameworkElement));

                return (bool)propertyDescriptor.Metadata.DefaultValue;
            } 
        }

        protected T GetValue<T>([CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException(propertyName);

            object val;
            if (_store.TryGetValue(propertyName, out val))
                return (T)val;

            return default(T);
        }

        protected void SetValue<T>(T newValue, [CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException(propertyName);

            var oldValue = GetValue<T>(propertyName);
            if (Equals(newValue, oldValue))
                return;

            _store[propertyName] = newValue;
            RaisePropertyChanged(propertyName);
        }

        protected void RaisePropertyChanged(string propertyName)
        {
            var propChanged = PropertyChanged;
            if (propChanged != null)
                propChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

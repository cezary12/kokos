﻿using System.Security;
using System.Windows;
using System.Windows.Controls;
using kokos.Communication.Extensions;
using MahApps.Metro.Controls;

namespace kokos.WPF.AttachedProperties
{
    /// <summary>
    /// http://blog.functionalfun.net/2008/06/wpf-passwordbox-and-data-binding.html
    /// </summary>
    public static class PasswordBoxAssistant
    {
        public static readonly DependencyProperty BoundPassword =
            DependencyProperty.RegisterAttached("BoundPassword", typeof (SecureString), typeof (PasswordBoxAssistant),
                new PropertyMetadata(string.Empty.ToSecureString(), OnBoundPasswordChanged));

        public static readonly DependencyProperty BindPassword =
            DependencyProperty.RegisterAttached("BindPassword", typeof (bool), typeof (PasswordBoxAssistant),
                new PropertyMetadata(false, OnBindPasswordChanged));

        private static readonly DependencyProperty UpdatingPassword =
            DependencyProperty.RegisterAttached("UpdatingPassword", typeof (bool), typeof (PasswordBoxAssistant),
                new PropertyMetadata(false));

        private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = d as PasswordBox;

            // only handle this event when the property is attached to a PasswordBox
            // and when the BindPassword attached property has been set to true
            if (passwordBox == null || !GetBindPassword(d))
            {
                return;
            }

            // avoid recursive updating by ignoring the box's changed event
            passwordBox.PasswordChanged -= HandlePasswordChanged;

            var newPasswordSecure = (SecureString) e.NewValue;
            var newPassword = newPasswordSecure.ToInsecureString();

            if (!GetUpdatingPassword(passwordBox))
            {
                passwordBox.Password = newPassword;
                if (!string.IsNullOrWhiteSpace(newPassword))
                    TextboxHelper.SetWatermark(passwordBox, "");
            }

            passwordBox.PasswordChanged += HandlePasswordChanged;
        }

        private static void OnBindPasswordChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            // when the BindPassword attached property is set on a PasswordBox,
            // start listening to its PasswordChanged event

            var passwordBox = dp as PasswordBox;

            if (passwordBox == null)
            {
                return;
            }

            var wasBound = (bool) (e.OldValue);
            var needToBind = (bool) (e.NewValue);

            if (wasBound)
            {
                passwordBox.PasswordChanged -= HandlePasswordChanged;
            }

            if (needToBind)
            {
                passwordBox.PasswordChanged += HandlePasswordChanged;
            }
        }

        private static void HandlePasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            if (passwordBox == null)
                return;

            // set a flag to indicate that we're updating the password
            SetUpdatingPassword(passwordBox, true);
            // push the new password into the BoundPassword property
            SetBoundPassword(passwordBox, passwordBox.Password.ToSecureString());
            SetUpdatingPassword(passwordBox, false);
        }

        public static void SetBindPassword(DependencyObject dp, bool value)
        {
            dp.SetValue(BindPassword, value);
        }

        public static bool GetBindPassword(DependencyObject dp)
        {
            return (bool) dp.GetValue(BindPassword);
        }

        public static SecureString GetBoundPassword(DependencyObject dp)
        {
            return (SecureString) dp.GetValue(BoundPassword);
        }

        public static void SetBoundPassword(DependencyObject dp, SecureString value)
        {
            dp.SetValue(BoundPassword, value);
        }

        private static bool GetUpdatingPassword(DependencyObject dp)
        {
            return (bool) dp.GetValue(UpdatingPassword);
        }

        private static void SetUpdatingPassword(DependencyObject dp, bool value)
        {
            dp.SetValue(UpdatingPassword, value);
        }
    }
}
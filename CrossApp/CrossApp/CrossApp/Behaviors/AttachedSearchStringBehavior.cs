using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace CrossApp.Behaviors
{
    public static class AttachedSearchStringBehavior
    {
        public static readonly BindableProperty CommandProperty =
            BindableProperty.CreateAttached(
                propertyName: "Command",
                returnType: typeof(ICommand),
                declaringType: typeof(SearchBar),
                defaultValue: null,
                defaultBindingMode: BindingMode.OneWay,
                validateValue: null,
                propertyChanged: OnSearchTapped);

        private static void OnSearchTapped(BindableObject bindable, object oldValue, object newValue)
            => (bindable as SearchBar).SearchButtonPressed += (sender, e) =>
            {
                var control = sender as SearchBar;
                var command = (ICommand)control.GetValue(CommandProperty);

                string asd = e.ToString();

                if (command != null && command.CanExecute(control.Text))
                    command.Execute(control.Text);

                control.Text = null;
            };
    }
}

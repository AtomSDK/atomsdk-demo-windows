using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Atom.VPN.Demo.UINotifiers
{
    public class UserControlBase : UserControl, INotifyPropertyChanged
    {
        public UserControlBase()
        {
            this.DataContext = this;
            this.IsVisibleChanged += OnVisiblityChanged;
            this.Loaded += UserControlBase_Loaded;
        }

        void UserControlBase_Loaded(object sender, System.Windows.RoutedEventArgs e) { OnViewLoaded((ContentControl)sender); }

        protected virtual void OnViewLoaded(ContentControl view) { }

        void OnVisiblityChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
                this.EnteredTime = DateTime.Now;
            else if (EnteredTime != null && this.EnteredTime != DateTime.MinValue)
                this.TimeSpentHere += (DateTime.Now - this.EnteredTime).TotalSeconds;
        }

        public double TimeSpentHere = 0;
        private DateTime EnteredTime;

        private static MainWindow _ParentWindow;
        public MainWindow ParentWindow { get { return (_ParentWindow = _ParentWindow ?? Application.Current.MainWindow as MainWindow); } }


        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies subscribers of the property change.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="exp">The property expression.</param>
        protected virtual void NotifyOfPropertyChange<T>(Expression<Func<T>> exp)
        {
            try
            {
                string name = ((LambdaExpression)exp).Body.ToString().Split('.').Last();
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs(name));
                }
            }
            catch { }
        }

        /// <summary>
        /// Notifies subscribers of the property change.
        /// </summary>
        /// <param name="name">Name of the property.</param>
        protected virtual void NotifyOfPropertyChange(string name)
        {
            try
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs(name));
                }
            }
            catch { }
        }
    }
}

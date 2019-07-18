using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Jaze.UI.ViewModel;

namespace Jaze.UI.Views
{
    /// <summary>
    /// Interaction logic for SelectGroupView.xaml
    /// </summary>
    public partial class SelectGroupView : UserControl
    {
        public SelectGroupViewModel ViewModel => DataContext as SelectGroupViewModel;

        public SelectGroupView()
        {
            InitializeComponent();
        }
    }
}
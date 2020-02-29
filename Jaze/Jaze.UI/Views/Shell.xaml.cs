using System.Diagnostics;
using MahApps.Metro.Controls;

namespace Jaze.UI
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell : MetroWindow
    {
        /// <summary>
        /// Initializes a new instance of the Shell class.
        /// </summary>
        public Shell()
        {
            Debug.WriteLine("Shell");
            InitializeComponent();
        }
    }
}
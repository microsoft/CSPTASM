// -----------------------------------------------------------------------
// <copyright file="About.xaml.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace TASM
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="About"/> class.
        /// </summary>
        public About()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Load event for the window, loads the limitations and name of the tool.
        /// </summary>
        /// <param name="sender">the Sender object</param>
        /// <param name="e">the Event Args object</param>
        private void About1_Loaded(object sender, RoutedEventArgs e)
        {
            lblTASM.Content = Constants.NameoftheTool;
            txtbMessage.Text = Constants.ToolLimitations;
        }
    }
}

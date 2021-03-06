﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace BugShooting.Output.Bugzilla
{
  partial class Send : Window
  {

    Dictionary<string, Product> products;

    public Send(string url, 
                string lastProduct, 
                string lastComponent,
                string lastVersion,
                string lastOperatingSystem,
                string lastPlatform,
                string lastPriority,
                string lastSeverity,
                int lastBugID,
                Dictionary<string, Product> products,
                List<FieldValue> operatingSystemValues,
                List<FieldValue> platformValues,
                List<FieldValue> priorityValues,
                List<FieldValue> severityValues,
                string userName, 
                string password, 
                string fileName)
    {
      InitializeComponent();

      this.products = products;
      
      Url.Text = url;
      NewBug.IsChecked = true;

      ProductComboBox.ItemsSource = new SortedList<string, Product>(products).Values;
      ProductComboBox.SelectedValue = lastProduct;
      
      if (ProductComboBox.SelectedValue != null)
      {
        ComponentComboBox.SelectedValue = lastComponent;
        VersionComboBox.SelectedValue = lastVersion;
      }
      
      OperatingSystemComboBox.ItemsSource = operatingSystemValues;
      OperatingSystemComboBox.SelectedValue = lastOperatingSystem;

      PlatformComboBox.ItemsSource = platformValues;
      PlatformComboBox.SelectedValue = lastPlatform;

      PriorityComboBox.ItemsSource = priorityValues;
      PriorityComboBox.SelectedValue = lastPriority;

      SeverityComboBox.ItemsSource = severityValues;
      SeverityComboBox.SelectedValue = lastSeverity;

      BugIDTextBox.Text = lastBugID.ToString();
      FileNameTextBox.Text = fileName;

      ProductComboBox.SelectionChanged += ValidateData;
      ComponentComboBox.SelectionChanged += ValidateData;
      VersionComboBox.SelectionChanged += ValidateData;
      OperatingSystemComboBox.SelectionChanged += ValidateData;
      PlatformComboBox.SelectionChanged += ValidateData;
      PriorityComboBox.SelectionChanged += ValidateData;
      SeverityComboBox.SelectionChanged += ValidateData;
      SummaryTextBox.TextChanged += ValidateData;
      DescriptionTextBox.TextChanged += ValidateData;
      BugIDTextBox.TextChanged += ValidateData;
      FileNameTextBox.TextChanged += ValidateData;
      ValidateData(null, null);

    }

    public bool CreateNewBug
    {
      get { return NewBug.IsChecked.Value; }
    }
 
    public string Product
    {
      get { return (string)ProductComboBox.SelectedValue; }
    }

    public string Component
    {
      get { return (string)ComponentComboBox.SelectedValue; }
    }

    public string Version
    {
      get { return (string)VersionComboBox.SelectedValue; }
    }

    public string OperatingSystem
    {
      get { return (string)OperatingSystemComboBox.SelectedValue; }
    }

    public string Platform
    {
      get { return (string)PlatformComboBox.SelectedValue; }
    }

    public string Priority
    {
      get { return (string)PriorityComboBox.SelectedValue; }
    }

    public string Severity
    {
      get { return (string)SeverityComboBox.SelectedValue; }
    }

    public string Summary
    {
      get { return SummaryTextBox.Text; }
    }

    public string Description
    {
      get { return DescriptionTextBox.Text; }
    }

    public int BugID
    {
      get { return Convert.ToInt32(BugIDTextBox.Text); }
    }

    public string Comment
    {
      get { return CommentTextBox.Text; }
    }

    public string FileName
    {
      get { return FileNameTextBox.Text; }
    }

    private void NewBug_CheckedChanged(object sender, EventArgs e)
    {

      if (NewBug.IsChecked.Value)
      {
        NewBugControls.Visibility = Visibility.Visible;
        AttachToBugControls.Visibility = Visibility.Collapsed;

        SummaryTextBox.SelectAll();
        SummaryTextBox.Focus();
      }
      else
      {
        NewBugControls.Visibility = Visibility.Collapsed;
        AttachToBugControls.Visibility = Visibility.Visible;
        
        BugIDTextBox.SelectAll();
        BugIDTextBox.Focus();
      }

      ValidateData(null, null);

    }

    private void BugID_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
    }

    private void ProductComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

      if (ProductComboBox.SelectedItem is null)
      {
        ComponentComboBox.ItemsSource = null;
        VersionComboBox.ItemsSource = null;
      }
      else
      {

        string product = ((Product)ProductComboBox.SelectedItem).Name;

        ComponentComboBox.ItemsSource = products[product].Components;
        ComponentComboBox.SelectedIndex = 0;

        VersionComboBox.ItemsSource = products[product].Versions;
        VersionComboBox.SelectedIndex = 0;

      }
      ValidateData(null, null);

    }

    private void ValidateData(object sender, EventArgs e)
    {
      OK.IsEnabled = ((CreateNewBug && Validation.IsValid(ProductComboBox) &&
                                       Validation.IsValid(ComponentComboBox) &&
                                       Validation.IsValid(VersionComboBox) &&
                                       Validation.IsValid(OperatingSystemComboBox) &&
                                       Validation.IsValid(PlatformComboBox) &&
                                       Validation.IsValid(PriorityComboBox) &&
                                       Validation.IsValid(SeverityComboBox) &&
                                       Validation.IsValid(SummaryTextBox) && 
                                       Validation.IsValid(DescriptionTextBox)) ||
                      (!CreateNewBug && Validation.IsValid(BugIDTextBox))) &&
                     Validation.IsValid(FileNameTextBox);
    }

    private void OK_Click(object sender, RoutedEventArgs e)
    {
      this.DialogResult = true;
    }

  }

}

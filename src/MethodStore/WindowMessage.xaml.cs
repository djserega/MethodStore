﻿using MahApps.Metro.Controls;
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
using System.Windows.Shapes;

namespace MethodStore
{
    /// <summary>
    /// Логика взаимодействия для WindowMessage.xaml
    /// </summary>
    public partial class WindowMessage : MetroWindow
    {

        #region Fields

        private string _textMessage;
        private bool _question;
        private int _timer;
        private string _textButtonOK;
        private string _textButtonCancel;

        #endregion

        #region Properties

        internal bool VisibleCancelCommand { get; private set; }

        internal bool PressButtonOK { get; private set; }
        internal bool PressButtonCancel { get; private set; }
        internal bool ClosedByTimeout { get; private set; }

        #endregion

        #region Constructors

        public WindowMessage()
        {
            InitializeComponent();
        }

        public WindowMessage(string textMessage, bool question = false)
        {
            InitializeComponent();

            _textMessage = textMessage;

            _question = question;
            PrepareForm();
        }

        public WindowMessage(string textMessage, int timer, bool question = false)
        {
            InitializeComponent();

            _textMessage = textMessage;
            _timer = timer;

            _question = question;
            PrepareForm();
        }

        #endregion


        private void FormMessage_Loaded(object sender, RoutedEventArgs e)
        {
            TextBlockMessage.Text = _textMessage;

            _textButtonOK = ButtonOK.Content.ToString();
            _textButtonCancel = ButtonCancel.Content.ToString();


            if (_timer > 0)
                StartTimerAsync();
        }

        #region Button

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            PressButtonOK = true;
            Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            PressButtonCancel = true;
            Close();
        }

        #endregion

        private async void StartTimerAsync()
        {
            if (_question)
                ButtonCancel.Content = _textButtonCancel + $" ({_timer} с.)";
            else
                ButtonOK.Content = _textButtonOK + $" ({_timer} с.)";

            _timer--;

            if (_timer >= 0)
            {
                await Timers.StartTimerPause();
                StartTimerAsync();
            }
            else if (_timer < 0)
            {
                ClosedByTimeout = true;
                Close();
            }
        }

        private void PrepareForm()
        {
            if (_question)
            {
                Title = "Вопрос";
                VisibleCancelCommand = true;
            }
        }

    }
}

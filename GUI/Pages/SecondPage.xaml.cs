using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace Simulation
{
    public partial class SecondPage : ContentPage
    {
        private int _rows, _cols;
        private Assembly _assembly;
        public SecondPage(Assembly asm)
        {
            _assembly = asm;
            InitializeComponent();
        }

        private void OnBttnSetDataClicked(object sender, EventArgs args)
        {
             SetRowsAndCols();
        }

        public void SetRowsAndCols()
        {
            if (FirstEntry.Text == null || SecondEntry.Text == null)
                 DisplayAlert("Warning", "Вы не ввели значение", "Ок");
            else if (FirstEntry.Text.Contains('-') || SecondEntry.Text.Contains('-'))
                 DisplayAlert("Warning", "Введите значения больше 0", "Ок");
            else if (FirstEntry.Text.Any(c => !char.IsDigit(c)) || SecondEntry.Text.Any(c => !char.IsDigit(c)))
                 DisplayAlert("Warning", "Для ввода доступны только цифры", "Ок");
            else if (FirstEntry.Text == "0" || SecondEntry.Text == "0")
                 DisplayAlert("Warning", "Сторона не может равняться нулю", "Ок");
            else
            {
                _rows = int.Parse(FirstEntry.Text);
                _cols = int.Parse(SecondEntry.Text);
                Navigation.PushAsync(new ThirdPage(_assembly, _rows, _cols));
            }
        }
    }
}

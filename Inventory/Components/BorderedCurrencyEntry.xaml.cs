using System.Globalization;
using System.Text.RegularExpressions;

namespace Inventory.Components;

public partial class BorderedCurrencyEntry : ContentView
{
    public BorderedCurrencyEntry()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty TextProperty = BindableProperty.Create(
    propertyName: nameof(Text), returnType: typeof(string), declaringType: typeof(BorderedEntry), defaultValue: null, defaultBindingMode: BindingMode.TwoWay);

    [GeneratedRegex("\\D")]
    private static partial Regex OnlyDigits();

    public string Text
    {
        get { return (string)GetValue(TextProperty); }
        set
        {
            SetValue(TextProperty, value);
        }
    }

    public static readonly BindableProperty LabelTextProperty = BindableProperty.Create(
           propertyName: nameof(LabelText), returnType: typeof(string), declaringType: typeof(BorderedEntry), defaultValue: null, defaultBindingMode: BindingMode.OneWay);

    public string LabelText { get => (string)GetValue(LabelTextProperty); set { SetValue(LabelTextProperty, value); } }

    private void EntryCurrency_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (this.EntryCurrency != null)
        {
            string valueFromString = OnlyDigits().Replace(this.EntryCurrency.Text.ToString(), "");
            string _value;

            if (valueFromString.Length <= 0)
            { _value = "0"; }
            else
            {
                if (string.IsNullOrEmpty(valueFromString)) _value = "0";
                else
                {
                    if (Convert.ToDecimal(valueFromString) <= 0)
                    {
                        _value = "0";
                    }
                    else
                    {

                        var decValue = (Convert.ToDecimal(valueFromString) / 100m);
                        var currencyFormatValue = decValue.ToString("N2", new CultureInfo("pt-BR", false));

                        _value = currencyFormatValue;
                    }
                }
            }
            this.EntryCurrency.Text = _value;
            this.EntryCurrency.CursorPosition = this.EntryCurrency.Text?.Length ?? 0;
        }
    }
}
namespace Bilong_Georges_Patrice;

public partial class MainPage : ContentPage
{
    private string _currentInput = "0";
    private double _firstOperand = 0;
    private string _currentOperator = "";
    private bool _isNewInput = true;
    private string _operationDisplay = "";

    public MainPage()
    {
        InitializeComponent();
    }

    private void OnNumber(object sender, EventArgs e)
    {
        var btn = (Button)sender;
        string digit = btn.CommandParameter?.ToString() ?? "";

        if (_isNewInput)
        {
            _currentInput = digit;
            _isNewInput = false;
        }
        else
        {
            if (_currentInput == "0")
                _currentInput = digit;
            else
                _currentInput += digit;
        }
        UpdateDisplay();
    }

    private void OnDecimal(object sender, EventArgs e)
    {
        if (_isNewInput)
        {
            _currentInput = "0.";
            _isNewInput = false;
        }
        else if (!_currentInput.Contains("."))
        {
            _currentInput += ".";
        }
        UpdateDisplay();
    }

    private void OnOperator(object sender, EventArgs e)
    {
        var btn = (Button)sender;
        string op = btn.CommandParameter?.ToString() ?? "";

        if (!string.IsNullOrEmpty(_currentOperator) && !_isNewInput)
        {
            Calculate();
        }

        if (double.TryParse(_currentInput, out double val))
        {
            _firstOperand = val;
        }

        _currentOperator = op;
        _operationDisplay = $"{_firstOperand} {GetOperatorSymbol(op)}";
        _isNewInput = true;
        UpdateDisplay();
    }

    private void OnEquals(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(_currentOperator)) return;
        Calculate();
        _currentOperator = "";
        _operationDisplay = "";
        _isNewInput = true;
        UpdateDisplay();
    }

    private void Calculate()
    {
        if (!double.TryParse(_currentInput, out double secondOperand)) return;

        double result = 0;
        try
        {
            switch (_currentOperator)
            {
                case "+": result = _firstOperand + secondOperand; break;
                case "-": result = _firstOperand - secondOperand; break;
                case "*": result = _firstOperand * secondOperand; break;
                case "/":
                    if (secondOperand == 0)
                    {
                        ResultLabel.Text = "Erreur : division par zéro";
                        _currentInput = "0";
                        _operationDisplay = "";
                        _currentOperator = "";
                        return;
                    }
                    result = _firstOperand / secondOperand;
                    break;
            }

            _currentInput = result.ToString();
            _firstOperand = result;
        }
        catch
        {
            ResultLabel.Text = "Erreur";
            _currentInput = "0";
        }
    }

    private void OnClear(object sender, EventArgs e)
    {
        _currentInput = "0";
        _firstOperand = 0;
        _currentOperator = "";
        _operationDisplay = "";
        _isNewInput = true;
        UpdateDisplay();
    }

    private void OnClearEntry(object sender, EventArgs e)
    {
        if (_currentInput.Length > 1)
            _currentInput = _currentInput.Substring(0, _currentInput.Length - 1);
        else
            _currentInput = "0";
        UpdateDisplay();
    }

    private void OnSign(object sender, EventArgs e)
    {
        if (_currentInput.StartsWith("-"))
            _currentInput = _currentInput.Substring(1);
        else if (_currentInput != "0")
            _currentInput = "-" + _currentInput;
        UpdateDisplay();
    }

    private void OnPercent(object sender, EventArgs e)
    {
        if (double.TryParse(_currentInput, out double val))
        {
            _currentInput = (val / 100).ToString();
            UpdateDisplay();
        }
    }

    private void UpdateDisplay()
    {
        OperationLabel.Text = _operationDisplay;
        ResultLabel.Text = _currentInput;
    }

    private string GetOperatorSymbol(string op) => op switch
    {
        "+" => "+",
        "-" => "−",
        "*" => "×",
        "/" => "÷",
        _ => op
    };
}
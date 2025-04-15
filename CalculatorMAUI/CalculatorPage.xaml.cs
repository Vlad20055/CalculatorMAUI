namespace CalculatorMAUI;

public partial class CalculatorPage : ContentPage
{
    enum State
    {
        NotModified,
        Modified
    }

    private string bin_operation = ""; 
    private double n1 = 0;
    private double n2 = 0;
    private State state = State.NotModified;
    private bool equal_pushed = true;

	public CalculatorPage()
	{
		InitializeComponent();
	}

    private bool is_dot_pushed()
    {
        string now = this.result.Text;
        foreach (var c in now)
        {
            if (c == '.') return true;
        }
        return false;
    }

	void OnNumberSelection(object sender, EventArgs e)
	{

        Button button = (Button)sender;

        if (state == State.NotModified)
        {
            this.result.Text = button.Text;
            state = State.Modified;
            return;
        }

        if (this.result.Text == "NaN" || this.result.Text == "∞") return;

        if (double.Parse(this.result.Text) == 0)
        {
            if (is_dot_pushed())
            {
                this.result.Text += button.Text;
                return;
            }
            else
            {
                this.result.Text = button.Text;
                return;
            }
        }

        this.result.Text += button.Text;
        return;
    }

    private void OnUnarnyOperator(object sender, EventArgs e)
    {
        n2 = double.Parse(this.result.Text);
        Button button = (Button)sender;

        switch (button.Text)
        {
            case "1/x":
                n2 = 1.0 / n2;
                break;
            case "x²":
                n2 *= n2;
                break;
            case "√x":
                n2 = Math.Sqrt(n2);
                break;
            case "sin(x)":

                if (n2 >= 0) n2 %= 360;
                else
                {
                    n2 = -n2;
                    n2 %= 360;
                    n2 = -n2;
                    n2 += 360;
                }

                double radians = Math.PI * n2 / 180.0;
                n2 = Math.Sin(radians);
                break;
            case "±":
                n2 = -n2;
                break;
            default:
                throw new Exception("No unarny operator");
        }

        state = State.Modified;
        this.result.Text = n2.ToString();
        return;
    }

    private void OnCE(object sender, EventArgs e)
    {
        this.result.Text = "0";
        state = State.Modified;
        return;
    }

    private void OnC(object sender, EventArgs e)
    {
        this.result.Text = "0";
        n1 = 0;
        n2 = 0;
        state = State.NotModified;
        bin_operation = "";
        equal_pushed = true;
        return;
    }

    private void OnEqual(object sender, EventArgs e)
    {
        equal_pushed = true;

        if (bin_operation == "")
        {
            return;
        }

        if (state == State.Modified)
        {
            state = State.NotModified;
            n2 = double.Parse(this.result.Text);

            switch (bin_operation)
            {
                case "+":
                    n1 += n2;
                    break;
                case "-":
                    n1 -= n2;
                    break;
                case "×":
                    n1 *= n2;
                    break;
                case "÷":
                    n1 /= n2;
                    break;
                default:
                    throw new Exception("No bin_operation");
            }

            this.result.Text = n1.ToString();
            return;
        }
        else // state == State.NotModified
        {
            n2 = double.Parse(this.result.Text);

            switch (bin_operation)
            {
                case "+":
                    n1 += n2;
                    break;
                case "-":
                    n1 -= n2;
                    break;
                case "×":
                    n1 *= n2;
                    break;
                case "÷":
                    n1 /= n2;
                    break;
                default:
                    throw new Exception("No bin_operation");
            }

            this.result.Text = n1.ToString();
            return;
        }
    }

    private void OnBinaryOperator(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        string now_operation = button.Text;

        if (bin_operation == "")
        {
            state = State.NotModified;
            equal_pushed = false;
            n1 = double.Parse(this.result.Text);
        }

        if (state == State.NotModified)
        {
            bin_operation = now_operation;
            return;
        }

        if (equal_pushed)
        {
            bin_operation = now_operation;
            equal_pushed = false;
            state = State.NotModified;
            n1 = double.Parse(this.result.Text);
            return;
        }

        equal_pushed = false;

        state = State.NotModified;
        n2 = double.Parse(this.result.Text);
        
        switch (bin_operation)
        {
            case "+":
                n1 += n2;
                break;
            case "-":
                n1 -= n2;
                break;
            case "×":
                n1 *= n2;
                break;
            case "÷":
                n1 /= n2;
                break;
            default:
                throw new Exception("No bin_operation");
        }

        bin_operation = now_operation;
        this.result.Text = n1.ToString();
    }

    private void OnBackspace(object sender, EventArgs e)
    {
        if (state == State.NotModified) return;

        if (this.result.Text == "NaN" || this.result.Text.Length == 1)
        {
            this.result.Text = "0";
            return;
        }

        this.result.Text = this.result.Text.Substring(0, this.result.Text.Length - 1);
        return;
    }

    private void OnDot(object sender, EventArgs e)
    {
        if (is_dot_pushed() ||
            (bin_operation != "" && state == State.NotModified) ||
            this.result.Text == "NaN" ||
            this.result.Text == "∞")
        {
            return;
        }

        this.result.Text += ".";
        return;
    }
}
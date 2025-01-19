// Código dedicado meramente a las clases de la interfaz, el punto será utilizarlo en el cliente.

namespace Proyecto3 {
    public class CalculatorForm : Form
    {
        private readonly Client client;
        private TextBox expressionInput;
        private Button calculateButton;
        private RadioButton mathModeButton;
        private RadioButton logicModeButton;
        private Label resultLabel;
        private ComboBox separatorOptions;

        private string result = "";

        public CalculatorForm(Client clientInstance)
        {
            client = clientInstance;
            // Configuración básica de la ventana
            Text = "Calculadora de Árboles de Expresión";
            Size = new Size(400, 300);

            // Entrada de la expresión
            Label expressionLabel = new Label();
            expressionLabel.Text = "Expresión:";
            expressionLabel.Location = new Point(20, 20);
            Controls.Add(expressionLabel);

            expressionInput = new TextBox();
            expressionInput.Location = new Point(100, 20);
            expressionInput.Width = 250;
            Controls.Add(expressionInput);

            // Botón para calcular
            calculateButton = new Button();
            calculateButton.Text = "Calcular";
            calculateButton.Location = new Point(20, 60);
            calculateButton.Click += CalculateButton_Click;
            Controls.Add(calculateButton);

            // Opciones de modo
            mathModeButton = new RadioButton();
            mathModeButton.Text = "Modo Matemático";
            mathModeButton.Location = new Point(20, 100);
            mathModeButton.Checked = true;
            Controls.Add(mathModeButton);

            logicModeButton = new RadioButton();
            logicModeButton.Text = "Modo Lógico";
            logicModeButton.Location = new Point(20, 130);
            Controls.Add(logicModeButton);

            // Opciones de separador
            Label separatorLabel = new Label();
            separatorLabel.Text = "Separador:";
            separatorLabel.Location = new Point(20, 170);
            Controls.Add(separatorLabel);

            separatorOptions = new ComboBox();
            separatorOptions.Items.AddRange(["Espacio", "Coma", "Punto y coma"]);
            separatorOptions.SelectedIndex = 0;
            separatorOptions.Location = new Point(100, 170);
            Controls.Add(separatorOptions);

            // Resultado
            resultLabel = new Label();
            resultLabel.Text = "Resultado: ";
            resultLabel.Location = new Point(20, 210);
            resultLabel.AutoSize = true;
            Controls.Add(resultLabel);
        }
        
        public string GetExpressionInput() {
            return expressionInput.Text;
        }

        public void SetResult(string data) {
            result = data;
        }

        private void CalculateButton_Click(object sender, EventArgs e)
        {
            try {
                string input = expressionInput.Text;

                if (string.IsNullOrWhiteSpace(input))
                {
                    MessageBox.Show("Ingrese una expresión válida.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // Enviar datos al servidor a través del cliente
                string response = client.SendExpression(input);
                
                // Mostrar resultado
                resultLabel.Text = $"Resultado: {response}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
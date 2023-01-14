using System.Drawing;
using System.Windows.Forms;

namespace Game2048
{
    class Score : Panel
    {
        public Score(string title, int initialValue = 0)
        {
            BackColor = Color.FromArgb(188, 174, 159);
            Width = WidthValue;
            Height = HeightValue;

            _Value = initialValue;

            _TitleLabel = new Label()
            {
                Text = title,
                Font = new Font("Arial", 16, FontStyle.Bold),
                Size = new Size(WidthValue, HeightValue / 2),
                Location = new Point(0, 0),
                ForeColor = Color.FromArgb(100, Color.FromArgb(255, 246, 230)),
                TextAlign = ContentAlignment.MiddleCenter
            };

            _ValueLabel = new Label()
            {
                Text = $"{_Value}",
                Font = new Font("Arial", 20, FontStyle.Bold),
                Size = new Size(WidthValue, HeightValue / 2),
                Location = new Point(0, HeightValue / 2),
                ForeColor = Color.FromArgb(255, 246, 230),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Controls.Add(_TitleLabel);
            Controls.Add(_ValueLabel);
        }

   
        // встановлює рахунок
  
        // <param name="value">Значення рахунку.</param>
        public void SetValue(int value)
        {
            _Value = value;
            _ValueLabel.Text = $"{_Value}";
        }

   
        // збільшує рахунок на передане значення.
 
        // <param name="value">значення на яке потрібно встановлювати рахунок.</param>
        public void Increase(int value)
        {
            SetValue(_Value + value);
        }

        // скидає рахунок.
   
        public void Reset()
        {
            SetValue(0);
        }

        public int Value
        {
            get
            {
                return _Value;
            }
        }

        public readonly static int WidthValue = 95;

        public readonly static int HeightValue = 70;

        private Label _TitleLabel;

        private Label _ValueLabel;

        private int _Value;
    }
}
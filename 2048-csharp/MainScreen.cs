using System;
using System.Drawing;
using System.Windows.Forms;

namespace Game2048
{
    public class MainScreen : Form
    {
        public MainScreen()
        {
            this.Load += (sender, e) => MainScreen_Load(sender, e);
            this.KeyDown += new KeyEventHandler(Form_KeyDown);
        }

    
        // Обробник події завантаження форми.
    
        //name="sender">Об'єкт
        // name="e">Клас події
        private void MainScreen_Load(object sender, EventArgs e)
        {
            int fieldSize = Cell.SizeValue * Field.Dimension + Cell.MarginValue * (Field.Dimension + 1);

            int headerHeight = 70;

            int width = fieldSize + _PADDING * 2;
            int height = fieldSize + _PADDING * 3 + headerHeight;
            int x = Screen.PrimaryScreen.Bounds.Width / 2 - width / 2;
            int y = Screen.PrimaryScreen.Bounds.Height / 2 - height / 2;

            Name = "2048";
            Text = "2048";
            MaximizeBox = false;
            ClientSize = new Size(width, height);
            Location = new Point(x, y);
            BackColor = Color.FromArgb(251, 249, 239);

            Panel header = new Panel()
            {
                Location = new Point(_PADDING, _PADDING),
                Width = width - _PADDING * 2,
                Height = headerHeight
            };

            _BestScore = new Score("Score", _Storage.ReadBestScore())
            {
                Location = new Point(header.Width - Score.WidthValue, 0)
            };

            _CurrentScore = new Score("Check")
            {
                Location = new Point(header.Width - Score.WidthValue * 2 - _PADDING, 0)
            };

            header.Controls.Add(_BestScore);
            header.Controls.Add(_CurrentScore);

            _Field = new Field()
            {
                Location = new Point(_PADDING, _PADDING * 2 + header.Height),
                Size = new Size(fieldSize, fieldSize)
            };

            Controls.Add(header);
            Controls.Add(_Field);

            _Field.AddRandomItem();
            _Field.UpdateUI();
        }

        // Обробник події натискання клавіші

        // name="sender">Об'єкт
        // name="e">Клас події
        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            bool isMove = false;
            int score = 0;

            switch (e.KeyCode)
            {
                case Keys.Up:
                    isMove = _Field.ChangeByDirection(EDirection.UP, out score);
                    break;
                case Keys.Right:
                    isMove = _Field.ChangeByDirection(EDirection.RIGHT, out score);
                    break;
                case Keys.Down:
                    isMove = _Field.ChangeByDirection(EDirection.DOWN, out score);
                    break;
                case Keys.Left:
                    isMove = _Field.ChangeByDirection(EDirection.LEFT, out score);
                    break;
            }

            _CurrentScore.Increase(score);
            if (_CurrentScore.Value > _BestScore.Value)
            {
                _BestScore.SetValue(_CurrentScore.Value);
                _Storage.WriteBestScore(_BestScore.Value);
            }

            if (isMove)
            {
                _Field.AddRandomItem();
            }
            _Field.UpdateUI();
            if (_Field.isGameOver())
            {
                MessageBox.Show("YOU LOSE!");
                ResetState();
            }
        }

 
        // скидає компоненти гри

        private void ResetState()
        {
            _CurrentScore.Reset();
            _Field.Reset();
            _Field.AddRandomItem();
            _Field.UpdateUI();
        }

        private readonly Storage _Storage = new Storage();

        private const int _PADDING = 25;

        private Field _Field;

        private Score _CurrentScore;

        private Score _BestScore;
    }
}
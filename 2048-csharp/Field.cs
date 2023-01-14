using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Game2048
{
    class Field : Panel
    {
        public Field()
        {
            BackColor = Color.FromArgb(188, 174, 159);

            _Field = new int[Dimension, Dimension];
            _Field.Initialize();

            _Cells = new Cell[Dimension, Dimension];
            for (int i = 0; i < _Cells.GetLength(0); i++)
            {
                for (int j = 0; j < _Cells.GetLength(1); j++)
                {
                    int x = j * Cell.SizeValue + (j + 1) * Cell.MarginValue;
                    int y = i * Cell.SizeValue + (i + 1) * Cell.MarginValue;
                    _Cells[i, j] = new Cell(x, y);
                    Controls.Add(_Cells[i, j]);
                }
            }

            _CellBackColors = new Dictionary<int, CellColor>()
            {
                {0, new CellColor(Color.FromArgb(121, 112, 99), Color.FromArgb(216, 206, 196))},
                {2, new CellColor(Color.FromArgb(121, 112, 99), Color.FromArgb(240, 228, 217))},
                {4, new CellColor(Color.FromArgb(121, 112, 99), Color.FromArgb(238, 225, 199))},
                {8, new CellColor(Color.FromArgb(255, 246, 230), Color.FromArgb(253, 175, 112))},
                {16, new CellColor(Color.FromArgb(255, 246, 230), Color.FromArgb(255, 143, 86))},
                {32, new CellColor(Color.FromArgb(255, 246, 230), Color.FromArgb(255, 112, 80))},
                {64, new CellColor(Color.FromArgb(255, 246, 230), Color.FromArgb(255, 70, 18))},
                {128, new CellColor(Color.FromArgb(255, 246, 230), Color.FromArgb(241, 210, 104))},
                {256, new CellColor(Color.FromArgb(255, 246, 230), Color.FromArgb(241, 208, 86))},
                {512, new CellColor(Color.FromArgb(255, 246, 230), Color.FromArgb(240, 203, 65))},
                {1024, new CellColor(Color.FromArgb(255, 246, 230), Color.FromArgb(242, 201, 39))},
                {2048, new CellColor(Color.FromArgb(255, 246, 230), Color.FromArgb(243, 197, 0))},
                {4096, new CellColor(Color.FromArgb(255, 246, 230), Color.FromArgb(255, 80, 94))},
                {8192, new CellColor(Color.FromArgb(255, 246, 230), Color.FromArgb(255, 34, 75))},
                {16384, new CellColor(Color.FromArgb(255, 246, 230), Color.FromArgb(248, 19, 30))},
                {32768, new CellColor(Color.FromArgb(255, 246, 230), Color.FromArgb(96, 178, 219))},
                {65536, new CellColor(Color.FromArgb(255, 246, 230), Color.FromArgb(83, 154, 229))},
                {131072, new CellColor(Color.FromArgb(255, 246, 230), Color.FromArgb(0, 118, 193))}
            };
        }

   
        // Обновляє графічний інтерфейс поля  _Field.
  
        public void UpdateUI()
        {
            for (int i = 0; i < _Field.GetLength(0); i++)
            {
                for (int j = 0; j < _Field.GetLength(1); j++)
                {
                    _Cells[i, j].Text = (_Field[i, j] == 0) ? "" : $"{_Field[i, j]}";
                    _Cells[i, j].Style = _CellBackColors[_Field[i, j]];
                }
            }
        }

    
        //  Приводить стан гри до початкового

        public void Reset()
        {
            for (int i = 0; i < _Field.GetLength(0); i++)
            {
                for (int j = 0; j < _Field.GetLength(1); j++)
                {
                    _Field[i, j] = 0;
                }
            }
            UpdateUI();
        }

  
        // Рандомить в випадкову вільну ячейку <c>_Field</c> нове значення.
  

        //  90% нове значення – 2.
        // 10% нове значення – 4.

        public void AddRandomItem()
        {
            Random rnd = new Random();
            List<Point> emptyCells = new List<Point>();
            int value = (rnd.Next(1, 10) == 10) ? 4 : 2;

            for (int i = 0; i < _Field.GetLength(0); i++)
            {
                for (int j = 0; j < _Field.GetLength(1); j++)
                {
                    if (_Field[i, j] == 0)
                    {
                        emptyCells.Add(new Point(j, i));
                    }
                }
            }

            Point randomCoord = emptyCells[rnd.Next(emptyCells.Count)];
            _Field[randomCoord.Y, randomCoord.X] = value;
        }



        // <c>true</c>, якщо хоч одне значення всередині матриці <c>_Field</c> перемістилося, <c>false</c> інакше

        public bool ChangeByDirection(EDirection direction, out int score)
        {
            return ChangeStateByDirection(direction, ref _Field, out score);
        }

 
        // Перевіряє, чи можна зробити хід у будь-якому з напрямків.
  
        /// <returns><c>true</c>, якщо не можна зробити хід, <c>false</c> інакше.</returns>
        public bool isGameOver()
        {
            int[,] fieldClone = (int[,])_Field.Clone();
            // Заглушка
            int score;

            return !(
                ChangeStateByDirection(EDirection.UP, ref fieldClone, out score) ||
                ChangeStateByDirection(EDirection.RIGHT, ref fieldClone, out score) ||
                ChangeStateByDirection(EDirection.DOWN, ref fieldClone, out score) ||
                ChangeStateByDirection(EDirection.LEFT, ref fieldClone, out score)
            );
        }


        // змінює стан поля.

        // <returns><c>true</c>, хоча б один  перемістився, <c>false</c> інакше.</returns>

        private bool ChangeStateByDirection(EDirection direction, ref int[,] field, out int score)
        {
            bool isMove = false;
            int last1 = _Field.GetUpperBound(0);
            int last2 = _Field.GetUpperBound(1);

            switch (direction)
            {
                case EDirection.UP:
                    isMove = MoveValues(0, last2, 0, last1, true, ref field, out score);
                    break;
                case EDirection.RIGHT:
                    isMove = MoveValues(0, last1, last2, 0, false, ref field, out score);
                    break;
                case EDirection.DOWN:
                    isMove = MoveValues(0, last2, last1, 0, true, ref field, out score);
                    break;
                case EDirection.LEFT:
                    isMove = MoveValues(0, last1, 0, last2, false, ref field, out score);
                    break;
                default:
                    score = 0;
                    break;
            }

            return isMove;
        }


        // Переміщає елементи всередині стану
        // name="from1">Початковий індекс зовнішнього циклу
        //name="to1">Кінцевий індекс зовнішнього циклу
        // name="from2">Початковий індекс внутрішнього циклу
        // name="to2">Кінцевий індекс внутрішнього циклу
        // name="isVertical"><c>true</c>, якщо переміщення має бути по вертикалі.
        //name="field">Стан.
        //name="score">Рахунок.
        private bool MoveValues(
            int from1,
            int to1,
            int from2,
            int to2,
            bool isVertical,
            ref int[,] field,
            out int score)
        {
            bool isMove = false;
            Stack<int> stack = new Stack<int>();
            score = 0;

            for (
                int j = from1;
                (from1 < to1) ? j <= to1 : j >= to1;
                j = from1 < to1 ? j + 1 : j - 1)
            {
                for (
                    int i = from2, lastValue = -1;
                    (from2 < to2) ? i <= to2 : i >= to2;
                    i = from2 < to2 ? i + 1 : i - 1)
                {
                    int irow = isVertical ? i : j;
                    int icolumn = isVertical ? j : i;

                    int value = field[irow, icolumn];

                    if (value != 0)
                    {
                        bool isSameValues = stack.Count != 0 && stack.Peek() == value && lastValue == value;

                        if (isSameValues)
                        {
                            int next = GetNextValue(stack.Pop());
                            score += next;
                            stack.Push(next);
                        }
                        else
                        {
                            stack.Push(value);
                            lastValue = value;
                        }

                    }
                }

                // Переворот стека
                stack = new Stack<int>(stack);

                for (
                    int i = from2;
                    (from2 < to2) ? i <= to2 : i >= to2;
                    i = (from2 < to2) ? i + 1 : i - 1)
                {
                    int irow = isVertical ? i : j;
                    int icolumn = isVertical ? j : i;

                    if (stack.Count != 0 && stack.Peek() != field[irow, icolumn])
                    {
                        isMove = true;
                    }

                    field[irow, icolumn] = (stack.Count != 0) ? stack.Pop() : 0;
                }
            }

            return isMove;
        }

        // Отримує значення наступного ступеня на підставі два.

        // Значення 2 (n + 1).
        // name="value">Значення 2^n.
        private int GetNextValue(int value)
        {
            int i;
            for (i = -1; value != 0; i++)
            {
                value >>= 1;
            }
            int log2 = (i == -1) ? 0 : i;

            return (int)Math.Pow(2, log2 + 1);
        }

        public readonly static int Dimension = 4;

        private readonly Dictionary<int, CellColor> _CellBackColors;

        private Cell[,] _Cells;

        private int[,] _Field;
    }
}
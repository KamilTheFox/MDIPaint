using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MDIPaint
{
    public class FixedTextToValue<T> where T : struct
    {

        private static readonly Dictionary<Type, Delegate> ParserMap = new Dictionary<Type, Delegate>
        {
            { typeof(sbyte), new Func<string, (bool, sbyte)>(s => { var success = sbyte.TryParse(s, out var result); return (success, result); }) },
            { typeof(short), new Func<string, (bool, short)>(s => { var success = short.TryParse(s, out var result); return (success, result); }) },
            { typeof(int), new Func<string, (bool, int)>(s => { var success = int.TryParse(s, out var result); return (success, result); }) },
            { typeof(long), new Func<string, (bool, long)>(s => { var success = long.TryParse(s, out var result); return (success, result); }) },

            { typeof(byte), new Func<string, (bool, byte)>(s => { var success = byte.TryParse(s, out var result); return (success, result); }) },
            { typeof(ushort), new Func<string, (bool, ushort)>(s => { var success = ushort.TryParse(s, out var result); return (success, result); }) },
            { typeof(uint), new Func<string, (bool, uint)>(s => { var success = uint.TryParse(s, out var result); return (success, result); }) },
            { typeof(ulong), new Func<string, (bool, ulong)>(s => { var success = ulong.TryParse(s, out var result); return (success, result); }) },

            { typeof(float), new Func<string, (bool, float)>(s => { var success = float.TryParse(s, out var result); return (success, result); }) },
            { typeof(double), new Func<string, (bool, double)>(s => { var success = double.TryParse(s, out var result); return (success, result); }) },
            { typeof(decimal), new Func<string, (bool, decimal)>(s => { var success = decimal.TryParse(s, out var result); return (success, result); }) },
        };

        private T value;

        private dynamic objectControl;

        private HashSet<Type> whiteListType = new HashSet<Type>()
        {
            typeof(TextBox),
            typeof(ToolStripTextBox),
        };

        public T Value 
        {
            get => value;
            set
            {
                objectControl.Text = fixedText + ": " + value;
                this.value = value;
            }
        }

        public event Action<T> ChangeValue;

        private readonly string fixedText;

        public FixedTextToValue(string _fixedText, object box, T _default = default(T))
        {
            if (whiteListType.Contains(box.GetType()) == false)
            {
                throw new InvalidCastException(@"Тип не поддерживается.
Если он имеет события KeyDown, objectControl и свойство Text, до добавь его в HashSet белого списка");
            }

            fixedText = _fixedText;

            objectControl = box;

            objectControl.KeyDown += new KeyEventHandler(PressDown_Text);

            objectControl.TextChanged += new EventHandler(Change_Text);

            objectControl.Text = _fixedText + ": " + _default;
        }

        private void Change_Text(object sender, EventArgs key)
        {
            SelectText(sender);
        }

        private void PressDown_Text(object sender, KeyEventArgs key)
        {
            FixedTextHead(sender, key, fixedText);
        }

        private void SelectText(object sender)
        {

            StringBuilder sb = new StringBuilder();

            string[] textHead = objectControl.Text.Split(':');

            sb.Append(fixedText);
            sb.Append(":");

            string textSave = textHead[1];

            if (textSave.Length == 0)
            {
                sb.Append(" ");
                objectControl.SelectionStart = objectControl.Text.Length;
            }

            textSave.Trim();
            if (TryParse(textSave, out T i))
            {
                objectControl.ForeColor = Color.Black;
                value = i;
                ChangeValue?.Invoke(i);
            }
            else
            {
                objectControl.ForeColor = Color.Red;
            }
            sb.Append(textSave);

            objectControl.Text = sb.ToString();
        }
        private void FixedTextHead(object sender, KeyEventArgs e, string fixedText)
        {
            e.SuppressKeyPress = objectControl.SelectionStart < fixedText.Length + 2;
        }

        public static bool TryParse(string value, out T result)
        {
            result = default(T);

            if (!ParserMap.TryGetValue(typeof(T), out var parser))
                return false;

            var (success, parsed) = ((Func<string, (bool, T)>)parser)(value);
            if (success)
                result = parsed;

            return success;
        }

        
    }
}

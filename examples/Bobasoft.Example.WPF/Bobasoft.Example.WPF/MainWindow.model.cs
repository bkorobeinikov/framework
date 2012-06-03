using System.Collections.ObjectModel;
using Bobasoft.Presentation;
using Bobasoft.Presentation.MVVM;

namespace Bobasoft.Example.WPF
{
    public class MainWindowModel : WindowModel
    {
        public enum Enum1
        {
            One,
            Two,
        }

        private const string ValueText = "text";
        private const string ValueTextInput = "text.input";
        private const string ValueGridVisibility = "grid.isvisible";

        private const string ValueListSelectedIndex = "list.selectedindex";
        private const string EventListOnSelectionChanged = "list.onselectionchanged";
        private const string CommandChangeGridVisibility = "grid.do_changevisibility";

        public MainWindowModel()
        {
            List = new ObservableCollection<string>();

#if DEBUG
            if (ClientEnvironment.IsInDesignMode)
                InitializeDesignTime();
#endif
        }

        public ObservableCollection<string> List { get; set; }

        private Enum1 _enumValue;
        public Enum1 EnumValue
        {
            get { return _enumValue; }
            set
            {
                if (_enumValue != value)
                {
                    _enumValue = value;
                    RaisePropertyChanged("EnumValue");
                }
            }
        }

        protected override void Initialize()
        {
            InitializeCommands();
            InitializeEvents();
            InitializeValues();

            List.Add("item 1");
            List.Add("item 2");
            List.Add("item 3");
        }

        private void InitializeCommands()
        {
            var cmd = AddCommand(CommandChangeGridVisibility, GridDoChangeVisibility);
            cmd.Text = "Hide grid";
        }

        private void GridDoChangeVisibility()
        {
            var isVisible = GetValue<bool>(ValueGridVisibility);
            SetValue(ValueGridVisibility, !isVisible);

            var cmd = Commands[CommandChangeGridVisibility];
            cmd.Text = !isVisible ? "Hide grid" : "Show grid";
        }

        private void InitializeEvents()
        {
            AddEvent<int>(EventListOnSelectionChanged, ListOnSelectionChanged);
        }

        private void ListOnSelectionChanged(int selectedIndex)
        {
            SetValue(ValueListSelectedIndex, "selected index: {0}".With(selectedIndex));
        }

        private void InitializeValues()
        {
            SetValue(ValueText, "some text");
            NewOrClearValue(ValueTextInput);
            SetValue(ValueListSelectedIndex, "nothing selected");
            SetValue(ValueGridVisibility, true);
        }

#if DEBUG
        private void InitializeDesignTime()
        {
            SetValue(ValueText, "dummy text");
            SetValue(ValueListSelectedIndex, "dummy selected index: 1");
            SetValue(ValueTextInput, "dummy text input");

            List.Add("dummy item 1");
            List.Add("dummy item 2");
            List.Add("dummy item 3");

            var cmd = AddCommand(CommandChangeGridVisibility, () => { });
            cmd.Text = "dummy cmd text";
        }
#endif
    }
}
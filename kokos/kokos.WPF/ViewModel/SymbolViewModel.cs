using kokos.WPF.ViewModel.Base;

namespace kokos.WPF.ViewModel
{
    public class SymbolViewModel : AViewModel
    {
        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public double? Bid
        {
            get { return GetValue<double?>(); }
            set { SetValue(value); }
        }

        public double? Ask
        {
            get { return GetValue<double?>(); }
            set { SetValue(value); }
        }
    }
}

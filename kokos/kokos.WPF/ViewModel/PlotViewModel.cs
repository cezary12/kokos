using kokos.WPF.ViewModel.Base;
using OxyPlot;

namespace kokos.WPF.ViewModel
{
    public class PlotViewModel : AReactiveViewModel
    {
        public PlotModel Plot
        {
            get { return GetValue<PlotModel>(); }
            set { SetValue(value); }
        }

        public IPlotController PlotController
        {
            get { return GetValue<IPlotController>(); }
            set { SetValue(value); }
        }

        public PlotViewModel(PlotModel plot)
        {
            Plot = plot;
            PlotController = CreatePlotController();
        }

        private static PlotController CreatePlotController()
        {
            // create a new plot controller with default bindings
            var plotController = new PlotController();

            // add a tracker command to the mouse enter event
            plotController.BindMouseEnter(PlotCommands.HoverPointsOnlyTrack);

            return plotController;
        }
    }
}

using BiberDAMM.Models;

namespace BiberDAMM.ViewModels
{
    public enum TreatmentSeriesDeleteOptions
    {
        onlySelectedTreatment,
        selectedAndFutureTreatments,
        allTreatments,
    }

    // this viewModel is used to work with different delete options for a treatment [KrabsJ]
    public class DetailsTreatment
    {
        public Treatment Treatment { get; set; }
        public TreatmentSeriesDeleteOptions DeleteOption { get; set; }
    }
}
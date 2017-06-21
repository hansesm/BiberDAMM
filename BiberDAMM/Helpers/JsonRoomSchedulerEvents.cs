//Author: [HansesM]

namespace BiberDAMM.Helpers
{
    //Class for for displaying room-schedule in a timetable view, needed in the Room-Scheduler-View and Controllers [HansesM]
    public class JsonRoomSchedulerEvents
    {
        //java-script want lowercase variables. =( 
        public string roomName { get; set; }

        public string treatmentType { get; set; }
        public string beginDate { get; set; }
        public string endDate { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Models.ViewModels
{
    public class WalkerProfileViewModel
    {
        public Walker Walker { get; set; }

        public List<Walks> Walks { get; set; }
        

        private String GetWalksDurationTotal()
        {
            double total = 0;
            double totalHours = 0;
            double totalMinutes = 0;
            foreach (Walks walk in Walks)
            {
                total += walk.DurationInMinutes;
                totalHours = Math.Floor(total / 60);
                totalMinutes = total % 60;
            }

            string AllWalksDuration = $"{totalHours} hours and {totalMinutes}";
            return AllWalksDuration;
        }
        public string TotalDuration { get { return GetWalksDurationTotal(); } }
    }
}

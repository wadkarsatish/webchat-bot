using System;

namespace BotBuilder.Samples.AdaptiveCards.Models
{
    public class Appointment
    {
        public Guid AppointmentId { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public int PatientId { get; set; }
        public bool IsActive { get; set; }
    }
}
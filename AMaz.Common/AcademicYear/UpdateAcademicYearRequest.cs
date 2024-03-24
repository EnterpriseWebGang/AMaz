namespace AMaz.Common
{
    public class UpdateAcademicYearRequest
    {
        public string AcademicYearId { get; set; }

        public DateTime DateTimeFrom { get; set; }

        public DateTime DateTimeTo { get; set; }
    }
}

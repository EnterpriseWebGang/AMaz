namespace AMaz.Common
{
    public class UpdateMagazineRequest
    {
        public string MagazineId { get; set; }
        public string Name { get; set; }
        public DateTime FirstClosureDate { get; set; }
        public DateTime FinalClosureDate { get; set; }
        public string AcademicYearId { get; set; }
        public string FacultyId { get; set; }

    }
}

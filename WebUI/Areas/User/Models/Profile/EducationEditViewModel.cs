namespace WebUI.Areas.User.Models.Profile
{
    public class EducationEditViewModel
    {
        public int Id { get; set; }
        public string SchoolName { get; set; }
        public string Field { get; set; }
        public string Degree { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}

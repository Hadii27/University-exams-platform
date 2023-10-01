using E_Exam.Models;

namespace E_Exam.Dto
{
    public class ExamDto
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public decimal Duration { get; set; }

    }
}

using ProgramInformationV2.Data.DataContext;
using ProgramInformationV2.Search.Models;

namespace ProgramInformationV2.Data.DataHelpers {

    public class FacultyNameCourseHelper(ProgramRepository? programRepository) {
        private readonly ProgramRepository _programRepository = programRepository ?? throw new ArgumentNullException("programRepository");

        public async Task<Course> AddFaculty(Course course) {
            if (course == null || course.Sections == null) {
                return course ?? new Course();
            }
            foreach (var section in course.Sections) {
                foreach (var faculty in section.FacultyNameList) {
                    if (!string.IsNullOrWhiteSpace(faculty.Name)) {
                        var name = await _programRepository.ReadAsync(pr => pr.FacultyNames.FirstOrDefault(fn => fn.Name == faculty.Name));
                        if (name != null) {
                            faculty.NetId = name.NetId;
                            faculty.ShowInProfile = true;
                            faculty.Url = name.ProfileUrl;
                        }
                    }
                }
            }
            return course;
        }
    }
}
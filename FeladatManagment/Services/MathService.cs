
using FeladatLibrary.Models.Math;
using FeladatLibrary.ViewModels;

using FeladatManagment.Services;

namespace FeladatManagment.Services
{
	public class MathService(TeszterApiService teszterApiService)
    {
		private readonly TeszterApiService _service = teszterApiService;
		public string ErrorMessage = string.Empty;

  //      public async Task<List<TaskList>> GetTaskList() => await _service.Get<List<TaskList>>("Math");
		//public async Task<List<Lesson>> SetLessons(List<Lesson> lessons) => await _service.Post("Math", lessons);
		//public async Task<List<ClassLesson>> SetClassLessons(List<ClassLesson> lessons) => await _service.Post("Math/Class", lessons);
		//public async Task<List<HasLesson>> HasLessons(List<HasLesson> hasLessons) => await _service.Post("Math/has", hasLessons);
		public async Task<List<Lesson>> GetLessons(Guid id) => await _service.Get<List<Lesson>>($"Math/{id}");
		public async Task<TaskView> Start(Guid id) => await _service.Get<TaskView>($"Math/Start/{id}");
		public async Task<TaskRecive> Result(TaskRecive taskRecivie) => await _service.Post("Math/Result", taskRecivie);
		//public async Task<TaskView> ClassLesson(Guid classId) => await _service.Get<TaskView>($"Math/Class/{classId}");
		//public async Task<List<ClassLesson>> GetClassLessons(Guid classId) => await _service.Get<List<ClassLesson>>($"Math/ClassLessons/{classId}");
		//public async Task<List<ClassRecive>> ClassResult(List<ClassRecive> classRecivies) => await _service.Post("Math/ClassResult", classRecivies);
		//public async Task<List<Lesson>> UpdateClassLessons(List<Lesson> lessons) => await _service.Put("Math", lessons);
		//public async Task<List<ResultClassView>> ResultClassView(string ids) => await _service.Get<List<ResultClassView>>($"Math/ResultClass/{ids}");
		//public async Task<List<ResultStudentView>> ResultStudentViews(string id) => await _service.Get<List<ResultStudentView>>($"Math/ResultStudent/{id}");
		//public async Task<List<ResultTaskView>> ResultTaskViews(int id) => await _service.Get<List<ResultTaskView>>($"Math/ResultTask/{id}");
		public async Task<List<Guid>> GetGroups() => await _service.Get<List<Guid>>("Math/Groups");
    }
}

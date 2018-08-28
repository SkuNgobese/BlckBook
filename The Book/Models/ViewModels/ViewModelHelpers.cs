using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace The_Book.Models.ViewModels
{
    public static class ViewModelHelpers
    {
        public static SATeacherViewModel ToViewModel(this Teacher teacher)
        {
            var teacherViewModel = new SATeacherViewModel
            {
                ApplicationUser = teacher.ApplicationUser
            };
            return teacherViewModel;
        }
        public static Teacher ToDomainModel(this SATeacherViewModel teacherViewModel)
        {
            var teacher = new Teacher();
            teacher.ApplicationUser = teacher.ApplicationUser;

            return teacher;
        }
    }
}
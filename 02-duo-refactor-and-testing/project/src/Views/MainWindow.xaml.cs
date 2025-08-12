using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CourseApp.Models;
using CourseApp.ViewModels;
using Microsoft.UI.Xaml;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1010 // Opening square brackets should be spaced correctly

namespace CourseApp.Views
{
    [ExcludeFromCodeCoverage]
    public sealed partial class MainWindow : Window
    {
        private readonly Dictionary<int, CourseViewModel> courseVMCache = [];
        private CourseViewModel? currentCourseVM;
        public static MainWindow? Instance { get; private set; }

        public MainWindow()
        {
            this.InitializeComponent();
            Instance = this;

            MainFrame.Navigated += OnMainFrameNavigated;
            MainFrame.Navigate(typeof(MainPage));
        }

        /// <summary>
        /// Called every time the MainFrame navigates to a new page.
        /// Controls which course timer should start or stop depending on navigation:
        /// - Pauses the previous course's timer if switching
        /// - Starts the current course timer only
        /// - Resumes timer from module to course and vice versa
        /// </summary>
        private void OnMainFrameNavigated(object sender, Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            // Navigated to a course page
            if (e.Parameter is Course course)
            {
                if (!courseVMCache.TryGetValue(course.CourseId, out var newVM))
                {
                    newVM = new CourseViewModel(course);
                    courseVMCache[course.CourseId] = newVM;
                }

                // Only pause previous timer if switching courses
                if (currentCourseVM != null && currentCourseVM != newVM)
                {
                    currentCourseVM.PauseCourseProgressTimer();
                }

                currentCourseVM = newVM;
                currentCourseVM.StartCourseProgressTimer();
            }

            // Navigated to a module page
            else if (e.Parameter is ValueTuple<Module, CourseViewModel> tuple)
            {
                var courseVM = tuple.Item2;

                if (currentCourseVM != null && currentCourseVM != courseVM)
                {
                    currentCourseVM.PauseCourseProgressTimer();
                }

                currentCourseVM = courseVM;
                currentCourseVM.StartCourseProgressTimer();
            }

            // Navigated to something else (like MainPage)
            else
            {
                // Only pause if leaving course/module
                if (e.SourcePageType != typeof(CoursePage) && e.SourcePageType != typeof(ModulePage))
                {
                    currentCourseVM?.PauseCourseProgressTimer();
                }
            }
        }

        public CourseViewModel GetOrCreateCourseViewModel(Course course)
        {
            if (!courseVMCache.TryGetValue(course.CourseId, out var vm))
            {
                vm = new CourseViewModel(course);
                courseVMCache[course.CourseId] = vm;
            }
            return vm;
        }
    }
}

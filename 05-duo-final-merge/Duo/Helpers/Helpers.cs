// <copyright file="Helpers.cs" company="DuoISS">
// Copyright (c) DuoISS. All rights reserved.
// </copyright>

namespace Duo.Helpers
{
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Media;

    /// <summary>
    /// Provides helper methods for working with XAML visual trees.
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Searches up the visual tree to find the first parent of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of parent to search for.</typeparam>
        /// <param name="child">The starting child element.</param>
        /// <returns>
        /// The first parent of type <typeparamref name="T"/> if found; otherwise, <c>null</c>.
        /// </returns>
        public static T? FindParent<T>(DependencyObject child)
            where T : DependencyObject
        {
            DependencyObject? parent = VisualTreeHelper.GetParent(child);
            while (parent != null)
            {
                if (parent is T match)
                {
                    return match;
                }

                parent = VisualTreeHelper.GetParent(parent);
            }

            return null;
        }
    }
}

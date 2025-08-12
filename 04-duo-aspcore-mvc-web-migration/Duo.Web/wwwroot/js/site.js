// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener('DOMContentLoaded', function() {
    window.addEventListener('themeChanged', function(e) {
        const isDark = e.detail.isDark;
        updateThemeForAllPages(isDark);
    });
    
    const pageThemeToggles = document.querySelectorAll('.page-theme-toggle');
    pageThemeToggles.forEach(toggle => {
        toggle.addEventListener('change', function() {
            const isDark = this.checked;
            // Dispatch event to notify layout toggles
            window.dispatchEvent(new CustomEvent('themeChanged', { 
                detail: { isDark: isDark } 
            }));
            updateThemeForAllPages(isDark);
        });
        
        toggle.checked = document.documentElement.classList.contains('dark-mode');
    });
});

function updateThemeForAllPages(isDark) {
    if (isDark) {
        document.documentElement.classList.add('dark-mode');
        localStorage.setItem('theme', 'dark');
    } else {
        document.documentElement.classList.remove('dark-mode');
        localStorage.setItem('theme', 'light');
    }
    
    const allThemeToggles = document.querySelectorAll('input[type="checkbox"].theme-toggle');
    allThemeToggles.forEach(toggle => {
        toggle.checked = isDark;
    });
}

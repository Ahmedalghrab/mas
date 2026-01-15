// Dark Mode Toggle Script

window.darkModeManager = {
    init: function() {
        // Check for saved theme preference or default to 'light' mode
        const currentTheme = localStorage.getItem('theme') || 'light';
        document.documentElement.setAttribute('data-theme', currentTheme);
        
        this.updateIcon(currentTheme);
    },
    
    toggle: function() {
        const currentTheme = document.documentElement.getAttribute('data-theme');
        const newTheme = currentTheme === 'dark' ? 'light' : 'dark';
        
        document.documentElement.setAttribute('data-theme', newTheme);
        localStorage.setItem('theme', newTheme);
        
        this.updateIcon(newTheme);
        
        return newTheme;
    },
    
    updateIcon: function(theme) {
        const icon = document.querySelector('.dark-mode-toggle i');
        if (icon) {
            icon.className = theme === 'dark' ? 'bi bi-sun-fill' : 'bi bi-moon-stars-fill';
        }
    },
    
    getCurrentTheme: function() {
        return document.documentElement.getAttribute('data-theme') || 'light';
    }
};

// Initialize on page load
document.addEventListener('DOMContentLoaded', function() {
    window.darkModeManager.init();
});

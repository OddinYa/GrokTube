// Добавляем класс при открытии сайдбара
document.addEventListener('DOMContentLoaded', function () {
    const sidebarToggle = document.getElementById('sidebarToggle');

    sidebarToggle.addEventListener('click', function () {
        document.body.classList.toggle('sidebar-open');

        // Обновляем сетку после анимации
        setTimeout(() => {
            window.dispatchEvent(new Event('resize'));
        }, 300);
    });
});
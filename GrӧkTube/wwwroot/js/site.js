document.addEventListener('DOMContentLoaded', function () {
    const sidebar = document.getElementById('sidebar');
    const sidebarToggle = document.getElementById('sidebarToggle');
    const backdrop = document.getElementById('backdrop');
    const mainContent = document.querySelector('.main-content');

    function toggleSidebar() {
        const isOpening = !sidebar.classList.contains('active');

        sidebar.classList.toggle('active');
        backdrop.classList.toggle('active');
        mainContent.classList.toggle('with-sidebar');

        // Блокируем скролл страницы при открытом сайдбаре
        document.body.style.overflow = isOpening ? 'hidden' : '';
    }

    sidebarToggle.addEventListener('click', toggleSidebar);
    backdrop.addEventListener('click', toggleSidebar);

   
});


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

$(document).ready(function () {
    $('#authModal form').submit(function (e) {
        e.preventDefault();

        $.ajax({
            url: $(this).attr('action'),
            method: 'POST',
            data: $(this).serialize(),
            success: function (response) {
                if (response.success) {
                    window.location.reload();
                } else {
                    $('#authModal .modal-body').prepend(
                        `<div class="alert alert-danger">${response.error}</div>`
                    );
                }
            }
        });
    });
});


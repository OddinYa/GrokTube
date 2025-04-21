document.addEventListener('DOMContentLoaded', function () {
    // Если цифра не задана на сервере, генерируем случайную
    if (!document.getElementById('captcha-digit').textContent.trim()) {
        const randomDigit = Math.floor(Math.random() * 10).toString();
        document.getElementById('captcha-digit').textContent = randomDigit;
        document.querySelector('input[name="CaptchaExpectedDigit"]').value = randomDigit;
    }

    // Настройка canvas
    const canvas = document.getElementById('captcha-canvas');
    const ctx = canvas.getContext('2d');
    const clearBtn = document.getElementById('clear-canvas');
    let isDrawing = false;

    // Начало рисования
    canvas.addEventListener('mousedown', startDrawing);
    canvas.addEventListener('touchstart', startDrawing);

    // Рисование
    canvas.addEventListener('mousemove', draw);
    canvas.addEventListener('touchmove', draw);

    // Окончание рисования
    canvas.addEventListener('mouseup', stopDrawing);
    canvas.addEventListener('touchend', stopDrawing);
    canvas.addEventListener('mouseout', stopDrawing);

    // Очистка canvas
    clearBtn.addEventListener('click', clearCanvas);

    function startDrawing(e) {
        isDrawing = true;
        draw(e);
    }

    function draw(e) {
        if (!isDrawing) return;

        e.preventDefault();

        // Получаем координаты
        const rect = canvas.getBoundingClientRect();
        let x, y;

        if (e.type.includes('touch')) {
            x = e.touches[0].clientX - rect.left;
            y = e.touches[0].clientY - rect.top;
        } else {
            x = e.clientX - rect.left;
            y = e.clientY - rect.top;
        }

        // Рисуем
        ctx.lineWidth = 3;
        ctx.lineCap = 'round';
        ctx.strokeStyle = '#000';

        ctx.lineTo(x, y);
        ctx.stroke();
        ctx.beginPath();
        ctx.moveTo(x, y);
    }

    function stopDrawing() {
        isDrawing = false;
        ctx.beginPath();

        // Сохраняем нарисованное в hidden input
        saveDrawing();
    }

    function clearCanvas() {
        ctx.clearRect(0, 0, canvas.width, canvas.height);
        document.getElementById('drawn-digit').value = '';
    }

    function saveDrawing() {
        // В реальном приложении здесь можно добавить анализ рисунка
        // Для простоты мы просто сохраняем факт того, что пользователь что-то нарисовал
        document.getElementById('drawn-digit').value = 'drawn';
    }
});
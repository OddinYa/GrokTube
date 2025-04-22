document.addEventListener('DOMContentLoaded', function () {
    // ���� ����� �� ������ �� �������, ���������� ���������
    if (!document.getElementById('captcha-digit').textContent.trim()) {
        const randomDigit = Math.floor(Math.random() * 10).toString();
        document.getElementById('captcha-digit').textContent = randomDigit;
        document.querySelector('input[name="CaptchaExpectedDigit"]').value = randomDigit;
    }

    // ��������� canvas
    const canvas = document.getElementById('captcha-canvas');
    const ctx = canvas.getContext('2d');
    const clearBtn = document.getElementById('clear-canvas');
    let isDrawing = false;

    // ������ ���������
    canvas.addEventListener('mousedown', startDrawing);
    canvas.addEventListener('touchstart', startDrawing);

    // ���������
    canvas.addEventListener('mousemove', draw);
    canvas.addEventListener('touchmove', draw);

    // ��������� ���������
    canvas.addEventListener('mouseup', stopDrawing);
    canvas.addEventListener('touchend', stopDrawing);
    canvas.addEventListener('mouseout', stopDrawing);

    // ������� canvas
    clearBtn.addEventListener('click', clearCanvas);

    function startDrawing(e) {
        isDrawing = true;
        draw(e);
    }

    function draw(e) {
        if (!isDrawing) return;

        e.preventDefault();

        // �������� ����������
        const rect = canvas.getBoundingClientRect();
        let x, y;

        if (e.type.includes('touch')) {
            x = e.touches[0].clientX - rect.left;
            y = e.touches[0].clientY - rect.top;
        } else {
            x = e.clientX - rect.left;
            y = e.clientY - rect.top;
        }

        // ������
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

        // ��������� ������������ � hidden input
        saveDrawing();
    }

    function clearCanvas() {
        ctx.clearRect(0, 0, canvas.width, canvas.height);
        document.getElementById('drawn-digit').value = '';
    }

    function saveDrawing() {
        // � �������� ���������� ����� ����� �������� ������ �������
        // ��� �������� �� ������ ��������� ���� ����, ��� ������������ ���-�� ���������
        document.getElementById('drawn-digit').value = 'drawn';
    }
});
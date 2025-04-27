// Добавляем "особенные" функции

function randomizeOptions(select) {
    const options = Array.from(select.options);
    options.sort(() => Math.random() - 0.5);
    options.forEach(opt => select.appendChild(opt));
}

function generateRandomPhone(btn) {
    const phone = '+7(' + Array.from({ length: 10 },
        () => Math.floor(Math.random() * 10)).join('')
        .replace(/(\d{3})(\d{3})(\d{2})(\d{2})/, '$1)$2-$3-$4');

    if (confirm('Это ваш номер?\n' + phone)) {
        document.getElementById('PhoneNumber').value = phone;
    } else {
        btn.style.backgroundColor = '#ff0000';
        setTimeout(() => btn.style.backgroundColor = '', 1000);
    }

    document.querySelector('input[name="PhoneNumber"]').value = phone;
    btn.classList.add('btn-success');
    setTimeout(() => btn.classList.remove('btn-success'), 1000);
}

function validateCrazyForm(e) {
    // Всегда показываем ошибку капчи
    document.getElementById('captcha-status').textContent = '❌ Неправильная капча!';
    
    return true;
}

// Инициализация из модели при загрузке
document.addEventListener('DOMContentLoaded', () => {
    const initialUrl = document.getElementById('avatarUrlValue').value;
    if (initialUrl) {
        updateAvatarPreview(initialUrl);
    }
});

function updateAvatarPreview(url) {
    const preview = document.getElementById('avatarPreview');
    const hiddenField = document.getElementById('avatarUrlValue');

    // Валидация URL
    if (url.startsWith('data:image') || isValidImageUrl(url)) {
        preview.src = url;
        hiddenField.value = url;
    } else {
        preview.src = 'data:image/svg+xml,...'; // Заглушка
        hiddenField.value = '';
    }
}

function setAvatarColor(element) {
    const color = element.style.backgroundColor;
    const canvas = document.createElement('canvas');
    canvas.width = 100;
    canvas.height = 100;

    const ctx = canvas.getContext('2d');
    ctx.fillStyle = color;
    ctx.fillRect(0, 0, 100, 100);

    const dataUrl = canvas.toDataURL();
    updateAvatarPreview(dataUrl);
}

function isValidImageUrl(url) {
    return /\.(jpg|jpeg|png|gif|bmp|webp)$/i.test(url) ||
        url.startsWith('http');
}

document.addEventListener('DOMContentLoaded', function () {
    const realBtn = document.getElementById('realSubmit');
    const fakeBtns = document.querySelectorAll('.fake-btn');
    const check = document.querySelector("form").checkValidity();


    fakeBtns.forEach(btn => {
        btn.addEventListener('click', function () {
            if (Math.random() < 0.3) {
                document.getElementById('winSound').play();
                realBtn.style.display = 'block';
                realBtn.scrollIntoView({ behavior: 'smooth' });
                fakeBtns.forEach(f => f.style.display = 'none');
            } else {
                document.getElementById('loseSound').play();
                this.style.backgroundColor = '#dc3545';
                setTimeout(() => {
                    this.style.backgroundColor = '';
                }, 500);
            }
        });
    });
});
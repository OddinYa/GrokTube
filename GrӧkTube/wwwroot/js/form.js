document.addEventListener('DOMContentLoaded', function () {
    const captchaHandler = {
        validate: () => isCaptchaValid,
        regenerate: generateNumbers
    };
    window.captchaHandler = captchaHandler;
    // Инициализация капчи
    const captchaDigit = document.getElementById('captcha-digit');
    const captchaExpectedInput = document.querySelector('input[name="CaptchaExpectedDigit"]');
    const drawnDigitInput = document.getElementById('drawn-digit');

    let expectedDigit; // Переносим в область видимости
    let isCaptchaValid = false;

    if (!captchaDigit.textContent.trim()) {
        const randomDigit = Math.floor(Math.random() * 10).toString();
        captchaDigit.textContent = randomDigit;
        captchaExpectedInput.value = randomDigit;
    }

    // Canvas элементы
    const canvas = document.getElementById('captcha-canvas');
    const ctx = canvas.getContext('2d');
   
    let numberPositions = [];
    let clickCount = 0;
   


    function initCaptcha() {
        const randomDigit = Math.floor(Math.random() * 10).toString();
        captchaDigit.textContent = randomDigit;
        captchaExpectedInput.value = randomDigit;
        expectedDigit = parseInt(randomDigit); 
    }

    // Генерация и отрисовка чисел
    function generateNumbers() {
        
            initCaptcha(); 
            numberPositions = [];
        
        const numbers = Array(3).fill(expectedDigit).concat(
            Array.from({ length: 7 }, () => {
                let digit;
                do { digit = Math.floor(Math.random() * 10); }
                while (digit === expectedDigit);
                return digit;
            })
        );

        shuffleArray(numbers);
        distributeNumbers(numbers);
        drawNumbers();
    }

    function distributeNumbers(numbers) {
        numbers.forEach(digit => {
            let pos, collision, attempts = 0;
            do {
                collision = false;
                pos = {
                    x: Math.random() * (canvas.width - 30) + 15,
                    y: Math.random() * (canvas.height - 30) + 15,
                    digit: digit
                };

                numberPositions.forEach(existing => {
                    const dx = pos.x - existing.x;
                    const dy = pos.y - existing.y;
                    if (Math.sqrt(dx * dx + dy * dy) < 30) collision = true;
                });
            } while (collision && ++attempts < 100);

            numberPositions.push({ ...pos, selected: false });
        });
    }

    function drawNumbers() {
        ctx.clearRect(0, 0, canvas.width, canvas.height);
        ctx.strokeStyle = '#ccc';
        ctx.lineWidth = 2;
        ctx.strokeRect(0, 0, canvas.width, canvas.height);

        numberPositions.forEach(pos => {
            ctx.fillStyle = pos.selected ? '#888' : '#000';
            ctx.font = '24px Arial';
            ctx.fillText(pos.digit, pos.x - 8, pos.y + 8);

            if (pos.selected) {
                ctx.strokeStyle = '#dc3545';
                ctx.lineWidth = 2;
                ctx.beginPath();
                ctx.arc(pos.x, pos.y, 15, 0, Math.PI * 2);
                ctx.stroke();
            }
        });
    }

    // Валидация
    function validateCaptcha() {
        const selected = numberPositions.filter(n => n.selected);
        const correctSelected = selected.filter(n => n.digit === expectedDigit).length;

        isCaptchaValid = selected.length === 3 && correctSelected === 3;

        if (isCaptchaValid) {
            document.getElementById('captcha-status').textContent = '✅ Верно!';
            drawnDigitInput.value = expectedDigit;
        } else {
            document.getElementById('captcha-status').textContent = '❌ Ошибка!';
            drawnDigitInput.value = '';
        }
        return isCaptchaValid;
    }

    // Обработчик кликов
    canvas.addEventListener('click', function (e) {
        const rect = canvas.getBoundingClientRect();
        const x = e.clientX - rect.left;
        const y = e.clientY - rect.top;

        numberPositions.forEach(pos => {
            const dx = x - pos.x;
            const dy = y - pos.y;
            if (Math.sqrt(dx * dx + dy * dy) < 15) {
                pos.selected = !pos.selected;
                clickCount += pos.selected ? 1 : -1;
            }
        });

        drawNumbers();
        document.getElementById('captcha-status').textContent = `Выбрано: ${clickCount}/3`;

        if (clickCount === 3) {
            validateCaptcha(); // Всегда вызываем валидацию
            if (!isCaptchaValid) {
                setTimeout(() => {
                    numberPositions.forEach(n => n.selected = false);
                    clickCount = 0;
                    generateNumbers(); 
                }, 1000);
            }
        } else {
            validateCaptcha(); // Обновляем статус при любом клике
        }
    });
    

    
    function shuffleArray(arr) {
        for (let i = arr.length - 1; i > 0; i--) {
            const j = Math.floor(Math.random() * (i + 1));
            [arr[i], arr[j]] = [arr[j], arr[i]];
        }
    }

    // Инициализация
    initCaptcha();
    generateNumbers();
});


document.addEventListener('DOMContentLoaded', function () {
    // Инициализация маски для телефона
    const phoneInput = document.querySelector('input[type="tel"]');
    if (phoneInput) {
        Inputmask({
            mask: "+7 (999) 999-99-99",
            showMaskOnHover: false,
            clearIncomplete: true
        }).mask(phoneInput);
    }
});



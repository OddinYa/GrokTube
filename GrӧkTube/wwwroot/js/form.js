document.addEventListener('DOMContentLoaded', function () {
    const canvas = document.getElementById('captcha-canvas');
    const ctx = canvas.getContext('2d');
    const expectedDigit = parseInt(document.getElementById('captcha-digit').textContent);
    const drawnDigitInput = document.getElementById('drawn-digit');
    let numberPositions = [];
    let clickCount = 0;
    let validationTriggered = false;

    // Генерация чисел
    function generateNumbers() {
        numberPositions = [];
        const numbers = Array(3).fill(expectedDigit);

        for (let i = 0; i < 7; i++) {
            let randomDigit;
            do {
                randomDigit = Math.floor(Math.random() * 10);
            } while (randomDigit === expectedDigit);
            numbers.push(randomDigit);
        }

        shuffleArray(numbers);
        distributeNumbers(numbers);
        drawNumbers();
    }

    // Распределение чисел
    function distributeNumbers(numbers) {
        numbers.forEach(digit => {
            let pos, collision;
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
            } while (collision);

            numberPositions.push({ ...pos, selected: false });
        });
    }

    // Отрисовка
    function drawNumbers() {
        ctx.clearRect(0, 0, canvas.width, canvas.height);

        // Рамка
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
    function validate() {
        const correctNumbers = numberPositions.filter(n => n.digit === expectedDigit);
        const selectedNumbers = numberPositions.filter(n => n.selected);

        // Проверка условий
        const isCorrectCount = selectedNumbers.length === 3;
        const allCorrectSelected = correctNumbers.every(n => n.selected);
        const noWrongSelected = selectedNumbers.every(n => n.digit === expectedDigit);

        if (isCorrectCount && allCorrectSelected && noWrongSelected) {
            drawnDigitInput.value = expectedDigit;
            document.getElementById('captcha-status').textContent = '✅ Верно!';
            return true;
        }

        document.getElementById('captcha-status').textContent = '❌ Ошибка!';
        drawnDigitInput.value = '';
        return false;
    }

    // Обработчик клика
    canvas.addEventListener('click', function (e) {
        if (validationTriggered) return;

        const rect = canvas.getBoundingClientRect();
        const x = e.clientX - rect.left;
        const y = e.clientY - rect.top;
        let selectionChanged = false;

        numberPositions.forEach(pos => {
            const dx = x - pos.x;
            const dy = y - pos.y;
            if (Math.sqrt(dx * dx + dy * dy) < 15) {
                pos.selected = !pos.selected;
                selectionChanged = true;
                clickCount += pos.selected ? 1 : -1;
            }
        });

        if (selectionChanged) {
            drawNumbers();
            document.getElementById('captcha-status').textContent = `Выбрано: ${clickCount}/3`;

            if (clickCount === 3) {
                validationTriggered = true;
                const isValid = validate();

                if (!isValid) {
                    setTimeout(() => {
                        numberPositions.forEach(n => n.selected = false);
                        clickCount = 0;
                        validationTriggered = false;
                        drawNumbers();
                        document.getElementById('captcha-status').textContent = '';
                    }, 1000);
                    generateNumbers();
                } else {
                    validationTriggered = false;
                }
            }
        }
    });


    // Отправка формы
    document.querySelector('form').addEventListener('submit', function (e) {
        if (!validate()) {
            e.preventDefault();
            e.stopImmediatePropagation();
        }
    });

    // Утилиты
    function shuffleArray(arr) {
        for (let i = arr.length - 1; i > 0; i--) {
            const j = Math.floor(Math.random() * (i + 1));
            [arr[i], arr[j]] = [arr[j], arr[i]];
        }
    }

    // Инициализация
    generateNumbers();
});
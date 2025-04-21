document.addEventListener('DOMContentLoaded', async function () {
    // Canvas initialization
    const canvas = document.getElementById('captcha-canvas');
    const ctx = canvas.getContext('2d');
    let isDrawing = false;

    // Setup drawing context
    ctx.lineWidth = 15;
    ctx.lineCap = 'round';
    ctx.strokeStyle = '#000000';

    // Model loading
    let model = null;

    async function loadModel() {
        model = await tf.loadLayersModel('https://storage.googleapis.com/tfjs-models/tfjs/iris_v1/model.json');
        console.log('Model loaded');
    }

    await loadModel();

    // Drawing handlers
    canvas.addEventListener('mousedown', startDrawing);
    canvas.addEventListener('mousemove', draw);
    canvas.addEventListener('mouseup', endDrawing);
    canvas.addEventListener('mouseout', endDrawing);

    // Touch support
    canvas.addEventListener('touchstart', startDrawing);
    canvas.addEventListener('touchmove', draw);
    canvas.addEventListener('touchend', endDrawing);

    function startDrawing(e) {
        isDrawing = true;
        const pos = getMousePos(canvas, e);
        ctx.beginPath();
        ctx.moveTo(pos.x, pos.y);
        e.preventDefault();
    }

    function draw(e) {
        if (!isDrawing) return;
        const pos = getMousePos(canvas, e);
        ctx.lineTo(pos.x, pos.y);
        ctx.stroke();
        predictDigit();
        e.preventDefault();
    }

    function endDrawing() {
        isDrawing = false;
        ctx.beginPath();
    }

    // Clear button
    document.getElementById('clear-canvas').addEventListener('click', () => {
        ctx.clearRect(0, 0, canvas.width, canvas.height);
        document.getElementById('drawn-digit').value = '';
        updateCaptchaStatus('');
    });

    // Helper functions
    function getMousePos(canvas, e) {
        const rect = canvas.getBoundingClientRect();
        let clientX, clientY;

        if (e.touches) {
            clientX = e.touches[0].clientX;
            clientY = e.touches[0].clientY;
        } else {
            clientX = e.clientX;
            clientY = e.clientY;
        }

        return {
            x: (clientX - rect.left) * (canvas.width / rect.width),
            y: (clientY - rect.top) * (canvas.height / rect.height)
        };
    }

    async function predictDigit() {
        if (!model) return;

        // Preprocess canvas image
        const tensor = tf.tidy(() => {
            return tf.browser.fromPixels(canvas)
                .resizeNearestNeighbor([28, 28])
                .mean(2)
                .expandDims(2)  // Добавляем размерность канала -> [28, 28, 1]
                .expandDims()   // Добавляем батч-размерность -> [1, 28, 28, 1]
                .toFloat()
                .div(255.0);
        });

        try {
            // Make prediction
            const prediction = await model.predict(tensor).data();
            const digit = prediction.indexOf(Math.max(...prediction));

            // Update hidden field and status
            document.getElementById('drawn-digit').value = digit;
            validateCaptcha(digit);
        } catch (error) {
            console.error('Prediction error:', error);
        } finally {
            tensor.dispose();
        }
    }

    function validateCaptcha(drawnDigit) {
        const expectedDigit = parseInt(document.getElementById('CaptchaExpectedDigit').value);
        const isValid = drawnDigit === expectedDigit;

        updateCaptchaStatus(isValid ? '✓ Правильно' : '✗ Неверно', isValid);
        return isValid;
    }

    function updateCaptchaStatus(text, isValid = false) {
        const status = document.getElementById('captcha-status');
        status.textContent = text;
        status.style.color = isValid ? 'green' : 'red';
    }

    // Form validation override
    document.querySelector('form').addEventListener('submit', async function (e) {
        const expected = parseInt(document.getElementById('CaptchaExpectedDigit').value);
        const drawn = parseInt(document.getElementById('drawn-digit').value);

        if (!validateCaptcha(drawn)) {
            e.preventDefault();
            e.stopPropagation();
            alert('Пожалуйста, правильно введите капчу!');
        }

        this.classList.add('was-validated');
    });
});

async function trainModel() {
    const model = tf.sequential({
        layers: [
            tf.layers.flatten({ inputShape: [28, 28, 1] }),
            tf.layers.dense({ units: 128, activation: 'relu' }),
            tf.layers.dense({ units: 10, activation: 'softmax' }),
        ]
    });

    model.compile({
        optimizer: 'adam',
        loss: 'categoricalCrossentropy',
        metrics: ['accuracy']
    });

    const data = new MnistData();
    await data.load();

    const batchSize = 512;
    const trainIterations = 50;

    model.fit(data.trainImages, data.trainLabels, {
        batchSize,
        epochs: trainIterations,
        validationData: [data.testImages, data.testLabels],
        shuffle: true
    });

    await model.save('downloads://mnist-model');
}
// Test JS
const canvas = document.getElementById('frameCanvas');
const ctx = canvas.getContext('2d');

// Draw a simple 2D frame example
ctx.strokeStyle = 'blue';
ctx.lineWidth = 3;
ctx.beginPath();
ctx.moveTo(100, 500);
ctx.lineTo(100, 100);
ctx.lineTo(700, 100);
ctx.lineTo(700, 500);
ctx.closePath();
ctx.stroke();

ctx.fillStyle = 'red';
ctx.fillRect(95, 495, 10, 10); // node example
ctx.fillRect(695, 495, 10, 10);


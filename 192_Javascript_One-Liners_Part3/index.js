//1. Get the day of the year from a date
//Do you want to know the day of the year a certain date is?

const dayOfYear = (date) => Math.floor((date - new Date(date.getFullYear(), 0, 0)) / (1000 * 60 * 60 * 24))

console.log(
  dayOfYear(new Date()) // 74
);






//2. Generate a random hex color
//If you need a random color value, this function will do.

const randomColor = () => `#${Math.random().toString(16).slice(2, 8).padEnd(6, '0')}`

console.log(
  randomColor() // #9dae4f
);
console.log(
  randomColor() // #6ef10e
);



//3. Convert RGB color to hex

const rgbToHex = (r, g, b) => "#" + ((1 << 24) + (r << 16) + (g << 8) + b).toString(16).slice(1)

console.log(
  rgbToHex(255, 255, 255)  // '#ffffff'
);




//4. Detect dark mode
const isDarkMode = window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches
console.log(isDarkMode);




//5. Swap two variables
//[foo, bar] = [bar, foo]


//7. pause for a while
const pause = (millis) => new Promise(resolve => setTimeout(resolve, millis))
const fn = async () => {
  await pause(1000)
  console.log('fatfish') // 1s later
}
fn()